using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Nullability;

[DocFile]
public class StringNullability
{
    public class Thing
    {
        public int Id { get; set; }
        public string StringProperty { get; set; } = default!;
        public string? NullableStringProperty { get; set; } = default!;
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("`string` Generates `NOT NULL`.")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("[StringProperty] nvarchar(max) NOT NULL", reader.SkipToLineContaining("StringProperty"));
    }

    [Fact]
    [DocContent("`string?` Generates `NULL`.")]
    public void SqlServerNullable()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("[NullableStringProperty] nvarchar(max) NULL", reader.SkipToLineContaining("NullableStringProperty"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("`string` Generates `NOT NULL`.")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("\"StringProperty\" TEXT NOT NULL", reader.SkipToLineContaining("StringProperty"));
    }

    [Fact]
    [DocContent("`string?` Generates `NULL`.")]
    public void SqliteNullable()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("\"NullableStringProperty\" TEXT NULL", reader.SkipToLineContaining("NullableStringProperty"));
    }
}
