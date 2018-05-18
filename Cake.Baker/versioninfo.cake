public class VersionInfo
{
    public string CakeVersion { get; }
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string FullSemVersion { get; private set; }
    public string InformationalVersion { get; private set; }
    public string Milestone { get; private set; }

    private readonly Builder _builder;
    private readonly ICakeContext _context;

    public VersionInfo(Builder builder)
    {
        _builder = builder;
        _context = builder.Context;

        CakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();
    }

    public void CalculateVersion()
    {
        if (_builder.Parameters.ShouldRunGitVersion)
        {
            _context.Information("");
            _context.Information("Calculating Semantic Version...");

            var noFetch = !_builder.Parameters.IsPublicRepository && _builder.Parameters.IsRunningOnAppVeyor;

            if (!_builder.BuildSystem.IsLocalBuild)
            {
                _context.GitVersion(new GitVersionSettings
                {
                    UpdateAssemblyInfoFilePath = _builder.Paths.Files.SolutionInfo,
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer,
                    NoFetch = noFetch
                });
            }

            GitVersion gitVersion = _context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json,
                NoFetch = noFetch
            });

            Version = gitVersion.MajorMinorPatch;
            SemVersion = gitVersion.LegacySemVerPadded;
            FullSemVersion = gitVersion.FullSemVer;
            Milestone = Version;
            InformationalVersion = gitVersion.InformationalVersion;

            _context.Information("Calculated Semantic Version: {0}", SemVersion);
        }

        if (_builder.Paths.Files.SolutionInfo.Exists() && (String.IsNullOrWhiteSpace(Version) || String.IsNullOrWhiteSpace(SemVersion)))
        {
            _context.Information("Fetching version from SolutionInfo...");

            var assemblyInfo = _context.ParseAssemblyInfo(_builder.Paths.Files.SolutionInfo);

            Version = assemblyInfo.AssemblyVersion;
            SemVersion = assemblyInfo.AssemblyInformationalVersion;
            FullSemVersion = assemblyInfo.AssemblyInformationalVersion;
            Milestone = assemblyInfo.AssemblyVersion;
            InformationalVersion = assemblyInfo.AssemblyInformationalVersion;
        }
    }
}