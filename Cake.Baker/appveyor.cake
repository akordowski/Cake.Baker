/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.AppVeyorPrintEnvironmentVariablesTask = Task("AppVeyorPrintEnvironmentVariables")
    .WithCriteria(() => Build.Parameters.IsRunningOnAppVeyor)
    .WithCriteria(() => Build.Parameters.ShouldAppVeyorPrintEnvironmentVariables)
    .Does(() =>
    {
        var values = new Dictionary<string, string>
        {
            { "CI", EnvironmentVariable("CI") },
            { "APPVEYOR_API_URL", EnvironmentVariable("APPVEYOR_API_URL") },
            { "APPVEYOR_PROJECT_ID", EnvironmentVariable("APPVEYOR_PROJECT_ID") },
            { "APPVEYOR_PROJECT_NAME", EnvironmentVariable("APPVEYOR_PROJECT_NAME") },
            { "APPVEYOR_PROJECT_SLUG", EnvironmentVariable("APPVEYOR_PROJECT_SLUG") },
            { "APPVEYOR_BUILD_FOLDER", EnvironmentVariable("APPVEYOR_BUILD_FOLDER") },
            { "APPVEYOR_BUILD_ID", EnvironmentVariable("APPVEYOR_BUILD_ID") },
            { "APPVEYOR_BUILD_NUMBER", EnvironmentVariable("APPVEYOR_BUILD_NUMBER") },
            { "APPVEYOR_BUILD_VERSION", EnvironmentVariable("APPVEYOR_BUILD_VERSION") },
            { "APPVEYOR_PULL_REQUEST_NUMBER", EnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER") },
            { "APPVEYOR_PULL_REQUEST_TITLE", EnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE") },
            { "APPVEYOR_JOB_ID", EnvironmentVariable("APPVEYOR_JOB_ID") },
            { "APPVEYOR_REPO_PROVIDER", EnvironmentVariable("APPVEYOR_REPO_PROVIDER") },
            { "APPVEYOR_REPO_SCM", EnvironmentVariable("APPVEYOR_REPO_SCM") },
            { "APPVEYOR_REPO_NAME", EnvironmentVariable("APPVEYOR_REPO_NAME") },
            { "APPVEYOR_REPO_BRANCH", EnvironmentVariable("APPVEYOR_REPO_BRANCH") },
            { "APPVEYOR_REPO_TAG", EnvironmentVariable("APPVEYOR_REPO_TAG") },
            { "APPVEYOR_REPO_TAG_NAME", EnvironmentVariable("APPVEYOR_REPO_TAG_NAME") },
            { "APPVEYOR_REPO_COMMIT", EnvironmentVariable("APPVEYOR_REPO_COMMIT") },
            { "APPVEYOR_REPO_COMMIT_AUTHOR", EnvironmentVariable("APPVEYOR_REPO_COMMIT_AUTHOR") },
            { "APPVEYOR_REPO_COMMIT_TIMESTAMP", EnvironmentVariable("APPVEYOR_REPO_COMMIT_TIMESTAMP") },
            { "APPVEYOR_SCHEDULED_BUILD", EnvironmentVariable("APPVEYOR_SCHEDULED_BUILD") },
            { "APPVEYOR_FORCED_BUILD", EnvironmentVariable("APPVEYOR_FORCED_BUILD") },
            { "APPVEYOR_RE_BUILD", EnvironmentVariable("APPVEYOR_RE_BUILD") },
            { "PLATFORM", EnvironmentVariable("PLATFORM") },
            { "CONFIGURATION", EnvironmentVariable("CONFIGURATION") }
        };

        Print("", values);
    });

Tasks.AppVeyorUploadArtifactsTask = Task("AppVeyorUploadArtifacts")
    .WithCriteria(() => Build.Parameters.IsRunningOnAppVeyor)
    .WithCriteria(() => Build.Parameters.ShouldAppVeyorUploadArtifacts)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.Packages))
    .Does(() =>
{
    var packages = GetFiles(Build.Paths.Directories.Packages + "/**/*");

    if (packages.Any())
    {
        Information("Uploading Artifacts.");

        foreach (var package in packages)
        {
            AppVeyor.UploadArtifact(package);
        }
    }
});

Tasks.AppVeyorUploadTestResultsTask = Task("AppVeyorUploadTestResults")
    .WithCriteria(() => Build.Parameters.IsRunningOnAppVeyor)
    .WithCriteria(() => Build.Parameters.ShouldAppVeyorUploadTestResults)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.TestResults))
    .Does(() =>
{
    if (FileExists(Build.Paths.Files.FixieTestResults))
    {
        Information("Uploading Fixie Test Results.");
        AppVeyor.UploadTestResults(Build.Paths.Files.FixieTestResults, AppVeyorTestResultsType.XUnit);
    }

    if (FileExists(Build.Paths.Files.MSTestTestResults))
    {
        Information("Uploading MSTest Test Results.");
        AppVeyor.UploadTestResults(Build.Paths.Files.MSTestTestResults, AppVeyorTestResultsType.MSTest);
    }

    if (FileExists(Build.Paths.Files.NUnitTestResults))
    {
        Information("Uploading NUnit Test Results.");
        AppVeyor.UploadTestResults(Build.Paths.Files.NUnitTestResults, AppVeyorTestResultsType.NUnit);
    }

    if (FileExists(Build.Paths.Files.NUnit3TestResults))
    {
        Information("Uploading NUnit3 Test Results.");
        AppVeyor.UploadTestResults(Build.Paths.Files.NUnit3TestResults, AppVeyorTestResultsType.NUnit3);
    }

    if (FileExists(Build.Paths.Files.XUnitTestResults))
    {
        Information("Uploading XUnit Test Results.");
        AppVeyor.UploadTestResults(Build.Paths.Files.XUnitTestResults, AppVeyorTestResultsType.XUnit);
    }
});