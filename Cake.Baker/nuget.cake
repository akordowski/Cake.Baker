/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Tasks.CreateNuGetPackagesTask = Task("CreateNuGetPackages")
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.Nuspec))
    .Does(() =>
    {
        Information("Creating NuGet Packages...");
        CleanDirectory(Build.Paths.Directories.PackagesNuGet);

        var nuspecFiles = GetFiles(Build.Paths.Directories.Nuspec + "/**/*.nuspec");

        foreach (var nuspecFile in nuspecFiles)
        {
            var nuspecName = nuspecFile.GetFilenameWithoutExtension().ToString();
            var basePath = Build.Paths.Directories.Nuspec;

            if (DirectoryExists(Build.Paths.Directories.Image))
            {
                basePath = Build.Paths.Directories.Image;
            }
            else if (DirectoryExists(Build.Paths.Directories.PublishedLibraries.Combine(nuspecName)))
            {
                basePath = Build.Paths.Directories.PublishedLibraries.Combine(nuspecName);
            }
            else if (DirectoryExists(Build.Paths.Directories.PublishedApplications.Combine(nuspecName)))
            {
                basePath = Build.Paths.Directories.PublishedApplications.Combine(nuspecName);
            }

            Information("Setting BasePath '{0}' for nuspec file '{1}'", basePath, nuspecName);

            NuGetPack(nuspecFile, new NuGetPackSettings
            {
                Version = Build.Version.SemVersion,
                ReleaseNotes = Build.Parameters.ReleaseNotes?.Notes.ToArray(),
                BasePath = basePath,
                OutputDirectory = Build.Paths.Directories.PackagesNuGet,
                Symbols = Build.ToolSettings.NuGetSymbolPackage,
                NoPackageAnalysis = Build.ToolSettings.NuGetNoPackageAnalysis
            });
        }
    });

Tasks.PublishNuGetPackagesTask = Task("PublishNuGetPackages")
    .WithCriteria(() => Build.Parameters.ShouldPublishToNuGet)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PackagesNuGet))
    .Does(() =>
    {
        if (Build.Parameters.CanPublishToNuGet)
        {
            var nupkgFiles = GetFiles(Build.Paths.Directories.PackagesNuGet + "/**/*.nupkg");

            foreach (var nupkgFile in nupkgFiles)
            {
                NuGetPush(nupkgFile, new NuGetPushSettings
                {
                    ApiKey = Build.Credentials.NuGet.ApiKey,
                    Source = Build.Credentials.NuGet.Source
                });
            }
        }
        else
        {
            Warning("Unable to publish to NuGet, as necessary credentials are not available");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishNuGetPackages");
        publishingError = true;
    });

Tasks.PublishMyGetPackagesTask = Task("PublishMyGetPackages")
    .WithCriteria(() => Build.Parameters.ShouldPublishToMyGet)
    .WithCriteria(() => DirectoryExists(Build.Paths.Directories.PackagesNuGet))
    .Does(() =>
    {
        if (Build.Parameters.CanPublishToMyGet)
        {
            var nupkgFiles = GetFiles(Build.Paths.Directories.PackagesNuGet + "/**/*.nupkg");

            foreach (var nupkgFile in nupkgFiles)
            {
                NuGetPush(nupkgFile, new NuGetPushSettings
                {
                    ApiKey = Build.Credentials.MyGet.ApiKey,
                    Source = Build.Credentials.MyGet.Source
                });
            }
        }
        else
        {
            Warning("Unable to publish to MyGet, as necessary credentials are not available");
        }
    })
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishMyGetPackages");
        publishingError = true;
    });