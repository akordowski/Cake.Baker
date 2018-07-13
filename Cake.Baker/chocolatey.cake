Tasks.CreateChocolateyPackagesTask = Task("CreateChocolateyPackages")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.NuspecChocolatey))
    .Does(() =>
    {
        Information("Creating Chocolatey Packages...");
        CleanDirectory(Build.Paths.Directories.PackagesChocolatey);

        var nuspecFiles = GetFiles(Build.Paths.Directories.NuspecChocolatey + "/**/*.nuspec");

        foreach (var nuspecFile in nuspecFiles)
        {
            ChocolateyPack(nuspecFile, new ChocolateyPackSettings
            {
                Version = Build.Version.SemVersion,
                ReleaseNotes = Build.Parameters.ReleaseNotes?.Notes.ToArray(),
                OutputDirectory = Build.Paths.Directories.PackagesChocolatey
            });
        }
    });

Tasks.PublishChocolateyPackagesTask = Task("PublishChocolateyPackages")
    .WithCriteria(() => Build.Parameters.ShouldPublishToChocolatey)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PackagesChocolatey))
    .Does(() =>
    {
        if (Build.Parameters.CanPublishToChocolatey)
        {
            var nupkgFiles = GetFiles(Build.Paths.Directories.PackagesChocolatey + "/**/*.nupkg");

            foreach (var nupkgFile in nupkgFiles)
            {
                ChocolateyPush(nupkgFile, new ChocolateyPushSettings
                {
                    ApiKey = Build.Credentials.Chocolatey.ApiKey,
                    Source = Build.Credentials.Chocolatey.Source
                });
            }
        }
        else
        {
            Warning("Unable to publish to Chocolatey, as necessary credentials are not available.");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishChocolateyPackages");
        publishingError = true;
    });