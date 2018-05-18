public class Builder
{
    public ICakeContext Context { get; }
    public BuildSystem BuildSystem { get; }

    public Credentials Credentials { get; private set; }
    public Environment Environment { get; private set; }
    public Parameters Parameters { get; private set; }
    public Paths Paths { get; private set; }
    public ToolSettings ToolSettings { get; private set; }
    public VersionInfo Version { get; private set; }

    private readonly Action<string> _runTarget;

    public Builder(ICakeContext context, BuildSystem buildSystem, Action<string> runTarget)
    {
        Context = context;
        BuildSystem = buildSystem;
        _runTarget = runTarget;

        Version = new VersionInfo(this);

        SetEnvironmentVariableNames();
        SetPaths();
        SetToolSettings();
    }

    public void Run()
    {
        _runTarget(Parameters.Target);
    }

    /* ---------------------------------------------------------------------------------------------------- */
    /* Methods */

    public Builder SetEnvironmentVariableNames(
        string appVeyorApiTokenVariable = null,
        string gitHubUsernameVariable = null,
        string gitHubPasswordVariable = null,
        string myGetApiKeyVariable = null,
        string myGetSourceVariable = null,
        string nuGetApiKeyVariable = null,
        string nuGetSourceVariable = null,
        string twitterAccessTokenVariable = null,
        string twitterAccessTokenSecretVariable = null,
        string twitterConsumerKeyVariable = null,
        string twitterConsumerSecretVariable = null)
    {
        Environment = new Environment(
            appVeyorApiTokenVariable,
            gitHubUsernameVariable,
            gitHubPasswordVariable,
            myGetApiKeyVariable,
            myGetSourceVariable,
            nuGetApiKeyVariable,
            nuGetSourceVariable,
            twitterAccessTokenVariable,
            twitterAccessTokenSecretVariable,
            twitterConsumerKeyVariable,
            twitterConsumerSecretVariable);

        Credentials = new Credentials(Context, Environment);

        return this;
    }

    public Builder SetParameters(
        string title,
        string repositoryOwner,
        string repositoryName = null,
        string repositoryBranch = null,
        bool isPrerelease = false,
        bool isPublicRepository = true,
        bool shouldBuildNuGetSymbolPackage = false,
        bool shouldRunNuGetPackageAnalysis = false,
        bool? shouldCreateCoverageReport = null,
        bool? shouldPostToTwitter = null,
        bool? shouldPublishToGitHub = null,
        bool? shouldPublishToMyGet = null,
        bool? shouldPublishToNuGet = null,
        bool? shouldRunGitVersion = null,
        bool? printAllInfo = null,
        bool? printVersionInfo = null,
        bool? printParametersInfo = null,
        bool? printDirectoriesInfo = null,
        bool? printFilesInfo = null,
        bool? printEnvironmentInfo = null,
        bool? printToolSettingsInfo = null,
        string publishMessage = null)
    {
        Parameters = new Parameters(
            this,
            title,
            repositoryOwner,
            repositoryName,
            repositoryBranch,
            isPrerelease,
            isPublicRepository,
            shouldBuildNuGetSymbolPackage,
            shouldRunNuGetPackageAnalysis,
            shouldCreateCoverageReport,
            shouldPostToTwitter,
            shouldPublishToGitHub,
            shouldPublishToMyGet,
            shouldPublishToNuGet,
            shouldRunGitVersion,
            printAllInfo,
            printVersionInfo,
            printParametersInfo,
            printDirectoriesInfo,
            printFilesInfo,
            printEnvironmentInfo,
            printToolSettingsInfo,
            publishMessage);

        return this;
    }

    public Builder SetPaths(
        DirectoryPath rootDirectoryPath = null,
        DirectoryPath nuspecDirectoryPath = null,
        DirectoryPath sourceDirectoryPath = null,
        DirectoryPath artifactsDirectoryPath = null,
        DirectoryPath docsDirectoryPath = null,
        DirectoryPath logDirectoryPath = null,
        DirectoryPath packagesDirectoryPath = null,
        DirectoryPath packagesNuGetDirectoryPath = null,
        DirectoryPath packagesZipDirectoryPath = null,
        DirectoryPath tmpDirectoryPath = null,
        DirectoryPath publishedApplicationsDirectoryPath = null,
        DirectoryPath publishedLibrariesDirectoryPath = null,
        DirectoryPath publishedWebsitesDirectoryPath = null,
        DirectoryPath publishedNUnitTestsDirectoryPath = null,
        DirectoryPath testsDirectoryPath = null,
        DirectoryPath testCoverageDirectoryPath = null,
        DirectoryPath testReportDirectoryPath = null,
        DirectoryPath testResultsDirectoryPath = null,
        DirectoryPath testResultsNUnitDirectoryPath = null,
        FilePath licenseFilePath = null,
        FilePath releaseNotesFilePath = null,
        FilePath gitReleaseNotesFilePath = null,
        FilePath buildLogFilePath = null,
        FilePath testCoverageOutputFilePath = null,
        FilePath solutionFilePath = null,
        FilePath solutionInfoFilePath = null)
    {
        var directories = new Directories(
            Context,
            rootDirectoryPath,
            nuspecDirectoryPath,
            sourceDirectoryPath,
            artifactsDirectoryPath,
            docsDirectoryPath,
            logDirectoryPath,
            packagesDirectoryPath,
            packagesNuGetDirectoryPath,
            packagesZipDirectoryPath,
            tmpDirectoryPath,
            publishedApplicationsDirectoryPath,
            publishedLibrariesDirectoryPath,
            publishedWebsitesDirectoryPath,
            publishedNUnitTestsDirectoryPath,
            testsDirectoryPath,
            testCoverageDirectoryPath,
            testReportDirectoryPath,
            testResultsDirectoryPath,
            testResultsNUnitDirectoryPath);

        var files = new Files(
            Context,
            directories,
            licenseFilePath,
            releaseNotesFilePath,
            gitReleaseNotesFilePath,
            buildLogFilePath,
            testCoverageOutputFilePath,
            solutionFilePath,
            solutionInfoFilePath);

        Paths = new Paths(directories, files);

        return this;
    }

    public Builder SetToolSettings(
        string testCoverageFilter = null,
        string testCoverageExcludeByAttribute = null,
        string testCoverageExcludeByFile = null)
    {
        ToolSettings = new ToolSettings(
            this,
            testCoverageFilter,
            testCoverageExcludeByAttribute,
            testCoverageExcludeByFile);

        return this;
    }
}