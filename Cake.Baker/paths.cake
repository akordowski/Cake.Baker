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
    public DirectoryPath Logs { get; }
    public DirectoryPath Packages { get; }
    public DirectoryPath PackagesNuGet { get; }
    public DirectoryPath PackagesZip { get; }
    public DirectoryPath Published { get; }
    public DirectoryPath PublishedApplications { get; }
    public DirectoryPath PublishedLibraries { get; }
    public DirectoryPath PublishedWebApplications { get; }
    public DirectoryPath PublishedNUnitTests { get; }
    public DirectoryPath PublishedXUnitTests { get; }
    public DirectoryPath PublishedMSTestTests { get; }
    public DirectoryPath PublishedFixieTests { get; }
    public DirectoryPath Tests { get; }
    public DirectoryPath TestCoverage { get; }
    public DirectoryPath TestReports { get; }
    public DirectoryPath TestReportGenerator { get; }
    public DirectoryPath TestReportUnit { get; }
    public DirectoryPath TestResults { get; }

    public Directories(
        ICakeContext context,
        DirectoryPath root,
        DirectoryPath nuspec,
        DirectoryPath source,
        DirectoryPath artifacts,
        DirectoryPath docs,
        DirectoryPath logs,
        DirectoryPath packages,
        DirectoryPath packagesNuGet,
        DirectoryPath packagesZip,
        DirectoryPath published,
        DirectoryPath publishedApplications,
        DirectoryPath publishedLibraries,
        DirectoryPath publishedWebApplications,
        DirectoryPath publishedNUnitTests,
        DirectoryPath publishedXUnitTests,
        DirectoryPath publishedMSTestTests,
        DirectoryPath publishedFixieTests,
        DirectoryPath tests,
        DirectoryPath testCoverage,
        DirectoryPath testReports,
        DirectoryPath testReportGenerator,
        DirectoryPath testReportUnit,
        DirectoryPath testResults)
    {
        root = root ?? context.MakeAbsolute(context.Directory("./"));
        artifacts = artifacts ?? root.Combine("artifacts");
        packages = packages ?? artifacts.Combine("packages");
        published = published ?? artifacts.Combine("published");
        tests = tests ?? artifacts.Combine("tests");
        testReports = testReports ??  tests.Combine("reports");

        Root = root;
        Nuspec = nuspec ?? root.Combine("nuspec");
        Source = source ?? root.Combine("src");
        Artifacts = artifacts ?? root.Combine("artifacts");
        Docs = docs ?? artifacts.Combine("docs");
        Logs = logs ?? artifacts.Combine("logs");

        Packages = packages;
        PackagesNuGet = packagesNuGet ?? packages.Combine("nuget");
        PackagesZip = packagesZip ?? packages.Combine("zip");

        Published = published;
        PublishedApplications = publishedApplications ?? published.Combine("Applications");
        PublishedLibraries = publishedLibraries ?? published.Combine("Libraries");
        PublishedWebApplications = publishedWebApplications ?? published.Combine("WebApplications");
        PublishedNUnitTests = publishedNUnitTests ?? published.Combine("NUnitTests");
        PublishedXUnitTests = publishedXUnitTests ?? published.Combine("XUnitTests");
        PublishedMSTestTests = publishedMSTestTests ?? published.Combine("MSTestTests");
        PublishedFixieTests = publishedFixieTests ?? published.Combine("FixieTests");

        Tests = tests;
        TestCoverage = testCoverage ?? tests.Combine("coverage");
        TestReports = testReports;
        TestReportGenerator = testReportGenerator ?? testReports.Combine("ReportGenerator");
        TestReportUnit = testReportUnit ?? testReports.Combine("ReportUnit");
        TestResults = testResults ?? tests.Combine("results");
    }
}

public class Files
{
    public FilePath License { get; }
    public FilePath ReleaseNotes { get; }
    public FilePath Solution { get; }
    public FilePath SolutionInfo { get; }

    public FilePath GitReleaseNotes { get; }
    public FilePath BuildLog { get; }
    public FilePath NUnitOutputFile { get; }

    public FilePath OpenCover { get; }
    public FilePath NUnitTestResults { get; }
    public FilePath XUnitTestResults { get; }
    public FilePath MSTestTestResults { get; }
    public FilePath FixieTestResults { get; }

    public Files(
        ICakeContext context,
        Directories directories,
        FilePath license,
        FilePath releaseNotes,
        FilePath solution,
        FilePath solutionInfo,
        FilePath gitReleaseNotes,
        FilePath buildLog,
        FilePath nunitOutputFile,
        FilePath openCover,
        FilePath nunitTestResults,
        FilePath xunitTestResults,
        FilePath msTestTestResults,
        FilePath fixieTestResults)
    {
        License = license ?? directories.Root.CombineWithFilePath("LICENSE");
        ReleaseNotes = releaseNotes ?? directories.Root.CombineWithFilePath("RELEASENOTES.md");
        Solution = solution ?? context.GetFiles("./**/*.sln").FirstOrDefault();
        SolutionInfo = solutionInfo ?? context.GetFiles("./**/SolutionInfo.cs").FirstOrDefault();

        GitReleaseNotes = gitReleaseNotes ?? directories.Artifacts.CombineWithFilePath("GitReleaseNotes.md");
        BuildLog = buildLog ?? directories.Logs.CombineWithFilePath("MSBuild.log");
        NUnitOutputFile = nunitOutputFile ?? directories.Logs.CombineWithFilePath("NUnitOutputFile.log");

        OpenCover = openCover ?? directories.TestCoverage.CombineWithFilePath("OpenCover.xml");
        NUnitTestResults = nunitTestResults ?? directories.TestResults.CombineWithFilePath("NUnitTestResults.xml");
        XUnitTestResults = xunitTestResults ?? directories.TestResults.CombineWithFilePath("XUnitTestResults.xml");
        MSTestTestResults = msTestTestResults ?? directories.TestResults.CombineWithFilePath("MSTestTestResults.xml");
        FixieTestResults = fixieTestResults ?? directories.TestResults.CombineWithFilePath("FixieTestResults.xml");
    }
}