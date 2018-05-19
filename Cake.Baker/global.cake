/* ---------------------------------------------------------------------------------------------------- */
/* Global Methods */

public void CopyBuildOutput()
{
    Information("");
    Information("Copying build output...");

    foreach(var project in ParseSolution(Build.Paths.Files.Solution).GetProjects())
    {
        var buildPlatformTarget = Build.ToolSettings.BuildPlatformTarget;
        var platformTarget = buildPlatformTarget == PlatformTarget.MSIL ? "AnyCPU" : buildPlatformTarget.ToString();

        var parsedProject = ParseProject(project.Path, Build.Parameters.Configuration, platformTarget);
        var projectPath = project.Path.FullPath.ToLower();
        var assemblyName = parsedProject.AssemblyName;
        var references = parsedProject.References;
        var isLibrary = parsedProject.IsLibrary();

        Information("----------------------------------------------------------------------------------------------------");
        Information("Input BuildPlatformTarget: {0}", buildPlatformTarget.ToString());
        Information("Using BuildPlatformTarget: {0}", platformTarget);

        if(projectPath.Contains("wixproj"))
        {
            Warning("Skipping wix project");
            continue;
        }

        if(projectPath.Contains("shproj"))
        {
            Warning("Skipping shared project");
            continue;
        }

        if (assemblyName == null || parsedProject.OutputType == null || parsedProject.OutputPaths.Length == 0)
        {
            Information("AssemblyName:      {0}", assemblyName);
            Information("OutputType:        {0}", parsedProject.OutputType);
            Information("OutputPaths Count: {0}", parsedProject.OutputPaths.Length);

            throw new Exception(string.Format("Unable to parse project file correctly: {0}", project.Path));
        }

        /* -------------------------------------------------- */

        var isNUnitProject = false;

        foreach (var reference in references)
        {
            Verbose("Reference Include: {0}", reference.Include);

            var include = reference.Include.ToLower();

            if (include.Contains("nunit.framework"))
            {
                isNUnitProject = true;
                break;
            }
        }

        /* -------------------------------------------------- */

        DirectoryPath outputFolder = null;

        if (isLibrary && isNUnitProject)
        {
            Information("Project has an output type of library and is a NUnit Test Project: {0}", assemblyName);
            outputFolder = Build.Paths.Directories.PublishedNUnitTests;
        }
        else
        {
            Information("Project has an output type of library: {0}", assemblyName);
            outputFolder = Build.Paths.Directories.PublishedLibraries;
        }

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

public void Print(object obj, bool print)
{
    if (print)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();
        var padCount = properties.Select(p => p.Name.Length).Max() + 2;

        Context.Information("");
        Context.Information(type.Name);
        Context.Information("----------------------------------------");

        foreach (var property in properties)
        {
            Context.Information((property.Name + ":").PadRight(padCount) + "{0}", property.GetValue(obj, null));
        }
    }
}

public void Print(string name, Dictionary<string, string> items)
{
    var padCount = items.Keys.Select(key => key.Length).Max() + 2;

    if (!String.IsNullOrWhiteSpace(name))
    {
        Context.Information("");
        Context.Information(name);
        Context.Information("----------------------------------------");
    }

    foreach (var item in items)
    {
        Context.Information((item.Key + ":").PadRight(padCount) + "{0}", item.Value);
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