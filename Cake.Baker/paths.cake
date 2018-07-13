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
    public DirectoryPath NuspecChocolatey { get; }
    public DirectoryPath NuspecNuGet { get; }
    public DirectoryPath Source { get; }
    public DirectoryPath Artifacts { get; }
    public DirectoryPath Docs { get; }
    public DirectoryPath Image { get; }
    public DirectoryPath Logs { get; }
    public DirectoryPath Packages { get; }
    public DirectoryPath PackagesChocolatey { get; }
    public DirectoryPath PackagesNuGet { get; }
    public DirectoryPath PackagesZip { get; }
    public DirectoryPath Published { get; }
    public DirectoryPath PublishedApplications { get; }
    public DirectoryPath PublishedLibraries { get; }
    public DirectoryPath PublishedWebApplications { get; }
    public DirectoryPath PublishedFixieTests { get; }
    public DirectoryPath PublishedMSTestTests { get; }
    public DirectoryPath PublishedNUnitTests { get; }
    public DirectoryPath PublishedNUnit3Tests { get; }
    public DirectoryPath PublishedXUnitTests { get; }
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
        DirectoryPath nuspecChocolatey,
        DirectoryPath nuspecNuGet,
        DirectoryPath source,
        DirectoryPath artifacts,
        DirectoryPath docs,
        DirectoryPath image,
        DirectoryPath logs,
        DirectoryPath packages,
        DirectoryPath packagesChocolatey,
        DirectoryPath packagesNuGet,
        DirectoryPath packagesZip,
        DirectoryPath published,
        DirectoryPath publishedApplications,
        DirectoryPath publishedLibraries,
        DirectoryPath publishedWebApplications,
        DirectoryPath publishedFixieTests,
        DirectoryPath publishedMSTestTests,
        DirectoryPath publishedNUnitTests,
        DirectoryPath publishedNUnit3Tests,
        DirectoryPath publishedXUnitTests,
        DirectoryPath tests,
        DirectoryPath testCoverage,
        DirectoryPath testReports,
        DirectoryPath testReportGenerator,
        DirectoryPath testReportUnit,
        DirectoryPath testResults)
    {
        root = root ?? context.MakeAbsolute(context.Directory("./"));
        artifacts = artifacts ?? root.Combine("artifacts");
        nuspec = nuspec ?? root.Combine("nuspec");
        packages = packages ?? artifacts.Combine("packages");
        published = published ?? artifacts.Combine("published");
        tests = tests ?? artifacts.Combine("tests");
        testReports = testReports ?? tests.Combine("reports");

        Root = root;
        Source = source ?? root.Combine("src");
        Artifacts = artifacts ?? root.Combine("artifacts");
        Docs = docs ?? artifacts.Combine("docs");
        Image = image ?? artifacts.Combine("image");
        Logs = logs ?? artifacts.Combine("logs");

        Nuspec = nuspec;
        NuspecChocolatey = nuspecChocolatey ?? nuspec.Combine("chocolatey");
        NuspecNuGet = nuspecNuGet ?? nuspec.Combine("nuget");

        Packages = packages;
        PackagesChocolatey = packagesChocolatey ?? packages.Combine("chocolatey");
        PackagesNuGet = packagesNuGet ?? packages.Combine("nuget");
        PackagesZip = packagesZip ?? packages.Combine("zip");

        Published = published;
        PublishedApplications = publishedApplications ?? published.Combine("Applications");
        PublishedLibraries = publishedLibraries ?? published.Combine("Libraries");
        PublishedWebApplications = publishedWebApplications ?? published.Combine("WebApplications");
        PublishedFixieTests = publishedFixieTests ?? published.Combine("FixieTests");
        PublishedMSTestTests = publishedMSTestTests ?? published.Combine("MSTestTests");
        PublishedNUnitTests = publishedNUnitTests ?? published.Combine("NUnitTests");
        PublishedNUnit3Tests = publishedNUnit3Tests ?? published.Combine("NUnit3Tests");
        PublishedXUnitTests = publishedXUnitTests ?? published.Combine("XUnitTests");

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
    public FilePath NUnit3OutputFile { get; }

    public FilePath OpenCover { get; }
    public FilePath FixieTestResults { get; }
    public FilePath MSTestTestResults { get; }
    public FilePath NUnitTestResults { get; }
    public FilePath NUnit3TestResults { get; }
    public FilePath XUnitTestResults { get; }

    public Files(
        ICakeContext context,
        Directories directories,
        FilePath license,
        FilePath releaseNotes,
        FilePath solution,
        FilePath solutionInfo,
        FilePath gitReleaseNotes,
        FilePath buildLog,
        FilePath nunit3OutputFile,
        FilePath openCover,
        FilePath fixieTestResults,
        FilePath msTestTestResults,
        FilePath nunitTestResults,
        FilePath nunit3TestResults,
        FilePath xunitTestResults)
    {
        License = license ?? directories.Root.CombineWithFilePath("LICENSE");
        ReleaseNotes = releaseNotes ?? directories.Root.CombineWithFilePath("RELEASENOTES.md");
        Solution = solution ?? context.GetFiles("./**/*.sln").FirstOrDefault();
        SolutionInfo = solutionInfo ?? context.GetFiles("./**/SolutionInfo.cs").FirstOrDefault();

        GitReleaseNotes = gitReleaseNotes ?? directories.Artifacts.CombineWithFilePath("GitReleaseNotes.md");
        BuildLog = buildLog ?? directories.Logs.CombineWithFilePath("MSBuild.log");
        NUnit3OutputFile = nunit3OutputFile ?? directories.Logs.CombineWithFilePath("NUnit3OutputFile.log");

        OpenCover = openCover ?? directories.TestCoverage.CombineWithFilePath("OpenCover.xml");
        FixieTestResults = fixieTestResults ?? directories.TestResults.CombineWithFilePath("FixieTestResults.xml");
        MSTestTestResults = msTestTestResults ?? directories.TestResults.CombineWithFilePath("MSTestTestResults.xml");
        NUnitTestResults = nunitTestResults ?? directories.TestResults.CombineWithFilePath("NUnitTestResults.xml");
        NUnit3TestResults = nunit3TestResults ?? directories.TestResults.CombineWithFilePath("NUnit3TestResults.xml");
        XUnitTestResults = xunitTestResults ?? directories.TestResults.CombineWithFilePath("XUnitTestResults.xml");
    }
}