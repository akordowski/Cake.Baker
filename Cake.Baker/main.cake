/* ---------------------------------------------------------------------------------------------------- */
/* Global Variables */

var Build = new Builder(Context, BuildSystem, target => RunTarget(target));
var publishingError = false;

/* ---------------------------------------------------------------------------------------------------- */
/* Setup/Teardown */

Setup(context =>
{
    Information(Figlet(Build.Parameters.Title));
    Information("Starting Setup...");

    Build.Version.CalculateVersion();

    BlankLine();
    Information("Building version {0} of {1} using version {2} of Cake",
        Build.Version.SemVersion,
        Build.Parameters.Title,
        Build.Version.CakeVersion);
});

/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Task("ShowInfo")
    .Does(() =>
    {
        Print(Build.Version, Build.Parameters.PrintVersionInfo);
        Print(Build.Parameters, Build.Parameters.PrintParametersInfo);
        Print(Build.Paths.Directories, Build.Parameters.PrintDirectoriesInfo);
        Print(Build.Paths.Files, Build.Parameters.PrintFilesInfo);
        Print(Build.Environment, Build.Parameters.PrintEnvironmentInfo);
        Print(Build.ToolSettings, Build.Parameters.PrintToolSettingsInfo);
    });

Task("Clean")
    .Does(() =>
    {
        Information("Cleaning...");
        CleanDirectory(Build.Paths.Directories.Artifacts);
    });

Task("Restore")
    .WithCriteria(() => Build.Paths.Files.Solution.Exists())
    .Does(() =>
    {
        Information("Restoring...");
        NuGetRestore(Build.Paths.Files.Solution);
    });

Task("Build")
    .WithCriteria(() => Build.Paths.Files.Solution.Exists())
    .Does(() =>
    {
        Information("Building...");
        BlankLine();
        Information("Input BuildPlatformTarget: {0}", Build.ToolSettings.BuildPlatformTarget);
        Information("Using BuildPlatformTarget: {0}", Build.ToolSettings.UsingBuildPlatformTarget);
        BlankLine();

        var treatWarningsAsErrors = Build.ToolSettings.BuildTreatWarningsAsErrors.ToString().ToLower();

        if (Build.Parameters.IsRunningOnWindows)
        {
            var msBuildSettings = new MSBuildSettings()
                .SetVerbosity(Verbosity.Minimal)
                .SetConfiguration(Build.Parameters.Configuration)
                .SetMaxCpuCount(Build.ToolSettings.BuildMaxCpuCount)
                .WithTarget("Build")
                .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors);

            MSBuild(Build.Paths.Files.Solution, msBuildSettings);
        }
        else
        {
            var xBuildSettings = new XBuildSettings()
                .SetVerbosity(Verbosity.Minimal)
                .SetConfiguration(Build.Parameters.Configuration)
                .WithTarget("Build")
                .WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors);

            XBuild(Build.Paths.Files.Solution, xBuildSettings);
        }

        CopyBuildOutput();
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Execution */

Task("Default")
    .IsDependentOn("AppVeyorPrintEnvironmentVariables")
    .IsDependentOn("ShowInfo")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("CreateNuGetPackages")
    .IsDependentOn("AppVeyorUploadArtifacts")
    .IsDependentOn("AppVeyorUploadTestResults")
    .IsDependentOn("PublishNuGetPackages")
    .IsDependentOn("PublishMyGetPackages")
    .IsDependentOn("PublishGitHubRelease")
    .IsDependentOn("SendMessageToTwitter")
    ;