using QuickPulse.Explains;

namespace EntityFramework.Explained;

[DocFile]
[DocFileHeader("EntityFramework.Explained")]
[DocContent("> Because We Need to Talk About Kevin.")]
public class CreateDoc
{
    [Fact(Skip = "Not All the Time, Please")]
    [DocHeader("Dependencies", 2)]
    [DocContent("* [QuickPulse.Explains](https://github.com/kilfour/QuickPulse.Explains)")]
    [DocContent("* [QuickPulse.Show](https://github.com/kilfour/QuickPulse.Show)")]
    public void Now()
    {
        Explain.This<CreateDoc>("README.md");
    }
}