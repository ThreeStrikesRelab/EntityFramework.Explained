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
    [DocHeader("Sql Server")]
    [DocContent("`NullThing` Generates `NULL`.")]
    public void SqlServer_IsNull()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("[NullThingId] int NULL", reader.SkipToLineContaining("NullThingId"));

    }

    [Fact]
    [DocContent("`SomeThing` Generates `NOT NULL`.")]
    public void SqlServer_IsNotNull()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("[SomeThingId] int NOT NULL", reader.SkipToLineContaining("SomeThingId"));
    }

    [Fact]
    [DocContent("`SomeThingId` adds `ON DELETE CASCADE` to the foreign key definition")]
    public void SqlServer_ForeignKeyParameter()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("ON DELETE CASCADE", reader.SkipToLineContaining("FK_Items_ThingTwo_SomeThingId"));
    }

    [Fact]
    [DocContent("`NullThing` does not add `ON DELETE CASCADE` to the foreign key definition")]
    public void SaqlServer_ForeignKeyNoParameter()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.DoesNotContain("ON DELETE CASCADE", reader.SkipToLineContaining("FK_Items_ThingTwo_NullThingId"));
    }

    // SQLite
    [Fact]
    [DocHeader("Sq Lite")]
    [DocContent("`NullThing` Generates `NULL`.")]
    public void Sqlite_IsNull()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("\"NullThingId\" INTEGER NULL", reader.SkipToLineContaining("NullThingId"));
    }

    [Fact]
    [DocContent("`SomeThing` Generates `NOT NULL`.")]
    public void Sqlite_IsNotNull()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("\"SomeThingId\" INTEGER NOT NULL", reader.SkipToLineContaining("SomeThingId"));
    }

    [Fact]
    [DocContent("`SomeThingId` adds `ON DELETE CASCADE` to the foreign key definition")]
    public void Sqlite_ForeignKeyParameter()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Contains("ON DELETE CASCADE", reader.SkipToLineContaining("FK_Items_ThingTwo_SomeThingId"));
    }

    [Fact]
    [DocContent("`NullThing` does not add `ON DELETE CASCADE` to the foreign key definition")]
    public void Saqlite_ForeignKeyNoParameter()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.DoesNotContain("ON DELETE CASCADE", reader.SkipToLineContaining("FK_Items_ThingTwo_NullThingId"));
    }
}
