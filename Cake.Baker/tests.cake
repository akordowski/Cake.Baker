/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.InstallOpenCoverTask = Task("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.IsRunningOnWindows)
    .WithCriteria(() => Build.Parameters.ShouldRunTests)
    .WithCriteria(() => Build.Parameters.ShouldRunOpenCover)
    .Does(() => RequireTool(OpenCoverTool, () => {}));

Tasks.TestNUnit3Task = Task("TestNUnit3")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.ShouldRunNUnit3Tests)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedNUnit3Tests))
    .Does(() => RequireTool(NUnitTool, () =>
    {
        ExecuteTestTool(context =>
        {
            EnsureDirectoryExists(Build.Paths.Files.NUnit3OutputFile.GetDirectory());

            context.NUnit3(
                GetTestFiles(Build.Paths.Directories.PublishedNUnit3Tests),
                new NUnit3Settings
                {
                    NoHeader = true,
                    NoResults = false,
                    OutputFile = Build.Paths.Files.NUnit3OutputFile,
                    Results = new[] { new NUnit3Result { FileName = Build.Paths.Files.NUnit3TestResults } }
                });
        });
    }));

Tasks.TestXUnitTask = Task("TestXUnit")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.ShouldRunXUnitTests)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedXUnitTests))
    .Does(() => RequireTool(XUnitTool, () =>
    {
        ExecuteTestTool(context =>
        {
            context.XUnit2(
                GetTestFiles(Build.Paths.Directories.PublishedXUnitTests),
                new XUnit2Settings
                {
                    NoAppDomain = true,
                    XmlReport = true,
                    OutputDirectory = Build.Paths.Files.XUnitTestResults.GetDirectory(),
                    ReportName = Build.Paths.Files.XUnitTestResults.GetFilenameWithoutExtension().ToString()
                });
        });
    }));

Tasks.TestMSTestTask = Task("TestMSTest")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.ShouldRunMSTestTests)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedMSTestTests))
    .Does(() =>
    {
        ExecuteTestTool(context =>
        {
            context.MSTest(
                GetTestFiles(Build.Paths.Directories.PublishedMSTestTests),
                new MSTestSettings
                {
                    NoIsolation = false,
                    ResultsFile = Build.Paths.Files.MSTestTestResults.FullPath
                });
        });
    });

Tasks.TestFixieTask = Task("TestFixie")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.ShouldRunFixieTests)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedFixieTests))
    .Does(() => RequireTool(FixieTool, () =>
    {
        ExecuteTestTool(context =>
        {
            context.Fixie(
                GetTestFiles(Build.Paths.Directories.PublishedFixieTests),
                new FixieSettings
                {
                    XUnitXml = Build.Paths.Files.FixieTestResults.FullPath
                });
        });
    }));

Tasks.DotNetCoreTestTask = Task("DotNetCoreTest")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => Build.Parameters.ShouldRunDotNetCoreTest)
    .WithCriteria(() => Build.Projects.DotNetCoreTests.Any())
    .Does(() =>
    {
        foreach (var project in Build.Projects.DotNetCoreTests)
        {
            ExecuteTestTool(context =>
            {
                context.DotNetCoreTest(project.ProjectFilePath.FullPath, new DotNetCoreTestSettings
                {
                    Configuration = Build.Parameters.Configuration,
                    NoBuild = true
                });
            });
        }
    });

Tasks.RunReportGeneratorTask = Task("RunReportGenerator")
    .WithCriteria(() => Build.Parameters.IsRunningOnWindows)
    .WithCriteria(() => Build.Parameters.ShouldRunReportGenerator)
    .WithCriteria(() => FileExists(Build.Paths.Files.OpenCover))
    .Does(() => RequireTool(ReportGeneratorTool, () =>
    {
        CleanDirectory(Build.Paths.Directories.TestReportGenerator);
        ReportGenerator(
            Build.Paths.Files.OpenCover,
            Build.Paths.Directories.TestReportGenerator);
    }));

Tasks.RunReportUnitTask = Task("RunReportUnit")
    .WithCriteria(() => Build.Parameters.IsRunningOnWindows)
    .WithCriteria(() => Build.Parameters.ShouldRunReportUnit)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.TestResults))
    .Does(() => RequireTool(ReportUnitTool, () =>
    {
        CleanDirectory(Build.Paths.Directories.TestReportUnit);
        ReportUnit(
            Build.Paths.Directories.TestResults,
            Build.Paths.Directories.TestReportUnit,
            new ReportUnitSettings());
    }));

/* ---------------------------------------------------------------------------------------------------- */
/* Methods */

private void ExecuteTestTool(Action<ICakeContext> testTool)
{
    EnsureDirectoryExists(Build.Paths.Directories.TestResults);

    if (Build.Parameters.IsRunningOnUnix || !Build.Parameters.ShouldRunOpenCover)
    {
        testTool(Context);
    }
    else if (Build.Parameters.IsRunningOnWindows)
    {
        EnsureDirectoryExists(Build.Paths.Directories.TestCoverage);
        var coverageOutput = Build.Paths.Files.OpenCover;

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
    var files = GetFiles(directory + Build.Parameters.TestFilePattern)
        .Where(f => Build.Projects.DotNetFrameworkTests.Any(p => p.ProjectFilePath.FullPath.Contains(f.GetFilenameWithoutExtension().FullPath)));

    return new FilePathCollection(files, PathComparer.Default);
}