/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.SendMessageToMicrosoftTeamsTask = Task("SendMessageToMicrosoftTeams")
    .WithCriteria(() => !publishingError)
    .WithCriteria(() => Build.Parameters.ShouldPostToMicrosoftTeams)
    .Does(() =>
    {
        if (Build.Parameters.CanPostToMicrosoftTeams)
        {
            Information("Sending message to Microsoft Teams...");

            MicrosoftTeamsPostMessage(Build.Messages.MicrosoftTeamsMessage,
                new MicrosoftTeamsSettings
                {
                    IncomingWebhookUrl = Build.Credentials.MicrosoftTeams.WebHookUrl
                });

            Information("Message succcessfully sent.");
        }
        else
        {
            Warning("Unable to send message to MicrosoftTeams, as necessary credentials are not available.");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "SendMessageToMicrosoftTeams");
    });