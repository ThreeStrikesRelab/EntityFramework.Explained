using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Nullability;

[DocFile]
public class ClassNullability
{
    public class ThingTwo
    {
        public int Id { get; set; }
    }

    public class Thing
    {
        public int Id { get; set; }
        public ThingTwo? NullThing { get; set; } = default!;
        public ThingTwo SomeThing { get; set; } = default!;
    }

    [Fact]
    public void SqlServer_IsNull()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("[NullThingId] int NULL", reader.SkipToLineContaining("NullThingId"));

    }

    [Fact]
    public void SqlServer_IsNotNull()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);


        Assert.Contains("[SomeThingId] int NOT NULL", reader.SkipToLineContaining("SomeThingId"));
    }

    [Fact]
    public void Sqlite_IsNull()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("\"NullThingId\" INTEGER NULL", reader.SkipToLineContaining("NullThingId"));
    }

    [Fact]
    public void Sqlite_IsNotNull()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("\"SomeThingId\" INTEGER NOT NULL", reader.SkipToLineContaining("SomeThingId"));
    }
}