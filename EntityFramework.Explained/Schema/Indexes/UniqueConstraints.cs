using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Indexes;

[DocFile]
public class UniqueConstraints
{
    [Index(nameof(Name), IsUnique = true)]
    public class Thing
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    [Index(nameof(Name), nameof(Id), IsUnique = true, IsDescending = new[] { false, true })]
    public class CombinedThing
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("Has unique index on Name.")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("CREATE UNIQUE INDEX [IX_Items_Name] ON [Items] ([Name]);", reader.SkipToLineContaining("INDEX"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Has combined unique index on Name and Id with a descending order on Id.")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<CombinedThing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("CREATE UNIQUE INDEX \"IX_Items_Name_Id\" ON \"Items\" (\"Name\", \"Id\" DESC);", reader.SkipToLineContaining("Index"));
    }
}