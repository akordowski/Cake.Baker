/* ---------------------------------------------------------------------------------------------------- */
/* Global Methods */

public void CopyBuildOutput()
{
    BlankLine();
    Information("Copying build output...");

    foreach(var project in ParseSolution(Build.Paths.Files.Solution).GetProjects())
    {
        var buildPlatformTarget = Build.ToolSettings.BuildPlatformTarget;
        var platformTarget = buildPlatformTarget == PlatformTarget.MSIL ? "AnyCPU" : buildPlatformTarget.ToString();
        var projectPath = project.Path.FullPath.ToLower();
        var parsedProject = ParseProject(project.Path, Build.Parameters.Configuration, platformTarget);

        if (projectPath.Contains("wixproj"))
        {
            Warning("Skipping wix project");
            continue;
        }

        if (projectPath.Contains("shproj"))
        {
            Warning("Skipping shared project");
            continue;
        }

        if (parsedProject.AssemblyName == null || parsedProject.OutputType == null || parsedProject.OutputPaths.Length == 0)
        {
            Information("AssemblyName:      {0}", parsedProject.AssemblyName);
            Information("OutputType:        {0}", parsedProject.OutputType);
            Information("OutputPaths Count: {0}", parsedProject.OutputPaths.Length);

            throw new Exception($"Unable to parse project file correctly: {project.Path}");
        }

        if (parsedProject.IsNetCore && parsedProject.IsTestProject())
        {
            DotNetCoreTestProjects.Add(parsedProject.ProjectFilePath);
            continue;
        }

        /* -------------------------------------------------- */

        SeparatorLine();
        Information("Input BuildPlatformTarget: {0}", buildPlatformTarget.ToString());
        Information("Using BuildPlatformTarget: {0}", platformTarget);
        BlankLine();

        /* -------------------------------------------------- */

        var isApplication = !parsedProject.IsLibrary();
        var isLibrary = parsedProject.IsLibrary();
        var isTestProject = parsedProject.IsTestProject();
        var isWebApplication = parsedProject.IsWebApplication();

        var isNUnitProject = ContainsReference(parsedProject, "nunit");
        var isXUnitProject = ContainsReference(parsedProject, "xunit");
        var isMSTestProject = ContainsReference(parsedProject, "mstest", "unittestframework", "visualstudio.testplatform");
        var isFixieProject = ContainsReference(parsedProject, "fixie");

        string info = null;
        DirectoryPath outputFolder = null;

        if (isApplication)
        {
            info = "Project has an output type of {0} application: {1}";
            outputFolder = Build.Paths.Directories.PublishedApplications;
        }
        else if (isWebApplication)
        {
            info = "Project has an output type of {0} web application: {1}";
            outputFolder = Build.Paths.Directories.PublishedWebApplications;
        }
        else if (isTestProject && isNUnitProject)
        {
            info = "Project has an output type of {0} library and is a NUnit Test Project: {1}";
            outputFolder = Build.Paths.Directories.PublishedNUnitTests;
        }
        else if (isTestProject && isXUnitProject)
        {
            info = "Project has an output type of {0} library and is a XUnit Test Project: {1}";
            outputFolder = Build.Paths.Directories.PublishedXUnitTests;
        }
        else if (isTestProject && isMSTestProject)
        {
            info = "Project has an output type of {0} library and is a MSTest Test Project: {1}";
            outputFolder = Build.Paths.Directories.PublishedMSTestTests;
        }
        else if (isTestProject && isFixieProject)
        {
            info = "Project has an output type of {0} library and is a Fixie Test Project: {1}";
            outputFolder = Build.Paths.Directories.PublishedFixieTests;
        }
        else if (isLibrary)
        {
            info = "Project has an output type of {0} library: {1}";
            outputFolder = Build.Paths.Directories.PublishedLibraries;
        }

        var projectTarget = GetProjectTarget(parsedProject);
        var assemblyName = parsedProject.AssemblyName;

        Information(info, projectTarget, assemblyName);

        foreach (var outputPath in parsedProject.OutputPaths)
        {
            var files = GetFiles(outputPath.FullPath + "/**/*");
            outputFolder = outputFolder.Combine(assemblyName);

            if (parsedProject.IsVS2017ProjectFormat)
            {
                outputFolder = outputFolder.Combine(outputPath.GetDirectoryName());
            }

            CleanDirectory(outputFolder);
            CopyFiles(files, outputFolder, true);
        }
    }
}

private bool ContainsReference(CustomProjectParserResult parsedProject, params string[] values)
{
    List<string> references = new List<string>();
    references.AddRange(parsedProject.References.Select(o => o.Include.ToLower()));
    references.AddRange(parsedProject.PackageReferences.Select(p => p.Name.ToLower()));

    foreach(var reference in references)
    {
        foreach(var value in values)
        {
            if (reference.Contains(value))
            {
                return true;
            }
        }
    }

    return false;
}

private string GetProjectTarget(CustomProjectParserResult parsedProject)
{
    var target = "";

    if (parsedProject.IsNetCore)
    {
        target = ".Net Core";
    }
    else if (parsedProject.IsNetFramework)
    {
        target = ".Net Framework";
    }
    else if (parsedProject.IsNetStandard)
    {
        target = ".Net Standard";
    }

    return target;
}

public void BlankLine()
{
    Information("");
}

public void SeparatorLine(int count = 100)
{
    Information(new String('-', count));
}

public void Print(object obj, bool print)
{
    if (print)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();
        var padCount = properties.Select(p => p.Name.Length).Max() + 2;

        BlankLine();
        Information(type.Name);
        SeparatorLine(50);

        foreach (var property in properties)
        {
            Information((property.Name + ":").PadRight(padCount) + "{0}", property.GetValue(obj, null));
        }
    }
}

public void Print(string name, Dictionary<string, string> items)
{
    var padCount = items.Keys.Select(key => key.Length).Max() + 2;

    if (!String.IsNullOrWhiteSpace(name))
    {
        BlankLine();
        Information(name);
        SeparatorLine(50);
    }

    foreach (var item in items)
    {
        Information((item.Key + ":").PadRight(padCount) + "{0}", item.Value);
    }
}

public bool ToolDirectoryExists(string tool)
{
    var toolName = System.Text.RegularExpressions.Regex.Replace(tool, @"#tool nuget:\?package=(.+)&version=(.+)", "$1.$2");
    var exists = System.IO.Directory.Exists("tools" + System.IO.Path.DirectorySeparatorChar + toolName);

    return exists;
}

public void RequireTool(string value, Action action)
{
    RequireTool(new[] { value }, action);
}

public void RequireTool(string[] values, Action action)
{
    values = values.Where(o => !ToolDirectoryExists(o)).ToArray();

    if (values.Any())
    {
        var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));

        try
        {
            System.IO.File.WriteAllLines(script.FullPath, values);
            var arguments = new Dictionary<string, string>();
            var cake = Build.Parameters.CakeConfiguration;

            if (cake.GetValue("NuGet_UseInProcessClient") != null)
            {
                arguments.Add("nuget_useinprocessclient", cake.GetValue("NuGet_UseInProcessClient"));
            }

            if (cake.GetValue("Settings_SkipVerification") != null)
            {
                arguments.Add("settings_skipverification", cake.GetValue("Settings_SkipVerification"));
            }

            CakeExecuteScript(script, new CakeSettings
            {
                Arguments = arguments
            });
        }
        finally
        {
            if (FileExists(script))
            {
                DeleteFile(script);
            }
        }
    }

    action();
}