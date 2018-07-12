public class Messages
{
    public string DefaultMessage => String.Format(_defaultMessage, _builder.Version.Version, _builder.Parameters.Title);
    public string TwitterMessage => _twitterMessage != null ? String.Format(_twitterMessage, _builder.Version.Version, _builder.Parameters.Title) : DefaultMessage;

    private readonly Builder _builder;

    private string _defaultMessage;
    private string _twitterMessage;

    public Messages(
        Builder builder,
        string defaultMessage,
        string twitterMessage)
    {
        _builder = builder;
        _defaultMessage = defaultMessage.DefaultValue("Version {0} of {1} has just been released, https://www.nuget.org/packages/{1}/.");
        _twitterMessage = twitterMessage;
    }
}