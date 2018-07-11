/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.PostMessageToTwitterTask = Task("PostMessageToTwitter")
    .WithCriteria(() => !publishingError)
    .WithCriteria(() => Build.Parameters.ShouldPostToTwitter)
    .Does(() =>
    {
        if (Build.Parameters.CanPostToTwitter)
        {
            Information("Post message to Twitter...");

            TwitterSendTweet(
                Build.Credentials.Twitter.ConsumerKey,
                Build.Credentials.Twitter.ConsumerSecret,
                Build.Credentials.Twitter.AccessToken,
                Build.Credentials.Twitter.AccessTokenSecret,
                Build.Parameters.PostMessage);

            Information("Message succcessfully posted.");
        }
        else
        {
            Warning("Unable to post message to Twitter, as necessary credentials are not available");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PostMessageToTwitter");
    });