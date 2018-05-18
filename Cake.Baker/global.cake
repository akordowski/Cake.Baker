/* ---------------------------------------------------------------------------------------------------- */
/* Global Methods */

public void CopyBuildOutput()
{
    Information("");
    Information("Copying build output...");

    foreach(var project in ParseSolution(Build.Paths.Files.Solution).GetProjects())
    {
        var parsedProject = ParseProject(project.Path, Build.Parameters.Configuration, "AnyCPU");
        var assemblyName = parsedProject.AssemblyName;
        var isLibrary = parsedProject.IsLibrary();
        var references = parsedProject.References;

        if (parsedProject.AssemblyName == null || parsedProject.OutputType == null || parsedProject.OutputPaths?.Length == 0)
        {
            Information("AssemblyName: {0}", parsedProject.AssemblyName);
            Information("OutputType:   {0}", parsedProject.OutputType);
            Information("OutputPaths:  {0}", parsedProject.OutputPaths);

            throw new Exception(string.Format("Unable to parse project file correctly: {0}", project.Path));
        }

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

        DirectoryPath directoryPath = null;

        if (isLibrary && isNUnitProject)
        {
            Information("Project has an output type of library and is a NUnit Test Project: {0}", assemblyName);
            directoryPath = Build.Paths.Directories.PublishedNUnitTests;
        }
        else
        {
            Information("Project has an output type of library: {0}", assemblyName);

            if (parsedProject.IsVS2017ProjectFormat)
            {
                foreach(var outputPath in parsedProject.OutputPaths)
                {
                }
                continue;
            }
            else
            {
                directoryPath = Build.Paths.Directories.PublishedLibraries;
            }
        }

        var files = GetFiles(parsedProject.OutputPaths[0].FullPath + "/**/*");
        var outputFolder = directoryPath.Combine(assemblyName);

        CleanDirectory(outputFolder);
        CopyFiles(files, outputFolder, true);
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