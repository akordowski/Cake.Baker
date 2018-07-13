public class Environment
{
    public string AppVeyorApiTokenVariable { get; }
    public string ChocolateyApiKeyVariable { get; }
    public string ChocolateySourceUrlVariable { get; }
    public string CoverallsRepoTokenVariable { get; }
    public string CodecovRepoTokenVariable { get; }
    public string GitHubUsernameVariable { get; }
    public string GitHubPasswordVariable { get; }
    public string GitterTokenVariable { get; }
    public string GitterRoomIdVariable { get; }
    public string MicrosoftTeamsWebHookUrlVariable { get; }
    public string MyGetApiKeyVariable { get; }
    public string MyGetSourceVariable { get; }
    public string NuGetApiKeyVariable { get; }
    public string NuGetSourceVariable { get; }
    public string SlackTokenVariable { get; }
    public string SlackChannelVariable { get; }
    public string TwitterAccessTokenVariable { get; }
    public string TwitterAccessTokenSecretVariable { get; }
    public string TwitterConsumerKeyVariable { get; }
    public string TwitterConsumerSecretVariable { get; }
    public string TransifexApiTokenVariable { get; }
    public string WyamAccessTokenVariable { get; }
    public string WyamDeployRemoteVariable { get; }
    public string WyamDeployBranchVariable { get; }

    public Environment(
        string appVeyorApiTokenVariable,
        string chocolateyApiKeyVariable,
        string chocolateySourceUrlVariable,
        string coverallsRepoTokenVariable,
        string codecovRepoTokenVariable,
        string githubUsernameVariable,
        string githubPasswordVariable,
        string gitterTokenVariable,
        string gitterRoomIdVariable,
        string microsoftTeamsWebHookUrlVariable,
        string mygetApiKeyVariable,
        string mygetSourceVariable,
        string nugetApiKeyVariable,
        string nugetSourceVariable,
        string slackTokenVariable,
        string slackChannelVariable,
        string twitterAccessTokenVariable,
        string twitterAccessTokenSecretVariable,
        string twitterConsumerKeyVariable,
        string twitterConsumerSecretVariable,
        string transifexApiTokenVariable,
        string wyamAccessTokenVariable,
        string wyamDeployRemoteVariable,
        string wyamDeployBranchVariable)
    {
        AppVeyorApiTokenVariable = appVeyorApiTokenVariable.DefaultValue("APPVEYOR_API_TOKEN");

        ChocolateyApiKeyVariable = chocolateyApiKeyVariable.DefaultValue("CHOCOLATEY_API_KEY");
        ChocolateySourceUrlVariable = chocolateySourceUrlVariable.DefaultValue("CHOCOLATEY_SOURCE_URL");
        CoverallsRepoTokenVariable = coverallsRepoTokenVariable.DefaultValue("COVERALLS_REPO_TOKEN");
        CodecovRepoTokenVariable = codecovRepoTokenVariable.DefaultValue("CODECOV_REPO_TOKEN");

        GitHubUsernameVariable = githubUsernameVariable.DefaultValue("GITHUB_USERNAME");
        GitHubPasswordVariable = githubPasswordVariable.DefaultValue("GITHUB_PASSWORD");

        GitterTokenVariable = gitterTokenVariable.DefaultValue("GITTER_TOKEN");
        GitterRoomIdVariable = gitterRoomIdVariable.DefaultValue("GITTER_ROOM_ID");
        MicrosoftTeamsWebHookUrlVariable = microsoftTeamsWebHookUrlVariable.DefaultValue("MICROSOFT_TEAMS_WEB_HOOK_URL");

        MyGetApiKeyVariable = mygetApiKeyVariable.DefaultValue("MYGET_API_KEY");
        MyGetSourceVariable = mygetSourceVariable.DefaultValue("MYGET_SOURCE");
        NuGetApiKeyVariable = nugetApiKeyVariable.DefaultValue("NUGET_API_KEY");
        NuGetSourceVariable = nugetSourceVariable.DefaultValue("NUGET_SOURCE");

        SlackTokenVariable = slackTokenVariable.DefaultValue("SLACK_TOKEN");
        SlackChannelVariable = slackChannelVariable.DefaultValue("SLACK_CHANNEL");

        TwitterConsumerKeyVariable = twitterConsumerKeyVariable.DefaultValue("TWITTER_CONSUMER_KEY");
        TwitterConsumerSecretVariable = twitterConsumerSecretVariable.DefaultValue("TWITTER_CONSUMER_SECRET");
        TwitterAccessTokenVariable = twitterAccessTokenVariable.DefaultValue("TWITTER_ACCESS_TOKEN");
        TwitterAccessTokenSecretVariable = twitterAccessTokenSecretVariable.DefaultValue("TWITTER_ACCESS_TOKEN_SECRET");

        TransifexApiTokenVariable = transifexApiTokenVariable.DefaultValue("TRANSIFEX_API_TOKEN");
        WyamAccessTokenVariable = wyamAccessTokenVariable.DefaultValue("WYAM_ACCESS_TOKEN");
        WyamDeployRemoteVariable = wyamDeployRemoteVariable.DefaultValue("WYAM_DEPLOY_REMOTE");
        WyamDeployBranchVariable = wyamDeployBranchVariable.DefaultValue("WYAM_DEPLOY_BRANCH");
    }
}