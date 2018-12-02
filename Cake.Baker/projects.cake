public class Projects
{
    public SolutionParserResult Solution { get; }
    public IEnumerable<SolutionProject> SolutionProjects { get; }
    public IEnumerable<CustomProjectParserResult> ParsedProjects { get; }

    public IEnumerable<CustomProjectParserResult> DotNetCoreApplications { get; }
    public IEnumerable<CustomProjectParserResult> DotNetCoreLibraries { get; }
    public IEnumerable<CustomProjectParserResult> DotNetCoreWebApplications { get; }
    public IEnumerable<CustomProjectParserResult> DotNetCoreTests { get; }
    public IEnumerable<CustomProjectParserResult> DotNetCoreIntegrationTests { get; }

    public IEnumerable<CustomProjectParserResult> DotNetFrameworkApplications { get; }
    public IEnumerable<CustomProjectParserResult> DotNetFrameworkLibraries { get; }
    public IEnumerable<CustomProjectParserResult> DotNetFrameworkWebApplications { get; }
    public IEnumerable<CustomProjectParserResult> DotNetFrameworkTests { get; }
    public IEnumerable<CustomProjectParserResult> DotNetFrameworkIntegrationTests { get; }

    public IEnumerable<CustomProjectParserResult> DotNetStandardLibraries { get; }

    public Projects(Builder builder)
    {
        ICakeContext context = builder.Context;

        if (builder.Paths.Files.Solution != null && context.FileExists(builder.Paths.Files.Solution))
        {
            var configuration = builder.Parameters.Configuration;
            var platformTarget = builder.ToolSettings.UsingBuildPlatformTarget;
            var testProjectPattern = builder.Parameters.TestProjectPattern;
            var integrationTestProjectPattern = builder.Parameters.IntegrationTestProjectPattern;

            var solution = context.ParseSolution(builder.Paths.Files.Solution);
            var solutionProjects = solution.GetProjects();
            var parsedProjects = solutionProjects.Select(p => context.ParseProject(p.Path, configuration, platformTarget));

            Solution = solution;
            SolutionProjects = solutionProjects;
            ParsedProjects = parsedProjects;

            DotNetCoreApplications = parsedProjects.Where(p =>
                p.IsNetCore &&
                !p.IsLibrary() &&
                !p.IsTestProject() &&
                !p.IsWebApplication());

            DotNetCoreLibraries = parsedProjects.Where(p =>
                p.IsNetCore &&
                p.IsLibrary() &&
                !p.IsTestProject() &&
                !p.IsWebApplication());

            DotNetCoreWebApplications = parsedProjects.Where(p =>
                p.IsNetCore &&
                !p.IsTestProject() &&
                p.IsWebApplication());

            DotNetCoreTests = parsedProjects.Where(p =>
                p.IsNetCore &&
                p.IsTestProject() &&
                !p.IsWebApplication() &&
                Match(p, testProjectPattern));

            DotNetCoreIntegrationTests = parsedProjects.Where(p =>
                p.IsNetCore &&
                p.IsTestProject() &&
                !p.IsWebApplication() &&
                Match(p, integrationTestProjectPattern));

            DotNetFrameworkApplications = parsedProjects.Where(p =>
                p.IsNetFramework &&
                !p.IsLibrary() &&
                !p.IsTestProject() &&
                !p.IsWebApplication());

            DotNetFrameworkLibraries = parsedProjects.Where(p =>
                p.IsNetFramework &&
                p.IsLibrary() &&
                !p.IsTestProject() &&
                !p.IsWebApplication());

            DotNetFrameworkWebApplications = parsedProjects.Where(p =>
                p.IsNetFramework &&
                !p.IsTestProject() &&
                p.IsWebApplication());

            DotNetFrameworkTests = parsedProjects.Where(p =>
                p.IsNetFramework &&
                p.IsTestProject() &&
                !p.IsWebApplication() &&
                Match(p, testProjectPattern));

            DotNetFrameworkIntegrationTests = parsedProjects.Where(p =>
                p.IsNetFramework &&
                p.IsTestProject() &&
                !p.IsWebApplication() &&
                Match(p, integrationTestProjectPattern));

            DotNetStandardLibraries = parsedProjects.Where(p =>
                p.IsNetStandard &&
                p.IsLibrary() &&
                !p.IsTestProject() &&
                !p.IsWebApplication());
        }
    }

    private bool Match(CustomProjectParserResult project, string pattern)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(project.ProjectFilePath.FullPath, pattern);
    }
}