public class Credentials
{
    public AppVeyorCredentials AppVeyor { get; }
    public ChocolateyCredentials Chocolatey { get; }
    public CodecovCredentials Codecov { get; }
    public CoverallsCredentials Coveralls { get; }
    public GitHubCredentials GitHub { get; }
    public GitterCredentials Gitter { get; }
    public MicrosoftTeamsCredentials MicrosoftTeams { get; }
    public MyGetCredentials MyGet { get; }
    public NuGetCredentials NuGet { get; }
    public SlackCredentials Slack { get; }
    public TwitterCredentials Twitter { get; }
    public TransifexCredentials Transifex { get; }
    public WyamCredentials Wyam { get; }

    public Credentials(ICakeContext context, Environment environment)
    {
        AppVeyor = new AppVeyorCredentials(
            context.EnvironmentVariable(environment.AppVeyorApiTokenVariable));

        Chocolatey = new ChocolateyCredentials(
            context.EnvironmentVariable(environment.ChocolateyApiKeyVariable),
            context.EnvironmentVariable(environment.ChocolateySourceUrlVariable));

        Codecov = new CodecovCredentials(
            context.EnvironmentVariable(environment.CodecovRepoTokenVariable));

        Coveralls = new CoverallsCredentials(
            context.EnvironmentVariable(environment.CoverallsRepoTokenVariable));

        GitHub = new GitHubCredentials(
            context.EnvironmentVariable(environment.GitHubUsernameVariable),
            context.EnvironmentVariable(environment.GitHubPasswordVariable));

        Gitter = new GitterCredentials(
            context.EnvironmentVariable(environment.GitterTokenVariable),
            context.EnvironmentVariable(environment.GitterRoomIdVariable));

        MicrosoftTeams = new MicrosoftTeamsCredentials(
            context.EnvironmentVariable(environment.MicrosoftTeamsWebHookUrlVariable));

        MyGet = new MyGetCredentials(
            context.EnvironmentVariable(environment.MyGetApiKeyVariable),
            context.EnvironmentVariable(environment.MyGetSourceVariable));

        NuGet = new NuGetCredentials(
            context.EnvironmentVariable(environment.NuGetApiKeyVariable),
            context.EnvironmentVariable(environment.NuGetSourceVariable));

        Slack = new SlackCredentials(
            context.EnvironmentVariable(environment.SlackTokenVariable),
            context.EnvironmentVariable(environment.SlackChannelVariable));

        Twitter = new TwitterCredentials(
            context.EnvironmentVariable(environment.TwitterConsumerKeyVariable),
            context.EnvironmentVariable(environment.TwitterConsumerSecretVariable),
            context.EnvironmentVariable(environment.TwitterAccessTokenVariable),
            context.EnvironmentVariable(environment.TwitterAccessTokenSecretVariable));

        Transifex = new TransifexCredentials(
            context.EnvironmentVariable(environment.TransifexApiTokenVariable));

        Wyam = new WyamCredentials(
            context.EnvironmentVariable(environment.WyamAccessTokenVariable),
            context.EnvironmentVariable(environment.WyamDeployRemoteVariable),
            context.EnvironmentVariable(environment.WyamDeployBranchVariable));
    }
}

/* ---------------------------------------------------------------------------------------------------- */
/* Classes */

public class AppVeyorCredentials
{
    public string ApiToken { get; }

    public AppVeyorCredentials(string apiToken)
    {
        ApiToken = apiToken;
    }
}

public class ChocolateyCredentials
{
    public string ApiKey { get; }
    public string Source { get; }

    public ChocolateyCredentials(string apiKey, string source)
    {
        ApiKey = apiKey;
        Source = source;
    }
}

public class CodecovCredentials
{
    public string RepoToken { get; }

    public CodecovCredentials(string repoToken)
    {
        RepoToken = repoToken;
    }
}

public class CoverallsCredentials
{
    public string RepoToken { get; }

    public CoverallsCredentials(string repoToken)
    {
        RepoToken = repoToken;
    }
}

public class GitHubCredentials
{
    public string Username { get; }
    public string Password { get; }

    public GitHubCredentials(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

public class GitterCredentials
{
    public string Token { get; }
    public string RoomId { get; }

    public GitterCredentials(string token, string roomId)
    {
        Token = token;
        RoomId = roomId;
    }
}

public class MicrosoftTeamsCredentials
{
    public string WebHookUrl { get; }

    public MicrosoftTeamsCredentials(string webHookUrl)
    {
        WebHookUrl = webHookUrl;
    }
}

public class MyGetCredentials : NuGetCredentials
{
    public MyGetCredentials(string apiKey, string source)
        : base(apiKey, source)
    {
    }
}

public class NuGetCredentials
{
    public string ApiKey { get; }
    public string Source { get; }

    public NuGetCredentials(string apiKey, string source)
    {
        ApiKey = apiKey;
        Source = source;
    }
}

public class SlackCredentials
{
    public string Token { get; }
    public string Channel { get; }

    public SlackCredentials(string token, string channel)
    {
        Token = token;
        Channel = channel;
    }
}

public class TransifexCredentials
{
    public string ApiToken { get; }

    public TransifexCredentials(string apiToken)
    {
        ApiToken = apiToken;
    }
}

public class TwitterCredentials
{
    public string ConsumerKey { get; }
    public string ConsumerSecret { get; }
    public string AccessToken { get; }
    public string AccessTokenSecret { get; }

    public TwitterCredentials(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
    {
        ConsumerKey = consumerKey;
        ConsumerSecret = consumerSecret;
        AccessToken = accessToken;
        AccessTokenSecret = accessTokenSecret;
    }
}

public class WyamCredentials
{
    public string AccessToken { get; }
    public string DeployRemote { get; }
    public string DeployBranch { get; }

    public WyamCredentials(string accessToken, string deployRemote, string deployBranch)
    {
        AccessToken = accessToken;
        DeployRemote = deployRemote;
        DeployBranch = deployBranch;
    }
}