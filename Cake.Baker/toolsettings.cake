public class ToolSettings
{
    public PlatformTarget BuildPlatformTarget { get; }
    public MSBuildToolVersion BuildMSBuildToolVersion { get; }
    public int BuildMaxCpuCount { get; }
    public bool BuildTreatWarningsAsErrors { get; }

    public bool NuGetSymbolPackage { get; }
    public bool NuGetNoPackageAnalysis { get; }

    public string TestCoverageFilter => String.Format(_testCoverageFilter, _builder.Parameters.Title);
    public string TestCoverageExcludeByAttribute { get; }
    public string TestCoverageExcludeByFile { get; }

    private readonly Builder _builder;
    private readonly string _testCoverageFilter;

    public ToolSettings(
        Builder builder,
        PlatformTarget buildPlatformTarget,
        MSBuildToolVersion buildMSBuildToolVersion,
        int buildMaxCpuCount,
        bool buildTreatWarningsAsErrors,
        bool nuGetSymbolPackage,
        bool nuGetNoPackageAnalysis,
        string testCoverageFilter,
        string testCoverageExcludeByAttribute,
        string testCoverageExcludeByFile)
    {
        _builder = builder;

        BuildPlatformTarget = buildPlatformTarget;
        BuildMSBuildToolVersion = buildMSBuildToolVersion;
        BuildMaxCpuCount = buildMaxCpuCount;
        BuildTreatWarningsAsErrors = buildTreatWarningsAsErrors;

        NuGetSymbolPackage = nuGetSymbolPackage;
        NuGetNoPackageAnalysis = nuGetNoPackageAnalysis;

        _testCoverageFilter = testCoverageFilter.DefaultValue("+[{0}*]* -[*.Tests]*");
        TestCoverageExcludeByAttribute = testCoverageExcludeByAttribute.DefaultValue("*.ExcludeFromCodeCoverage*");
        TestCoverageExcludeByFile = testCoverageExcludeByFile.DefaultValue("*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");
    }
}