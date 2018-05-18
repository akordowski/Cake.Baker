public class Environment
{
    public string AppVeyorApiTokenVariable { get; }
    public string GitHubUsernameVariable { get; }
    public string GitHubPasswordVariable { get; }
    public string MyGetApiKeyVariable { get; }
    public string MyGetSourceVariable { get; }
    public string NuGetApiKeyVariable { get; }
    public string NuGetSourceVariable { get; }
    public string TwitterAccessTokenVariable { get; }
    public string TwitterAccessTokenSecretVariable { get; }
    public string TwitterConsumerKeyVariable { get; }
    public string TwitterConsumerSecretVariable { get; }

    public Environment(
        string appVeyorApiTokenVariable,
        string githubUsernameVariable,
        string githubPasswordVariable,
        string mygetApiKeyVariable,
        string mygetSourceVariable,
        string nugetApiKeyVariable,
        string nugetSourceVariable,
        string twitterAccessTokenVariable,
        string twitterAccessTokenSecretVariable,
        string twitterConsumerKeyVariable,
        string twitterConsumerSecretVariable)
    {
        AppVeyorApiTokenVariable = appVeyorApiTokenVariable.DefaultValue("APPVEYOR_API_TOKEN");
        GitHubUsernameVariable = githubUsernameVariable.DefaultValue("GITHUB_USERNAME");
        GitHubPasswordVariable = githubPasswordVariable.DefaultValue("GITHUB_PASSWORD");
        MyGetApiKeyVariable = mygetApiKeyVariable.DefaultValue("MYGET_API_KEY");
        MyGetSourceVariable = mygetSourceVariable.DefaultValue("MYGET_SOURCE");
        NuGetApiKeyVariable = nugetApiKeyVariable.DefaultValue("NUGET_API_KEY");
        NuGetSourceVariable = nugetSourceVariable.DefaultValue("NUGET_SOURCE");
        TwitterConsumerKeyVariable = twitterConsumerKeyVariable.DefaultValue("TWITTER_CONSUMER_KEY");
        TwitterConsumerSecretVariable = twitterConsumerSecretVariable.DefaultValue("TWITTER_CONSUMER_SECRET");
        TwitterAccessTokenVariable = twitterAccessTokenVariable.DefaultValue("TWITTER_ACCESS_TOKEN");
        TwitterAccessTokenSecretVariable = twitterAccessTokenSecretVariable.DefaultValue("TWITTER_ACCESS_TOKEN_SECRET");
    }
}