public class ToolSettings
{
    public string TestCoverageFilter => String.Format(_testCoverageFilter, _builder.Parameters.Title);
    public string TestCoverageExcludeByAttribute { get; }
    public string TestCoverageExcludeByFile { get; }

    private readonly Builder _builder;
    private readonly string _testCoverageFilter;

    public ToolSettings(
        Builder builder,
        string testCoverageFilter,
        string testCoverageExcludeByAttribute,
        string testCoverageExcludeByFile)
    {
        _builder = builder;

        _testCoverageFilter = testCoverageFilter.DefaultValue("+[{0}*]* -[*.Tests]*");
        TestCoverageExcludeByAttribute = testCoverageExcludeByAttribute.DefaultValue("*.ExcludeFromCodeCoverage*");
        TestCoverageExcludeByFile = testCoverageExcludeByFile.DefaultValue("*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");
    }
}