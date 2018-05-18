/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Task("SendMessageToTwitter")
    .WithCriteria(() => !publishingError)
    .WithCriteria(() => Build.Parameters.ShouldPostToTwitter)
    .Does(() =>
    {
        if (Build.Parameters.CanPostToTwitter)
        {
            Information("Sending message to Twitter...");

            TwitterSendTweet(
                Build.Credentials.Twitter.ConsumerKey,
                Build.Credentials.Twitter.ConsumerSecret,
                Build.Credentials.Twitter.AccessToken,
                Build.Credentials.Twitter.AccessTokenSecret,
                Build.Parameters.PublishMessage);

            Information("Message succcessfully sent.");
        }
        else
        {
            Warning("Unable to send message to Twitter, as necessary credentials are not available");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "SendMessageToTwitter");
    });