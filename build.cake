#load nuget:https://www.myget.org/F/arkord/api/v2?package=Cake.Baker&version=0.11.0

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
        shouldPublishToGitHub: false,
        shouldPublishToMyGet: true)
    .Run();