using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Nullability;

[DocFile]
public class IntNullability
{
    public class Thing
    {
        public int Id { get; set; }
        public int IntProperty { get; set; } = default!;
        public int? NullIntProperty { get; set; } = default!;
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("`int` Generates `int NOT NULL`.")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("[IntProperty] int NOT NULL", reader.SkipToLineContaining("IntProperty"));
    }

    [Fact]
    [DocContent("`int?` Generates `int NULL`.")]
    public void SqlServerNullable()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("[NullIntProperty] int NULL", reader.SkipToLineContaining("NullIntProperty"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("`int` Generates `INTEGER NOT NULL`.")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("\"IntProperty\" INTEGER NOT NULL", reader.SkipToLineContaining("IntProperty"));
    }

    [Fact]
    [DocContent("`int?` Generates `INTEGER NULL`")]
    public void SqliteNullable()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("\"NullIntProperty\" INTEGER NULL", reader.SkipToLineContaining("NullIntProperty"));
    }
}
