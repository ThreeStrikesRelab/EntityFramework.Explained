using System.ComponentModel.DataAnnotations;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
public class RequiredAttributes
{
    public class Thing
    {
        public int Id { get; set; } = default!;

        [Required]
        public string StringProperty { get; set; } = default!;

        [Required(ErrorMessage = "lol")]
        public string? StringProp { get; set; } = "";
    }


    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("Generates required properties that will be created even if they are null or empty.")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE [Items] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [StringProperty] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    [StringProp] nvarchar(max) NOT NULL,", reader.NextLine());
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Generates required properties that will be created even if they are null or empty.")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("\"StringProperty\" TEXT NOT NULL", reader.SkipToLineContaining("StringProperty"));
        Assert.Contains("\"StringProp\" TEXT NOT NULL", reader.NextLine());
    }
}