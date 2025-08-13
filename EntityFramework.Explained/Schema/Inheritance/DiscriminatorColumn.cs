using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Inheritance;

[DocFile]
public class DiscriminatorColumn
{
    public class AnimalDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

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

    public class AnimalSqlServerDbContext<T> : AnimalDbContext<T> where T : class
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("DoesNotMatter");
    }

    public class AnimalSqliteDbContext<T> : AnimalDbContext<T> where T : class
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DoesNotMatter");
    }

    public class Animal
    {
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
    [DocContent("has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes. Property with type bool will generate an int in the database in SQL Server (output 0 or 1). So we used type string instead.")]
    public void SqlServer()
    {
        using var context = new AnimalSqlServerDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE [Animals] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [isPet] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    [AnimalType] nvarchar(8) NOT NULL,", reader.NextLine());
        Assert.Equal("    [numberOfBarks] int NULL,", reader.NextLine());
        Assert.Equal("    [numberOfMeows] int NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])", reader.NextLine());
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes")]
    public void Sqlite()
    {
        using var context = new AnimalSqliteDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Animals\" PRIMARY KEY AUTOINCREMENT,", reader.SkipToLineContaining("Id"));
        Assert.Equal("    \"isPet\" TEXT NOT NULL,", reader.NextLine());
        Assert.Equal("    \"AnimalType\" TEXT NOT NULL,", reader.NextLine());
        Assert.Equal("    \"numberOfBarks\" INTEGER NULL,", reader.NextLine());
        Assert.Equal("    \"numberOfMeows\" INTEGER NULL", reader.NextLine());
    }
}