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

    public bool CanPostToTwitter =>
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.ConsumerKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.ConsumerSecret) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.AccessToken) &&
        !String.IsNullOrEmpty(_builder.Credentials.Twitter.AccessTokenSecret);

    public bool CanPublishToGitHub =>
        !String.IsNullOrEmpty(_builder.Credentials.GitHub.Username) &&
        !String.IsNullOrEmpty(_builder.Credentials.GitHub.Password);

    public bool CanPublishToMyGet =>
        !String.IsNullOrEmpty(_builder.Credentials.MyGet.ApiKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.MyGet.Source);

    public bool CanPublishToNuGet =>
        !String.IsNullOrEmpty(_builder.Credentials.NuGet.ApiKey) &&
        !String.IsNullOrEmpty(_builder.Credentials.NuGet.Source);

    public bool ShouldPostToTwitter { get; }
    public bool ShouldPublishToGitHub { get; }
    public bool ShouldPublishToMyGet { get; }
    public bool ShouldPublishToNuGet { get; }
    public bool ShouldRunTests { get; }
    public bool ShouldRunIntegrationTests { get; }
    public bool ShouldRunGitVersion { get; }
    public bool ShouldRunOpenCover { get; }
    public bool ShouldRunReportGenerator { get; }
    public bool ShouldRunReportUnit { get; }

    public bool PrintAllInfo { get; }
    public bool PrintVersionInfo { get; }
    public bool PrintParametersInfo { get; }
    public bool PrintDirectoriesInfo { get; }
    public bool PrintFilesInfo { get; }
    public bool PrintEnvironmentInfo { get; }
    public bool PrintToolSettingsInfo { get; }

    public string TestFilePattern { get; }
    public string TestProjectPattern { get; }
    public string IntegrationTestFilePattern { get; }
    public string IntegrationTestProjectPattern { get; }
    public string PublishMessage => String.Format(_publishMessage, _builder.Version.Version, Title);

    private readonly Builder _builder;
    private readonly ICakeContext _context;
    private readonly BuildSystem _buildSystem;

    private string _publishMessage;

    public Parameters(
        Builder builder,
        string title,
        string repositoryOwner,
        string repositoryName,
        string repositoryBranch,
        bool isPrerelease,
        bool isPublicRepository,
        bool? shouldPostToTwitter,
        bool? shouldPublishToGitHub,
        bool? shouldPublishToMyGet,
        bool? shouldPublishToNuGet,
        bool shouldRunTests,
        bool shouldRunIntegrationTests,
        bool? shouldRunGitVersion,
        bool? shouldRunOpenCover,
        bool? shouldRunReportGenerator,
        bool? shouldRunReportUnit,
        bool? printAllInfo,
        bool? printVersionInfo,
        bool? printParametersInfo,
        bool? printDirectoriesInfo,
        bool? printFilesInfo,
        bool? printEnvironmentInfo,
        bool? printToolSettingsInfo,
        string testFilePattern,
        string testProjectPattern,
        string integrationTestFilePattern,
        string integrationTestProjectPattern,
        string publishMessage)
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
        RepositoryBranch = repositoryBranch.DefaultValue("master");
        IsPrerelease = isPrerelease;

        IsMainRepository = repo.Name.Equals($"{repositoryOwner}/{repositoryName}", StringComparison.OrdinalIgnoreCase);
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

        ShouldPostToTwitter = shouldPostToTwitter ?? false;

        ShouldPublishToMyGet = shouldPublishToMyGet ??
                               !IsLocalBuild &&
                               !IsPullRequest &&
                               IsMainRepository &&
                               IsTagged;

        ShouldPublishToNuGet = shouldPublishToNuGet ??
                               !IsLocalBuild &&
                               !IsPullRequest &&
                               IsMainRepository &&
                               IsTagged;

        ShouldPublishToGitHub = shouldPublishToGitHub ??
                                !IsLocalBuild &&
                                !IsPullRequest &&
                                IsMainRepository &&
                                IsTagged;

        ShouldRunTests = shouldRunTests;
        ShouldRunIntegrationTests = shouldRunIntegrationTests;

        ShouldRunGitVersion = shouldRunGitVersion ?? _context.IsRunningOnWindows();
        ShouldRunOpenCover = shouldRunOpenCover ?? _context.IsRunningOnWindows();
        ShouldRunReportGenerator = shouldRunReportGenerator ?? _context.IsRunningOnWindows();
        ShouldRunReportUnit = shouldRunReportUnit ?? _context.IsRunningOnWindows();

        PrintAllInfo = printAllInfo ?? false;
        PrintVersionInfo = printVersionInfo ?? (printAllInfo ?? true);
        PrintParametersInfo = printParametersInfo ?? (printAllInfo ?? true);
        PrintDirectoriesInfo = printDirectoriesInfo ?? (printAllInfo ?? false);
        PrintFilesInfo = printFilesInfo ?? (printAllInfo ?? false);
        PrintEnvironmentInfo = printEnvironmentInfo ?? (printAllInfo ?? false);
        PrintToolSettingsInfo = printToolSettingsInfo ?? (printAllInfo ?? false);

        TestFilePattern = testFilePattern ?? "/**/*.Tests.dll";
        TestProjectPattern = testProjectPattern ?? @".*\.Tests\.csproj";
        IntegrationTestFilePattern = integrationTestFilePattern ?? "/**/*.IntegrationTests.dll";
        IntegrationTestProjectPattern = integrationTestProjectPattern ?? @".*\.IntegrationTests\.csproj";

        _publishMessage = publishMessage.DefaultValue("Version {0} of {1} Addin has just been released, https://www.nuget.org/packages/{1}.");
    }
}