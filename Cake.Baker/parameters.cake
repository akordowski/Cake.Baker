public class Parameters
{
    public string Target { get; }
    public string Configuration { get; }
    public Cake.Core.Configuration.ICakeConfiguration CakeConfiguration { get; }
    public ReleaseNotes ReleaseNotes => _builder.Paths.Files.ReleaseNotes.Exists()
        ? _context.ParseReleaseNotes(_builder.Paths.Files.ReleaseNotes)
        : null;

    public string Title { get; }
    public string RepositoryOwner { get; }
    public string RepositoryName { get; }
    public string RepositoryFullName { get; }
    public string RepositoryBranch { get; }
    public bool IsPrerelease { get; }

    public bool IsMainRepository { get; }
    public bool IsPublicRepository { get; }
    public bool IsMasterBranch { get; }
    public bool IsDevelopBranch { get; }
    public bool IsReleaseBranch { get; }
    public bool IsHotFixBranch { get; }
    public bool IsPullRequest { get; }
    public bool IsTagged { get; }

    public bool IsLocalBuild { get; }
    public bool IsRunningOnUnix { get; }
    public bool IsRunningOnWindows { get; }
    public bool IsRunningOnAppVeyor { get; }

    public bool ShouldRunGitVersion { get; }
    public bool ShouldRunDocFx { get; }

    public bool ShouldRunTests { get; }
    public bool ShouldRunIntegrationTests { get; }

    public bool ShouldRunNUnit3Tests { get; }
    public bool ShouldRunXUnitTests { get; }
    public bool ShouldRunMSTestTests { get; }
    public bool ShouldRunFixieTests { get; }
    public bool ShouldRunDotNetCoreTest { get; }

    public bool ShouldRunOpenCover { get; }
    public bool ShouldRunReportGenerator { get; }
    public bool ShouldRunReportUnit { get; }

    public bool ShouldPackage { get; }
    public bool ShouldPackageNuGet { get; }

    public bool ShouldPublish { get; }
    public bool ShouldPublishToNuGet { get; }
    public bool ShouldPublishToMyGet { get; }
    public bool ShouldPublishToGitHub { get; }

    public bool CanPublishToNuGet =>
        !String.IsNullOrEmpty(_builder.Credentials.NuGet.ApiKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.NuGet.Source);

    public bool CanPublishToMyGet =>
        !String.IsNullOrEmpty(_builder.Credentials.MyGet.ApiKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.MyGet.Source);

    public bool CanPublishToGitHub =>
        !String.IsNullOrEmpty(_builder.Credentials.GitHub.Username) &&
        !String.IsNullOrEmpty(_builder.Credentials.GitHub.Password);

    public bool ShouldPost { get; }
    public bool ShouldPostToTwitter { get; }

    public bool CanPostToTwitter =>
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.ConsumerKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.ConsumerSecret) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.AccessToken) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.AccessTokenSecret);

    public bool ShouldAppVeyorPrintEnvironmentVariables { get; }
    public bool ShouldAppVeyorUploadArtifacts { get; }
    public bool ShouldAppVeyorUploadTestResults { get; }

    public bool PrintAllInfo { get; }
    public bool PrintVersionInfo { get; }
    public bool PrintParametersInfo { get; }
    public bool PrintMessagesInfo { get; }
    public bool PrintDirectoriesInfo { get; }
    public bool PrintFilesInfo { get; }
    public bool PrintEnvironmentInfo { get; }
    public bool PrintToolSettingsInfo { get; }

    public string TestFilePattern { get; }
    public string TestProjectPattern { get; }
    public string IntegrationTestFilePattern { get; }
    public string IntegrationTestProjectPattern { get; }

    private readonly Builder _builder;
    private readonly ICakeContext _context;
    private readonly BuildSystem _buildSystem;

    public Parameters(
        Builder builder,
        string title,
        string repositoryOwner,
        string repositoryName,
        string repositoryBranch,
        bool isPrerelease,
        bool isPublicRepository,
        bool? shouldRunGitVersion,
        bool shouldRunDocFx,
        bool shouldRunTests,
        bool shouldRunIntegrationTests,
        bool shouldRunNUnit3Tests,
        bool shouldRunXUnitTests,
        bool shouldRunMSTestTests,
        bool shouldRunFixieTests,
        bool shouldRunDotNetCoreTest,
        bool shouldRunOpenCover,
        bool shouldRunReportGenerator,
        bool shouldRunReportUnit,
        bool? shouldPackage,
        bool? shouldPackageNuGet,
        bool? shouldPublish,
        bool? shouldPublishToNuGet,
        bool? shouldPublishToMyGet,
        bool? shouldPublishToGitHub,
        bool? shouldPost,
        bool? shouldPostToTwitter,
        bool shouldAppVeyorPrintEnvironmentVariables,
        bool shouldAppVeyorUploadArtifacts,
        bool shouldAppVeyorUploadTestResults,
        bool? printAllInfo,
        bool? printVersionInfo,
        bool? printParametersInfo,
        bool? printMessagesInfo,
        bool? printDirectoriesInfo,
        bool? printFilesInfo,
        bool? printEnvironmentInfo,
        bool? printToolSettingsInfo,
        string testFilePattern,
        string testProjectPattern,
        string integrationTestFilePattern,
        string integrationTestProjectPattern)
    {
        _builder = builder;
        _context = builder.Context;
        _buildSystem = builder.BuildSystem;

        var env = _buildSystem.AppVeyor.Environment;
        var repo = env.Repository;
        var branch = repo.Branch;
        var pullRequest = env.PullRequest;
        var tag = repo.Tag;

        Target = _context.Argument("target", "Default");
        Configuration = _context.Argument("configuration", "Release");
        CakeConfiguration = _context.GetCakeConfiguration();

        Title = title;
        RepositoryOwner = repositoryOwner;
        RepositoryName = repositoryName.DefaultValue(title);
        RepositoryFullName = $"{RepositoryOwner}/{RepositoryName}";
        RepositoryBranch = repositoryBranch.DefaultValue("master");
        IsPrerelease = isPrerelease;

        IsMainRepository = repo.Name.Equals(RepositoryFullName, StringComparison.OrdinalIgnoreCase);
        IsPublicRepository = isPublicRepository;
        IsMasterBranch = branch.Equals("master", StringComparison.OrdinalIgnoreCase);
        IsDevelopBranch = branch.Equals("develop", StringComparison.OrdinalIgnoreCase);
        IsReleaseBranch = branch.StartsWith("release", StringComparison.OrdinalIgnoreCase);
        IsHotFixBranch = branch.StartsWith("hotfix", StringComparison.OrdinalIgnoreCase);
        IsPullRequest = pullRequest.IsPullRequest;
        IsTagged = tag.IsTag && !String.IsNullOrWhiteSpace(tag.Name);

        IsLocalBuild = _buildSystem.IsLocalBuild;
        IsRunningOnUnix = _context.IsRunningOnUnix();
        IsRunningOnWindows = _context.IsRunningOnWindows();
        IsRunningOnAppVeyor = _buildSystem.AppVeyor.IsRunningOnAppVeyor;

        ShouldRunGitVersion = shouldRunGitVersion ?? _context.IsRunningOnWindows();
        ShouldRunDocFx = shouldRunDocFx;

        ShouldRunTests = shouldRunTests;
        ShouldRunIntegrationTests = shouldRunIntegrationTests;
        ShouldRunNUnit3Tests = shouldRunNUnit3Tests;
        ShouldRunXUnitTests = shouldRunXUnitTests;
        ShouldRunMSTestTests = shouldRunMSTestTests;
        ShouldRunFixieTests = shouldRunFixieTests;
        ShouldRunDotNetCoreTest = shouldRunDotNetCoreTest;

        ShouldRunOpenCover = shouldRunOpenCover;
        ShouldRunReportGenerator = shouldRunReportGenerator;
        ShouldRunReportUnit = shouldRunReportUnit;

        ShouldPackage = shouldPackage ?? true;
        ShouldPackageNuGet = shouldPackageNuGet ?? (shouldPackage ?? true);

        ShouldPublish = shouldPublish ??
            !IsLocalBuild &&
            !IsPullRequest &&
            IsMainRepository &&
            IsTagged;

        ShouldPublishToNuGet = shouldPublishToNuGet ?? ShouldPublish;
        ShouldPublishToMyGet = shouldPublishToMyGet ?? ShouldPublish;
        ShouldPublishToGitHub = shouldPublishToGitHub ?? ShouldPublish;

        ShouldPost = shouldPost ??
            !IsLocalBuild &&
            !IsPullRequest &&
            IsMainRepository &&
            IsTagged;

        ShouldPostToTwitter = shouldPostToTwitter ?? ShouldPost;

        ShouldAppVeyorPrintEnvironmentVariables = shouldAppVeyorPrintEnvironmentVariables;
        ShouldAppVeyorUploadArtifacts = shouldAppVeyorUploadArtifacts;
        ShouldAppVeyorUploadTestResults = shouldAppVeyorUploadTestResults;

        PrintAllInfo = printAllInfo ?? false;
        PrintVersionInfo = printVersionInfo ?? (printAllInfo ?? true);
        PrintParametersInfo = printParametersInfo ?? (printAllInfo ?? true);
        PrintMessagesInfo = printMessagesInfo ?? (printAllInfo ?? false);
        PrintDirectoriesInfo = printDirectoriesInfo ?? (printAllInfo ?? false);
        PrintFilesInfo = printFilesInfo ?? (printAllInfo ?? false);
        PrintEnvironmentInfo = printEnvironmentInfo ?? (printAllInfo ?? false);
        PrintToolSettingsInfo = printToolSettingsInfo ?? (printAllInfo ?? false);

        TestFilePattern = testFilePattern ?? "/**/*.Tests.dll";
        TestProjectPattern = testProjectPattern ?? @".*\.Tests\.csproj";
        IntegrationTestFilePattern = integrationTestFilePattern ?? "/**/*.IntegrationTests.dll";
        IntegrationTestProjectPattern = integrationTestProjectPattern ?? @".*\.IntegrationTests\.csproj";
    }
}