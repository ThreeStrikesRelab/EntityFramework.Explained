using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.AllIndexes;

[DocFile]
public class DefaultIndexNames
{
    public class CombinedThing
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class Thing
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class ThingDbContext<T> : DbContext where T : class
    {
        public DbSet<Thing> Things => Set<Thing>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlite("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Thing>()
            .HasIndex(t => t.Name);
        }
    }

    public class CombinedThingDbContext<T> : DbContext where T : class
    {
        public DbSet<CombinedThing> CombinedThings => Set<CombinedThing>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlite("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CombinedThing>()
            .HasIndex(t => new { t.Id, t.Name })
            .IsDescending(false, true);
        }
    }



    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("has entity with a default index")]
    public void SqlServer_DefaultIndex()
    {
        using var context = new ThingDbContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("CREATE INDEX \"IX_Things_Name\" ON \"Things\" (\"Name\");", reader.SkipToLineContaining("INDEX"));
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("has entity with combined sorted index")]
    public void SqlServer_CombinedIndex()
    {
        using var context = new CombinedThingDbContext<CombinedThing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("CREATE INDEX \"IX_CombinedThings_Id_Name\" ON \"CombinedThings\" (\"Id\", \"Name\" DESC);", reader.SkipToLineContaining("INDEX"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("has entity with a default index")]
    public void Sqlite_DefaultIndex()
    {
        using var context = new ThingDbContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("CREATE INDEX \"IX_Things_Name\" ON \"Things\" (\"Name\");", reader.SkipToLineContaining("INDEX"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("has entity with combined sorted index")]
    public void Sqlite_CombinedIndex()
    {
        using var context = new CombinedThingDbContext<CombinedThing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        reader.AsAssertsToLogFile();
        Assert.Contains("CREATE INDEX \"IX_CombinedThings_Id_Name\" ON \"CombinedThings\" (\"Id\", \"Name\" DESC);", reader.SkipToLineContaining("INDEX"));
    }
}
