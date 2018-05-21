public class Paths
{
    public Directories Directories { get; }
    public Files Files { get; }

    public Paths(Directories directories, Files files)
    {
        Directories = directories;
        Files = files;
    }
}

public class Directories
{
    public DirectoryPath Root { get; }
    public DirectoryPath Nuspec { get; }
    public DirectoryPath Source { get; }
    public DirectoryPath Artifacts { get; }
    public DirectoryPath Docs { get; }
    public DirectoryPath Log { get; }
    public DirectoryPath Packages { get; }
    public DirectoryPath PackagesNuGet { get; }
    public DirectoryPath PackagesZip { get; }
    public DirectoryPath Tmp { get; }
    public DirectoryPath PublishedApplications { get; }
    public DirectoryPath PublishedLibraries { get; }
    public DirectoryPath PublishedWebApplications { get; }
    public DirectoryPath PublishedNUnitTests { get; }
    public DirectoryPath PublishedXUnitTests { get; }
    public DirectoryPath PublishedMSTestTests { get; }
    public DirectoryPath PublishedFixieTests { get; }
    public DirectoryPath Tests { get; }
    public DirectoryPath TestCoverage { get; }
    public DirectoryPath TestReport { get; }
    public DirectoryPath TestResults { get; }
    public DirectoryPath TestResultsNUnit { get; }
    public DirectoryPath TestResultsXUnit { get; }
    public DirectoryPath TestResultsMSTest { get; }
    public DirectoryPath TestResultsFixie { get; }

    public Directories(
        ICakeContext context,
        DirectoryPath root,
        DirectoryPath nuspec,
        DirectoryPath source,
        DirectoryPath artifacts,
        DirectoryPath docs,
        DirectoryPath log,
        DirectoryPath packages,
        DirectoryPath packagesNuGet,
        DirectoryPath packagesZip,
        DirectoryPath tmp,
        DirectoryPath publishedApplications,
        DirectoryPath publishedLibraries,
        DirectoryPath publishedWebApplications,
        DirectoryPath publishedNUnitTests,
        DirectoryPath publishedXUnitTests,
        DirectoryPath publishedMSTestTests,
        DirectoryPath publishedFixieTests,
        DirectoryPath tests,
        DirectoryPath testCoverage,
        DirectoryPath testReport,
        DirectoryPath testResults,
        DirectoryPath testResultsNUnit,
        DirectoryPath testResultsXUnit,
        DirectoryPath testResultsMSTest,
        DirectoryPath testResultsFixie)
    {
        root = root ?? context.MakeAbsolute(context.Directory("./"));
        artifacts = artifacts ?? root.Combine("artifacts");
        packages = packages ?? artifacts.Combine("packages");
        tmp = tmp ?? artifacts.Combine("tmp");
        tests = tests ?? artifacts.Combine("tests");
        testResults = testResults ?? tests.Combine("results");

        Root = root;
        Nuspec = nuspec ?? root.Combine("nuspec");
        Source = source ?? root.Combine("src");
        Artifacts = artifacts ?? root.Combine("artifacts");
        Docs = docs ?? artifacts.Combine("docs");
        Log = log ?? artifacts.Combine("log");
        Packages = packages;
        PackagesNuGet = packagesNuGet ?? packages.Combine("nuget");
        PackagesZip = packagesZip ?? packages.Combine("zip");

        Tmp = tmp;
        PublishedApplications = publishedApplications ?? tmp.Combine("PublishedApplications");
        PublishedLibraries = publishedLibraries ?? tmp.Combine("PublishedLibraries");
        PublishedWebApplications = publishedWebApplications ?? tmp.Combine("PublishedWebApplications");
        PublishedNUnitTests = publishedNUnitTests ?? tmp.Combine("PublishedNUnitTests");
        PublishedXUnitTests = publishedXUnitTests ?? tmp.Combine("PublishedXUnitTests");
        PublishedMSTestTests = publishedMSTestTests ?? tmp.Combine("PublishedMSTestTests");
        PublishedFixieTests = publishedFixieTests ?? tmp.Combine("PublishedFixieTests");

        Tests = tests;
        TestCoverage = testCoverage ?? tests.Combine("coverage");
        TestReport = testReport ??  tests.Combine("report");
        TestResults = testResults;
        TestResultsNUnit = testResultsNUnit ?? testResults.Combine("NUnit");
        TestResultsXUnit = testResultsXUnit ?? testResults.Combine("XUnit");
        TestResultsMSTest = testResultsMSTest ?? testResults.Combine("MSTest");
        TestResultsFixie = testResultsFixie ?? testResults.Combine("Fixie");
    }
}

public class Files
{
    public FilePath License { get; }
    public FilePath ReleaseNotes { get; }
    public FilePath GitReleaseNotes { get; }
    public FilePath BuildLog { get; }
    public FilePath TestCoverageOutput { get; }
    public FilePath Solution { get; }
    public FilePath SolutionInfo { get; }

    public Files(
        ICakeContext context,
        Directories directories,
        FilePath license,
        FilePath releaseNotes,
        FilePath gitReleaseNotes,
        FilePath buildLog,
        FilePath testCoverageOutput,
        FilePath solution,
        FilePath solutionInfo)
    {
        License = license ?? directories.Root.CombineWithFilePath("LICENSE");
        ReleaseNotes = releaseNotes ?? directories.Root.CombineWithFilePath("RELEASENOTES.md");
        GitReleaseNotes = gitReleaseNotes ?? directories.Artifacts.CombineWithFilePath("GITRELEASENOTES.md");
        BuildLog = buildLog ?? directories.Log.CombineWithFilePath("MSBuild.log");
        TestCoverageOutput = testCoverageOutput ?? directories.TestCoverage.CombineWithFilePath("OpenCover.xml");
        Solution = solution ?? context.GetFiles("./**/*.sln").FirstOrDefault();
        SolutionInfo = solutionInfo ?? context.GetFiles("./**/SolutionInfo.cs").FirstOrDefault();
    }
}