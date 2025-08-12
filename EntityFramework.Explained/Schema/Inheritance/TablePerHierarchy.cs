using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
public class TablePerHierarchy
{
    public class AnimalDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Animal>()
            .HasDiscriminator<string>("AnimalType")
            .HasValue<Cat>("Cat")
            .HasValue<Dog>("Dog");

            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfMeows");

            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfBarks");
        }
    }
    public class Animal
    {
        public int Id { get; set; }
        public bool isPet { get; set; }
    }

    public class Cat : Animal
    {
        public int numberOfMeows { get; set; }
    }

    public class Dog : Animal
    {
        public int numberOfBarks { get; set; }
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes.")]
    public void SqlServer()
    {
        using var context = new AnimalDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("[StringProperty] nvarchar(max) NOT NULL", reader.SkipToLineContaining("StringProperty"));
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes")]
    public void Sqlite()
    {
        using var context = new AnimalDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Contains("\"StringProperty\" TEXT NOT NULL", reader.SkipToLineContaining("StringProperty"));
    }
}