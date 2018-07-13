Tasks.PublishCodecovTask = Task("PublishCodecov")
    .WithCriteria(() => Build.Parameters.ShouldPublishToCodecov)
    .WithCriteria(() => FileExists(Build.Paths.Files.OpenCover))
    .Does(() => RequireTool(CodecovTool, () =>
    {
        if (Build.Parameters.CanPublishToCodecov)
        {
            var settings = new CodecovSettings
            {
                Files = new[] { Build.Paths.Files.OpenCover.ToString() },
                Required = true
            };

            if (Build.Parameters.IsRunningOnAppVeyor &&
                !String.IsNullOrWhiteSpace(Build.Version?.FullSemVersion))
            {
                var buildVersion = string.Format("{0}.build.{1}",
                    Build.Version.FullSemVersion,
                    BuildSystem.AppVeyor.Environment.Build.Number);

                settings.EnvironmentVariables = new Dictionary<string, string> { { "APPVEYOR_BUILD_VERSION", buildVersion } };
            }

            Codecov(settings);
        }
        else
        {
            Warning("Unable to publish to Codecov, as necessary credentials are not available.");
        }
    }))
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishCodecov");
        publishingError = true;
    });