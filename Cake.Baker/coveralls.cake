Tasks.PublishCoverallsTask = Task("PublishCoveralls")
    .WithCriteria(() => Build.Parameters.ShouldPublishToCoveralls)
    .WithCriteria(() => FileExists(Build.Paths.Files.OpenCover))
    .Does(() => RequireTool(CoverallsTool, () =>
    {
        if (Build.Parameters.CanPublishToCoveralls)
        {
            CoverallsIo(Build.Paths.Files.OpenCover, new CoverallsIoSettings()
            {
                RepoToken = Build.Credentials.Coveralls.RepoToken
            });
        }
        else
        {
            Warning("Unable to publish to Coveralls, as necessary credentials are not available.");
        }
    }))
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishCoveralls");
        publishingError = true;
    });