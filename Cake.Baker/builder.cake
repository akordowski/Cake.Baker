public class Builder
{
    public ICakeContext Context { get; }
    public BuildSystem BuildSystem { get; }

    public Credentials Credentials { get; private set; }
    public Environment Environment { get; private set; }
    public Messages Messages { get; private set; }
    public Parameters Parameters { get; private set; }
    public Projects Projects { get; private set; }
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
        SetMessages();
        SetPaths();
        SetToolSettings();
    }

    public void Run()
    {
        Projects = new Projects(this);

        _runTarget(Parameters.Target);
    }

    /* ---------------------------------------------------------------------------------------------------- */
    /* Methods */

    public Builder SetEnvironmentVariableNames(
        string appVeyorApiTokenVariable = null,
        string chocolateyApiKeyVariable = null,
        string chocolateySourceUrlVariable = null,
        string coverallsRepoTokenVariable = null,
        string codecovRepoTokenVariable = null,
        string gitHubUsernameVariable = null,
        string gitHubPasswordVariable = null,
        string gitterTokenVariable = null,
        string gitterRoomIdVariable = null,
        string microsoftTeamsWebHookUrlVariable = null,
        string myGetApiKeyVariable = null,
        string myGetSourceVariable = null,
        string nuGetApiKeyVariable = null,
        string nuGetSourceVariable = null,
        string slackTokenVariable = null,
        string slackChannelVariable = null,
        string twitterAccessTokenVariable = null,
        string twitterAccessTokenSecretVariable = null,
        string twitterConsumerKeyVariable = null,
        string twitterConsumerSecretVariable = null,
        string transifexApiTokenVariable = null,
        string wyamAccessTokenVariable = null,
        string wyamDeployRemoteVariable = null,
        string wyamDeployBranchVariable = null)
    {
        Environment = new Environment(
            appVeyorApiTokenVariable,
            chocolateyApiKeyVariable,
            chocolateySourceUrlVariable,
            coverallsRepoTokenVariable,
            codecovRepoTokenVariable,
            gitHubUsernameVariable,
            gitHubPasswordVariable,
            gitterTokenVariable,
            gitterRoomIdVariable,
            microsoftTeamsWebHookUrlVariable,
            myGetApiKeyVariable,
            myGetSourceVariable,
            nuGetApiKeyVariable,
            nuGetSourceVariable,
            slackTokenVariable,
            slackChannelVariable,
            twitterAccessTokenVariable,
            twitterAccessTokenSecretVariable,
            twitterConsumerKeyVariable,
            twitterConsumerSecretVariable,
            transifexApiTokenVariable,
            wyamAccessTokenVariable,
            wyamDeployRemoteVariable,
            wyamDeployBranchVariable);

        Credentials = new Credentials(Context, Environment);

        return this;
    }

    public Builder SetMessages(
        string defaultMessage = null,
        string gitterMessage = null,
        string microsoftTeamsMessage = null,
        string slackMessage = null,
        string twitterMessage = null)
    {
        Messages = new Messages(
            this,
            defaultMessage,
            gitterMessage,
            microsoftTeamsMessage,
            slackMessage,
            twitterMessage);

        return this;
    }

    public Builder SetParameters(
        string title,
        string repositoryOwner,
        string repositoryName = null,
        string repositoryBranch = null,
        bool isPrerelease = false,
        bool isPublicRepository = true,
        bool? shouldRunGitVersion = null,
        bool shouldRunDocFx = false,
        bool shouldRunTests = true,
        bool shouldRunIntegrationTests = false,
        bool shouldRunNUnit3Tests = true,
        bool shouldRunXUnitTests = true,
        bool shouldRunMSTestTests = true,
        bool shouldRunFixieTests = true,
        bool shouldRunDotNetCoreTest = true,
        bool shouldRunOpenCover = false,
        bool shouldRunReportGenerator = false,
        bool shouldRunReportUnit = false,
        bool? shouldPackage = null,
        bool? shouldPackageNuGet = null,
        bool? shouldPublish = null,
        bool? shouldPublishToChocolatey = null,
        bool? shouldPublishToCodecov = null,
        bool? shouldPublishToCoveralls = null,
        bool? shouldPublishToMyGet = null,
        bool? shouldPublishToNuGet = null,
        bool? shouldPublishToGitHub = null,
        bool? shouldPost = null,
        bool? shouldPostToGitter = null,
        bool? shouldPostToMicrosoftTeams = null,
        bool? shouldPostToSlack = null,
        bool? shouldPostToTwitter = null,
        bool shouldAppVeyorPrintEnvironmentVariables = false,
        bool shouldAppVeyorUploadArtifacts = false,
        bool shouldAppVeyorUploadTestResults = false,
        bool? printAllInfo = null,
        bool? printVersionInfo = null,
        bool? printParametersInfo = null,
        bool? printMessagesInfo = null,
        bool? printDirectoriesInfo = null,
        bool? printFilesInfo = null,
        bool? printEnvironmentInfo = null,
        bool? printToolSettingsInfo = null,
        string testFilePattern = null,
        string testProjectPattern = null,
        string integrationTestFilePattern = null,
        string integrationTestProjectPattern = null)
    {
        Parameters = new Parameters(
            this,
            title,
            repositoryOwner,
            repositoryName,
            repositoryBranch,
            isPrerelease,
            isPublicRepository,
            shouldRunGitVersion,
            shouldRunDocFx,
            shouldRunTests,
            shouldRunIntegrationTests,
            shouldRunNUnit3Tests,
            shouldRunXUnitTests,
            shouldRunMSTestTests,
            shouldRunFixieTests,
            shouldRunDotNetCoreTest,
            shouldRunOpenCover,
            shouldRunReportGenerator,
            shouldRunReportUnit,
            shouldPackage,
            shouldPackageNuGet,
            shouldPublish,
            shouldPublishToChocolatey,
            shouldPublishToCodecov,
            shouldPublishToCoveralls,
            shouldPublishToMyGet,
            shouldPublishToNuGet,
            shouldPublishToGitHub,
            shouldPost,
            shouldPostToGitter,
            shouldPostToMicrosoftTeams,
            shouldPostToSlack,
            shouldPostToTwitter,
            shouldAppVeyorPrintEnvironmentVariables,
            shouldAppVeyorUploadArtifacts,
            shouldAppVeyorUploadTestResults,
            printAllInfo,
            printVersionInfo,
            printParametersInfo,
            printMessagesInfo,
            printDirectoriesInfo,
            printFilesInfo,
            printEnvironmentInfo,
            printToolSettingsInfo,
            testFilePattern,
            testProjectPattern,
            integrationTestFilePattern,
            integrationTestProjectPattern);

        return this;
    }

    public Builder SetPaths(
        DirectoryPath rootDirectoryPath = null,
        DirectoryPath nuspecDirectoryPath = null,
        DirectoryPath sourceDirectoryPath = null,
        DirectoryPath artifactsDirectoryPath = null,
        DirectoryPath docsDirectoryPath = null,
        DirectoryPath imageDirectoryPath = null,
        DirectoryPath logsDirectoryPath = null,
        DirectoryPath packagesDirectoryPath = null,
        DirectoryPath packagesNuGetDirectoryPath = null,
        DirectoryPath packagesZipDirectoryPath = null,
        DirectoryPath publishedDirectoryPath = null,
        DirectoryPath publishedApplicationsDirectoryPath = null,
        DirectoryPath publishedLibrariesDirectoryPath = null,
        DirectoryPath publishedWebApplicationsDirectoryPath = null,
        DirectoryPath publishedFixieTestsDirectoryPath = null,
        DirectoryPath publishedMSTestTestsDirectoryPath = null,
        DirectoryPath publishedNUnitTestsDirectoryPath = null,
        DirectoryPath publishedNUnit3TestsDirectoryPath = null,
        DirectoryPath publishedXUnitTestsDirectoryPath = null,
        DirectoryPath testsDirectoryPath = null,
        DirectoryPath testCoverageDirectoryPath = null,
        DirectoryPath testReportsDirectoryPath = null,
        DirectoryPath testReportGeneratorDirectoryPath = null,
        DirectoryPath testReportUnitDirectoryPath = null,
        DirectoryPath testResultsDirectoryPath = null,
        FilePath licenseFilePath = null,
        FilePath releaseNotesFilePath = null,
        FilePath solutionFilePath = null,
        FilePath solutionInfoFilePath = null,
        FilePath gitReleaseNotesFilePath = null,
        FilePath buildLogFilePath = null,
        FilePath nunit3OutputFileFilePath = null,
        FilePath openCoverFilePath = null,
        FilePath fixieTestResultsFilePath = null,
        FilePath msTestTestResultsFilePath = null,
        FilePath nunitTestResultsFilePath = null,
        FilePath nunit3TestResultsFilePath = null,
        FilePath xunitTestResultsFilePath = null)
    {
        var directories = new Directories(
            Context,
            rootDirectoryPath,
            nuspecDirectoryPath,
            sourceDirectoryPath,
            artifactsDirectoryPath,
            docsDirectoryPath,
            imageDirectoryPath,
            logsDirectoryPath,
            packagesDirectoryPath,
            packagesNuGetDirectoryPath,
            packagesZipDirectoryPath,
            publishedDirectoryPath,
            publishedApplicationsDirectoryPath,
            publishedLibrariesDirectoryPath,
            publishedWebApplicationsDirectoryPath,
            publishedFixieTestsDirectoryPath,
            publishedMSTestTestsDirectoryPath,
            publishedNUnitTestsDirectoryPath,
            publishedNUnit3TestsDirectoryPath,
            publishedXUnitTestsDirectoryPath,
            testsDirectoryPath,
            testCoverageDirectoryPath,
            testReportsDirectoryPath,
            testReportGeneratorDirectoryPath,
            testReportUnitDirectoryPath,
            testResultsDirectoryPath);

        var files = new Files(
            Context,
            directories,
            licenseFilePath,
            releaseNotesFilePath,
            solutionFilePath,
            solutionInfoFilePath,
            gitReleaseNotesFilePath,
            buildLogFilePath,
            nunit3OutputFileFilePath,
            openCoverFilePath,
            fixieTestResultsFilePath,
            msTestTestResultsFilePath,
            nunitTestResultsFilePath,
            nunit3TestResultsFilePath,
            xunitTestResultsFilePath);

        Paths = new Paths(directories, files);

        return this;
    }

    public Builder SetToolSettings(
        PlatformTarget buildPlatformTarget = PlatformTarget.MSIL,
        MSBuildToolVersion buildMSBuildToolVersion = MSBuildToolVersion.Default,
        int buildMaxCpuCount = 0,
        bool buildTreatWarningsAsErrors = true,
        bool nuGetSymbolPackage = false,
        bool nuGetNoPackageAnalysis = true,
        string testCoverageFilter = null,
        string testCoverageExcludeByAttribute = null,
        string testCoverageExcludeByFile = null)
    {
        ToolSettings = new ToolSettings(
            this,
            buildPlatformTarget,
            buildMSBuildToolVersion,
            buildMaxCpuCount,
            buildTreatWarningsAsErrors,
            nuGetSymbolPackage,
            nuGetNoPackageAnalysis,
            testCoverageFilter,
            testCoverageExcludeByAttribute,
            testCoverageExcludeByFile);

        return this;
    }
}