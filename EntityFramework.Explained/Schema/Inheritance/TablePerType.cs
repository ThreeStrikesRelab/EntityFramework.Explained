using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;


namespace EntityFramework.Explained.Schema.Inheritance;

[DocFile]
[DocFileHeader("Inheritance:`Table per type`")]
[DocContent("**If base class is made and derived classes too, table per type is generated like this:**")]
[DocExample(typeof(AnimalServerDbContext<>))]
public class TablePerType
{
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

    [CodeExample]
    public class AnimalServerDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dog>()
            .ToTable("Dogs");

            modelBuilder.Entity<Cat>()
            .ToTable("Cats");

        }
    }

    public class AnimalSqliteDbContext<T> : DbContext where T : class
    {
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DoesNotMatter");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dog>()
            .ToTable("Dogs");
            modelBuilder.Entity<Cat>()
            .ToTable("Cats");
        }
    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("Sql Server creates tables for the base AND derived classes using the ToTable() method. The tables are 1:1 related with each other with the property Id. UseTptMappingStrategy() is recommended by Microsoft but this doesn't work here.")]
    public void SqlServer_Creates_TPT()
    {
        using var context = new AnimalServerDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE [Animals] (", reader.SkipToLineContaining("Animals"));
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [isPet] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Animals] PRIMARY KEY ([Id])", reader.NextLine());

        Assert.Equal("CREATE TABLE [Cats] (", reader.SkipToLineContaining("Cats"));
        Assert.Equal("    [Id] int NOT NULL,", reader.NextLine());
        Assert.Equal("    [numberOfMeows] int NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Cats] PRIMARY KEY ([Id]),", reader.NextLine());
        Assert.Equal("    CONSTRAINT [FK_Cats_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id]) ON DELETE CASCADE", reader.NextLine());

        Assert.Equal("CREATE TABLE [Dogs] (", reader.SkipToLineContaining("Dogs"));
        Assert.Equal("    [Id] int NOT NULL,", reader.NextLine());
        Assert.Equal("    [numberOfBarks] int NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Dogs] PRIMARY KEY ([Id]),", reader.NextLine());
        Assert.Equal("    CONSTRAINT [FK_Dogs_Animals_Id] FOREIGN KEY ([Id]) REFERENCES [Animals] ([Id]) ON DELETE CASCADE", reader.NextLine());

    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Sqlite creates tables for the base AND derived classes (=tpt) using the ToTable() method. The tables are 1:1 related with each other with the property Id. UseTptMappingStrategy() is not compatible with Sqlite. It will only create one table of the base class.")]
    public void Sqlite_Creates_TPT()
    {
        using var context = new AnimalSqliteDbContext<Animal>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE \"Animals\" (", reader.SkipToLineContaining("Animals"));
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Animals\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"isPet\" TEXT NOT NULL", reader.NextLine());

        Assert.Equal("CREATE TABLE \"Cats\" (", reader.SkipToLineContaining("Cats"));
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Cats\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"numberOfMeows\" INTEGER NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT \"FK_Cats_Animals_Id\" FOREIGN KEY (\"Id\") REFERENCES \"Animals\" (\"Id\") ON DELETE CASCADE", reader.NextLine());

        Assert.Equal("CREATE TABLE \"Dogs\" (", reader.SkipToLineContaining("Dogs"));
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Dogs\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"numberOfBarks\" INTEGER NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT \"FK_Dogs_Animals_Id\" FOREIGN KEY (\"Id\") REFERENCES \"Animals\" (\"Id\") ON DELETE CASCADE", reader.NextLine());

    }


}