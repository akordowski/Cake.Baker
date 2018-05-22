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
        ExecuteTestTool(context =>
        {
            EnsureDirectoryExists(Build.Paths.Directories.Logs);

            context.NUnit3(
                GetTestFiles(Build.Paths.Directories.PublishedNUnitTests),
                new NUnit3Settings
                {
                    NoHeader = true,
                    NoResults = false,
                    OutputFile = Build.Paths.Files.NUnitOutputFile,
                    Results = new[] { new NUnit3Result { FileName = Build.Paths.Files.NUnitTestResults } }
                });
        });
    }));

Task("TestXUnit")
    .IsDependentOn("InstallOpenCover")
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
                    OutputDirectory = Build.Paths.Directories.TestResults,
                    ReportName = "XUnitTestResults"
                });
        });
    }));

Task("TestMSTest")
    .IsDependentOn("InstallOpenCover")
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

Task("TestFixie")
    .IsDependentOn("InstallOpenCover")
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

Task("DotNetCoreTest")
    .IsDependentOn("InstallOpenCover")
    .WithCriteria(() => DotNetCoreTestProjects.Any())
    .Does(() =>
    {
        foreach (var project in DotNetCoreTestProjects)
        {
            ExecuteTestTool(context =>
            {
                context.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
                {
                    Configuration = Build.Parameters.Configuration,
                    NoBuild = true
                });
            });
        }
    });

Task("RunReportGenerator")
    .WithCriteria(() => Build.Parameters.ShouldRunReportGenerator)
    .WithCriteria(() => FileExists(Build.Paths.Files.OpenCover))
    .Does(() => RequireTool(ReportGeneratorTool, () =>
    {
        if (Build.Parameters.IsRunningOnWindows)
        {
            CleanDirectory(Build.Paths.Directories.TestReportGenerator);
            ReportGenerator(
                Build.Paths.Files.OpenCover,
                Build.Paths.Directories.TestReportGenerator);
        }
    }));

Task("RunReportUnit")
    .WithCriteria(() => Build.Parameters.ShouldRunReportUnit)
    .Does(() => RequireTool(ReportUnitTool, () =>
    {
        if (Build.Parameters.IsRunningOnWindows)
        {
            CleanDirectory(Build.Paths.Directories.TestReportUnit);
            ReportUnit(
                Build.Paths.Directories.TestResults,
                Build.Paths.Directories.TestReportUnit,
                new ReportUnitSettings());
        }
    }));

Task("Test")
    .IsDependentOn("TestNUnit")
    .IsDependentOn("TestXUnit")
    .IsDependentOn("TestMSTest")
    .IsDependentOn("TestFixie")
    // .IsDependentOn("DotNetCoreTest")
    .IsDependentOn("RunReportGenerator")
    .IsDependentOn("RunReportUnit");

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
    return GetFiles(directory + "/**/*.Tests.dll");
}