/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.SendMessageToSlackTask = Task("SendMessageToSlack")
    .WithCriteria(() => !publishingError)
    .WithCriteria(() => Build.Parameters.ShouldPostToSlack)
    .Does(() =>
    {
        if (Build.Parameters.CanPostToSlack)
        {
            Information("Sending message to Slack...");

            var postMessageResult = Slack.Chat.PostMessage(
                token: Build.Credentials.Slack.Token,
                channel: Build.Credentials.Slack.Channel,
                text: Build.Messages.SlackMessage);

            if (postMessageResult.Ok)
            {
                Information("Message {0} successfully sent.", postMessageResult.TimeStamp);
            }
            else
            {
                throw new Exception($"Failed to send message: {postMessageResult.Error}");
            }
        }
        else
        {
            Warning("Unable to send message to Slack, as necessary credentials are not available.");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "SendMessageToSlack");
    });