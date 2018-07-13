public static class Tasks
{
    // AppVeyor
    public static CakeTaskBuilder AppVeyorPrintEnvironmentVariablesTask { get; set; }
    public static CakeTaskBuilder AppVeyorUploadArtifactsTask { get; set; }
    public static CakeTaskBuilder AppVeyorUploadTestResultsTask { get; set; }

    // GitHub
    public static CakeTaskBuilder PublishGitHubReleaseTask { get; set; }

    // Main
    public static CakeTaskBuilder ShowInfoTask { get; set; }
    public static CakeTaskBuilder CleanTask { get; set; }
    public static CakeTaskBuilder RestoreTask { get; set; }
    public static CakeTaskBuilder BuildTask { get; set; }
    public static CakeTaskBuilder DefaultTask { get; set; }
    public static CakeTaskBuilder TestTask { get; set; }
    public static CakeTaskBuilder ImageTask { get; set; }
    public static CakeTaskBuilder PackageTask { get; set; }
    public static CakeTaskBuilder PublishTask { get; set; }
    public static CakeTaskBuilder SendMessageTask { get; set; }
    public static CakeTaskBuilder LocalTask { get; set; }
    public static CakeTaskBuilder AppVeyorTask { get; set; }

    // NuGet
    public static CakeTaskBuilder CreateNuGetPackagesTask { get; set; }
    public static CakeTaskBuilder PublishNuGetPackagesTask { get; set; }
    public static CakeTaskBuilder PublishMyGetPackagesTask { get; set; }

    // Tests
    public static CakeTaskBuilder InstallOpenCoverTask { get; set; }
    public static CakeTaskBuilder TestNUnit3Task { get; set; }
    public static CakeTaskBuilder TestXUnitTask { get; set; }
    public static CakeTaskBuilder TestMSTestTask { get; set; }
    public static CakeTaskBuilder TestFixieTask { get; set; }
    public static CakeTaskBuilder DotNetCoreTestTask { get; set; }
    public static CakeTaskBuilder RunReportGeneratorTask { get; set; }
    public static CakeTaskBuilder RunReportUnitTask { get; set; }

    // Twitter
    public static CakeTaskBuilder SendMessageToTwitterTask { get; set; }
}