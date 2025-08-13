using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Inheritance;

[DocFile]
public class TablePerHierarchy
{
    public class AnimalSqliteDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfMeows");

            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfBarks");
        }
    }

    public class AnimalServerDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfMeows");

            modelBuilder.Entity<Animal>()
            .Property<int?>("numberOfBarks");
        }
    }
    public class Animal
    {
        [Required]
        public int Id { get; set; }

        public string isPet { get; set; }
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
    [DocContent("has tph with inherited properties of base class and properties of derived classes.")]

    public void SqlServer_Has_TablePerHierarchy()
    {
        using var context = new AnimalServerDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.SkipToLineContaining("Id"));
        Assert.Equal("    [isPet] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    [numberOfBarks] int NULL,", reader.NextLine());
        Assert.Equal("    [numberOfMeows] int NULL,", reader.NextLine());

    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("has tph with inherited properties of base class and properties of derived classes.")]
    public void Sqlite_Has_TablePerHierarchy()
    {
        using var context = new AnimalSqliteDbContext<Animal>();

        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Animals\" PRIMARY KEY AUTOINCREMENT,", reader.SkipToLineContaining("Id"));
        Assert.Equal("    \"isPet\" TEXT NOT NULL,", reader.NextLine());
        Assert.Equal("    \"numberOfBarks\" INTEGER NULL,", reader.NextLine());
        Assert.Equal("    \"numberOfMeows\" INTEGER NULL", reader.NextLine());
    }
}