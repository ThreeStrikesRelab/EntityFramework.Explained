using System.ComponentModel.DataAnnotations;

using EntityFramework.Explained._Tools.TestContexts;
using QuickPulse.Explains;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
[DocFileHeader("Data Annotations: `[Range(...)]`")]
[DocContent("**Given:**")]
[DocCodeExample(typeof(Thing))]
public class DataAnnotations : SchemaPreTestBase
{
    [DocExample]
    public class Thing
    {
        public int Id { get; set; }
        [Range(0, 10)] // <= We are checking this 
        public int SecondInt { get; set; }
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("`[Range(0,10)]` gets ignored : `[SecondInt] int NOT NULL`.")]
    public void SqlServer()
    {
        var result = GetReader(new TestSqlServerContext<Thing>()).SkipToLineContaining("SecondInt");
        Assert.Contains("[SecondInt] int NOT NULL,", result);
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Same behaviour as Sql Server: `\"SecondInt\" INTEGER NOT NULL`.")]
    public void Sqlite()
    {
        var result = GetReader(new TestSqliteContext<Thing>()).SkipToLineContaining("SecondInt");
        Assert.Contains("\"SecondInt\" INTEGER NOT NULL", result);
    }
}
