using System.ComponentModel.DataAnnotations;
using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
public class GenericIdentity
{
    public record Id<T>(Guid Value)
    {
        public override string ToString() => $"{typeof(T).Name}: {Value}";
    }
    public class Thing
    {
        public Id<Thing> Id { get; set; }

        public string Name { get; set; } = default!;
    }

    [Fact]
    [DocHeader("Sql Server - Generic Identity")]
    [DocContent("looking what schema does for Generic Identity without mapping")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var ex = Assert.Throws<InvalidOperationException>(() => context.Database.GenerateCreateScript());
        Assert.Contains("The entity type 'Id<Thing>' requires a primary key to be defined.", ex.Message);
    }

    [Fact]
    [DocHeader("Sqlite - Generic Identity")]
    [DocContent("looking what schema does for Generic Identity without mapping")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Thing>();
        var ex = Assert.Throws<InvalidOperationException>(() => context.Database.GenerateCreateScript());
        Assert.Contains("The entity type 'Id<Thing>' requires a primary key to be defined.", ex.Message);
    }



    public static void MappingWithConverterAndGI(ModelBuilder modelbuilder)
    {
        // ValueConverter from Id<Thing> <-> Guid
        var idConverter = new ValueConverter<Id<Thing>, Guid>(
            id => id.Value,
            guid => new Id<Thing>(guid)
        );

        modelbuilder.Entity<Thing>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(idConverter)
                .ValueGeneratedNever(); // EF won’t auto-generate GUID

            entity.Property(e => e.Name)
                .IsRequired();
        });
    }


    //Generated the GenericAppDbContext using AI
    public class GenericAppDbContextSQLserver<T> : DbContext where T : class
    {
        public DbSet<Thing> Things { get; set; } = default!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("DoesNotMatter"); // Required by EF, never actually used
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingWithConverterAndGI(modelBuilder);
        }
    }

    public class GenericAppDbContextSQLite<T> : DbContext where T : class
    {
        public DbSet<Thing> Things { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DoesNotMatter"); // Required by EF, never actually used
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingWithConverterAndGI(modelBuilder);
        }
    }




    [Fact]
    [DocHeader("Sql Server - Generic Identity")]
    [DocContent("looking what schema does for Generic Identity with mapping")]
    public void SqlServer2()
    {
        using var context = new GenericAppDbContextSQLserver<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE [Things] (", reader.NextLine());
        Assert.Equal("    [Id] uniqueidentifier NOT NULL,", reader.NextLine());
        Assert.Equal("    [Name] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Things] PRIMARY KEY ([Id])", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
    }

    [Fact]
    [DocHeader("Sqlite - Generic Identity")]
    [DocContent("looking what schema does for Generic Identity with mapping")]
    public void Sqlite2()
    {
        using var context = new GenericAppDbContextSQLite<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE \"Things\" (", reader.NextLine());
        Assert.Equal("    \"Id\" TEXT NOT NULL CONSTRAINT \"PK_Things\" PRIMARY KEY,", reader.NextLine());
        Assert.Equal("    \"Name\" TEXT NOT NULL", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
    }

}