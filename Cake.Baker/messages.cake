public class Messages
{
    public static string DefaultMessage = "Version {0} of {1} has just been released, https://www.nuget.org/packages/{1}/.";

    public string Message => FormatMessage(_message);
    public string GitterMessage => FormatMessage(_gitterMessage);
    public string MicrosoftTeamsMessage => FormatMessage(_microsoftTeamsMessage);
    public string SlackMessage => FormatMessage(_slackMessage);
    public string TwitterMessage => FormatMessage(_twitterMessage);

    private readonly Builder _builder;

    private readonly string _message;
    private readonly string _gitterMessage;
    private readonly string _microsoftTeamsMessage;
    private readonly string _slackMessage;
    private readonly string _twitterMessage;

    public Messages(
        Builder builder,
        string message,
        string gitterMessage,
        string microsoftTeamsMessage,
        string slackMessage,
        string twitterMessage)
    {
        _builder = builder;

        _message = message ?? DefaultMessage;
        _gitterMessage = gitterMessage ?? "@/all " + _message;
        _microsoftTeamsMessage = microsoftTeamsMessage ?? "@/all " + _message;
        _slackMessage = slackMessage ?? "@/all " + _message;
        _twitterMessage = twitterMessage ?? _message;
    }

    private string FormatMessage(string message)
    {
        return String.Format(message, _builder.Version.Version, _builder.Parameters.Title);
    }
}