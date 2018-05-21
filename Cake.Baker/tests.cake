/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Task("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.IsRunningOnWindows)
    .Does(() => RequireTool(OpenCoverTool, () => {}));

Task("TestNUnit")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedNUnitTests))
    .Does(() => RequireTool(NUnitTool, () =>
    {
        ExecuteTestTool(Build.Paths.Directories.TestResultsNUnit, context =>
        {
            context.NUnit3(
                GetTestFiles(Build.Paths.Directories.PublishedNUnitTests),
                new NUnit3Settings
                {
                    NoHeader = true,
                    NoResults = true
                });
        });
    }));

Task("TestXUnit")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedXUnitTests))
    .Does(() => RequireTool(XUnitTool, () =>
    {
        ExecuteTestTool(Build.Paths.Directories.TestResultsXUnit, context =>
        {
            context.XUnit2(
                GetTestFiles(Build.Paths.Directories.PublishedXUnitTests),
                new XUnit2Settings
                {
                    OutputDirectory = Build.Paths.Directories.TestResultsXUnit,
                    XmlReport = true,
                    NoAppDomain = true
                });
        });
    }));

Task("TestMSTest")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedMSTestTests))
    .Does(() =>
    {
        ExecuteTestTool(Build.Paths.Directories.TestResultsMSTest, context =>
        {
            context.MSTest(
                GetTestFiles(Build.Paths.Directories.PublishedMSTestTests),
                new MSTestSettings
                {
                    NoIsolation = false
                });
        });
    });

Task("TestFixie")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedFixieTests))
    .Does(() => RequireTool(FixieTool, () =>
    {
        ExecuteTestTool(Build.Paths.Directories.TestResultsFixie, context =>
        {
            context.Fixie(
                GetTestFiles(Build.Paths.Directories.PublishedFixieTests),
                new FixieSettings
                {
                    XUnitXml = Build.Paths.Directories.TestResultsFixie + "TestResult.xml"
                });
        });
    }));

Task("DotNetCoreTest")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DotNetCoreTestProjects.Any())
    .Does(() =>
    {
        foreach (var project in DotNetCoreTestProjects)
        {
            ExecuteTestTool(Build.Paths.Directories.TestResults, context =>
            {
                context.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
                {
                    Configuration = Build.Parameters.Configuration,
                    NoBuild = true
                });
            });
        }
    });

Task("CreateCoverageReport")
    .WithCriteria(() => Build.Parameters.ShouldCreateCoverageReport)
    .WithCriteria(() => FileExists(Build.Paths.Files.TestCoverageOutput))
    .Does(() => RequireTool(ReportGeneratorTool, () =>
    {
        if (Build.Parameters.IsRunningOnWindows)
        {
            CleanDirectory(Build.Paths.Directories.TestReport);
            ReportGenerator(Build.Paths.Files.TestCoverageOutput, Build.Paths.Directories.TestReport);
        }
    }));

Task("Test")
    .IsDependentOn("TestNUnit")
    .IsDependentOn("TestXUnit")
    .IsDependentOn("TestMSTest")
    .IsDependentOn("TestFixie")
    .IsDependentOn("DotNetCoreTest")
    .IsDependentOn("CreateCoverageReport");

/* ---------------------------------------------------------------------------------------------------- */
/* Methods */

private void ExecuteTestTool(DirectoryPath testResultDirectory, Action<ICakeContext> testTool)
{
    EnsureDirectoryExists(Build.Paths.Directories.TestCoverage);
    EnsureDirectoryExists(testResultDirectory);

    if (Build.Parameters.IsRunningOnUnix)
    {
        testTool(Context);
    }
    else if (Build.Parameters.IsRunningOnWindows)
    {
        var coverageOutput = Build.Paths.Files.TestCoverageOutput;

        OpenCover(testTool,
            coverageOutput,
            new OpenCoverSettings
            {
                OldStyle = true,
                ReturnTargetCodeOffset = 0,
                Register = "user",
                MergeOutput = FileExists(coverageOutput)
            }
            .WithFilter(Build.ToolSettings.TestCoverageFilter)
            .ExcludeByAttribute(Build.ToolSettings.TestCoverageExcludeByAttribute)
            .ExcludeByFile(Build.ToolSettings.TestCoverageExcludeByFile));
    }
}

private FilePathCollection GetTestFiles(DirectoryPath directory)
{
    return GetFiles(directory + "/**/*.Tests.dll");
}