#load nuget:https://www.myget.org/F/arkord/api/v2?package=Cake.Baker&prerelease

Task("PublishCakeBaker")
    .IsDependentOn("ShowInfo")
    .IsDependentOn("Clean")
    .IsDependentOn("CreateNuGetPackages")
    .IsDependentOn("PublishMyGetPackages")
    .IsDependentOn("PublishGitHubRelease");

Build
    .SetParameters(
        "Cake.Baker",
        "akordowski",
        shouldPublishToGitHub: true,
        shouldPublishToMyGet: true)
    .Run();