public class Messages
{
    public string DefaultMessage => FormatMessage(_defaultMessage);
    public string GitterMessage => FormatMessage(_gitterMessage);
    public string MicrosoftTeamsMessage => FormatMessage(_microsoftTeamsMessage);
    public string SlackMessage => FormatMessage(_slackMessage);
    public string TwitterMessage => FormatMessage(_twitterMessage);

    private readonly Builder _builder;

    private readonly string _defaultMessage;
    private readonly string _gitterMessage;
    private readonly string _microsoftTeamsMessage;
    private readonly string _slackMessage;
    private readonly string _twitterMessage;

    public Messages(
        Builder builder,
        string defaultMessage,
        string gitterMessage,
        string microsoftTeamsMessage,
        string slackMessage,
        string twitterMessage)
    {
        _builder = builder;

        var message = "Version {0} of {1} has just been released, https://www.nuget.org/packages/{1}/.";

        _defaultMessage = defaultMessage ?? message;
        _gitterMessage = gitterMessage ?? "@/all " + _defaultMessage;
        _microsoftTeamsMessage = microsoftTeamsMessage ?? "@/all " + _defaultMessage;
        _slackMessage = slackMessage ?? "@/all " + _defaultMessage;
        _twitterMessage = twitterMessage ?? _defaultMessage;
    }

    private string FormatMessage(string message)
    {
        return String.Format(message, _builder.Version.Version, _builder.Parameters.Title);
    }
}