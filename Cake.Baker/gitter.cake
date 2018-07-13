/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.SendMessageToGitterTask = Task("SendMessageToGitter")
    .WithCriteria(() => !publishingError)
    .WithCriteria(() => Build.Parameters.ShouldPostToGitter)
    .Does(() =>
    {
        if (Build.Parameters.CanPostToGitter)
        {
            Information("Sending message to Gitter...");

            var postMessageResult = Gitter.Chat.PostMessage(
                message: Build.Messages.GitterMessage,
                messageSettings: new GitterChatMessageSettings
                {
                    Token = Build.Credentials.Gitter.Token,
                    RoomId = Build.Credentials.Gitter.RoomId
                });

            if (postMessageResult.Ok)
            {
                Information("Message {0} succcessfully sent.", postMessageResult.TimeStamp);
            }
            else
            {
                throw new Exception($"Failed to send message: {postMessageResult.Error}");
            }
        }
        else
        {
            Warning("Unable to send message to Gitter, as necessary credentials are not available.");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "SendMessageToGitter");
    });