public class Credentials
{
    public GitHubCredentials GitHub { get; }
    public MyGetCredentials MyGet { get; }
    public NuGetCredentials NuGet { get; }
    public TwitterCredentials Twitter { get; }

    public Credentials(ICakeContext context, Environment environment)
    {
        GitHub = new GitHubCredentials(
            context.EnvironmentVariable(environment.GitHubUsernameVariable),
            context.EnvironmentVariable(environment.GitHubPasswordVariable));

        MyGet = new MyGetCredentials(
            context.EnvironmentVariable(environment.MyGetApiKeyVariable),
            context.EnvironmentVariable(environment.MyGetSourceVariable));

        NuGet = new NuGetCredentials(
            context.EnvironmentVariable(environment.NuGetApiKeyVariable),
            context.EnvironmentVariable(environment.NuGetSourceVariable));

        Twitter = new TwitterCredentials(
            context.EnvironmentVariable(environment.TwitterConsumerKeyVariable),
            context.EnvironmentVariable(environment.TwitterConsumerSecretVariable),
            context.EnvironmentVariable(environment.TwitterAccessTokenVariable),
            context.EnvironmentVariable(environment.TwitterAccessTokenSecretVariable));
    }
}

/* ---------------------------------------------------------------------------------------------------- */
/* Classes */

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