/* ---------------------------------------------------------------------------------------------------- */
/* Task Definitions */

Task("PublishGitHubRelease")
    .WithCriteria(() => Build.Parameters.ShouldPublishToGitHub)
    .Does(() => RequireTool(GitReleaseManagerTool, () =>
    {
        if (Build.Parameters.CanPublishToGitHub)
        {
            var version = Build.Version.SemVersion;
            var assets = GetGitReleaseAssets();
            var releaseNotes = GetGitReleaseNotes();

            if (assets == null)
            {
                Warning("Assets are not available");
            }

            if (releaseNotes == null)
            {
                Warning("Release Notes are not available");
            }

            new GitHub(Build)
                .Create(new GitReleaseManagerCreateSettings
                {
                    Name = version,
                    InputFilePath = releaseNotes,
                    Prerelease = Build.Parameters.IsPrerelease,
                    Assets = assets,
                    TargetCommitish = Build.Parameters.RepositoryBranch
                })
                .Publish(version);
        }
        else
        {
            Warning("Unable to publish to GitHub, as necessary credentials are not available");
        }
    }))
    .OnError(ex =>
    {
        Error(ex.Message);
        Information("{0} Task failed, but continuing with next Task...", "PublishGitHubRelease");
        publishingError = true;
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Methods */

public string GetGitReleaseAssets()
{
    var files = GetFiles(Build.Paths.Directories.Packages + "/**/*");
    var assets = files.Any() ? String.Join(",", files) : null;

    return assets;
}

public string GetGitReleaseNotes()
{
    string path = null;

    if (Build.Parameters.ReleaseNotes != null)
    {
        path = Build.Paths.Files.GitReleaseNotes.FullPath;
        System.IO.File.WriteAllLines(path, Build.Parameters.ReleaseNotes.Notes.ToArray());
    }

    return path;
}

/* ---------------------------------------------------------------------------------------------------- */
/* Classes */

public class GitHub
{
    private readonly ICakeContext _context;
    private readonly GitHubCredentials _credentials;
    private readonly string _repositoryOwner;
    private readonly string _repositoryName;

    public GitHub(Builder builder)
    {
        _context = builder.Context;
        _credentials = builder.Credentials.GitHub;
        _repositoryOwner = builder.Parameters.RepositoryOwner;
        _repositoryName = builder.Parameters.RepositoryName;
    }

    public GitHub AddAssets(string tagName, string assets)
    {
        _context.GitReleaseManagerAddAssets(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            tagName,
            assets);

        return this;
    }

    public GitHub AddAssets(string tagName, string assets, GitReleaseManagerAddAssetsSettings settings)
    {
        _context.GitReleaseManagerAddAssets(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            tagName,
            assets,
            settings);

        return this;
    }

    public GitHub Close(string milestone)
    {
        _context.GitReleaseManagerClose(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            milestone);

        return this;
    }

    public GitHub Close(string milestone, GitReleaseManagerCloseMilestoneSettings settings)
    {
        _context.GitReleaseManagerClose(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            milestone,
            settings);

        return this;
    }

    public GitHub Create()
    {
        _context.GitReleaseManagerCreate(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName);

        return this;
    }

    public GitHub Create(GitReleaseManagerCreateSettings settings)
    {
        _context.GitReleaseManagerCreate(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            settings);

        return this;
    }

    public GitHub Export(FilePath fileOutputPath)
    {
        _context.GitReleaseManagerExport(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            fileOutputPath);

        return this;
    }

    public GitHub Export(FilePath fileOutputPath, GitReleaseManagerExportSettings settings)
    {
        _context.GitReleaseManagerExport(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            fileOutputPath,
            settings);

        return this;
    }

    public GitHub Publish(string tagName)
    {
        _context.GitReleaseManagerPublish(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            tagName);

        return this;
    }

    public GitHub Publish(string tagName, GitReleaseManagerPublishSettings settings)
    {
        _context.GitReleaseManagerPublish(
            _credentials.Username,
            _credentials.Password,
            _repositoryOwner,
            _repositoryName,
            tagName,
            settings);

        return this;
    }
}