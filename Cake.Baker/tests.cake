/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Task("TestNUnit")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PublishedNUnitTests))
    .Does(() => RequireTool(new[] { NUnitTool, OpenCoverTool }, () =>
    {
        if (Build.Parameters.IsRunningOnWindows)
        {
            CleanDirectory(Build.Paths.Directories.TestCoverage);

            OpenCover(tool =>
            {
                var files = GetFiles(Build.Paths.Directories.PublishedNUnitTests + "/**/*.Tests.dll");

                tool.NUnit3(files, new NUnit3Settings
                {
                    NoResults = true
                });
            },
            Build.Paths.Files.TestCoverageOutput,
            new OpenCoverSettings
            {
                OldStyle = true,
                ReturnTargetCodeOffset = 0
            }
            .WithFilter(Build.ToolSettings.TestCoverageFilter)
            .ExcludeByAttribute(Build.ToolSettings.TestCoverageExcludeByAttribute)
            .ExcludeByFile(Build.ToolSettings.TestCoverageExcludeByFile));
        }
    }));

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
    .IsDependentOn("CreateCoverageReport");