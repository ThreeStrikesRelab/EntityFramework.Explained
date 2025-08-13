using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Relationships;

[DocFile]
[DocContent("Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.")]
public class BiDirectionalOneToMany
{
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // Foreign key
        public Blog Blog { get; set; } = default!;
    }

    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }

    [Fact]
    [DocHeader("Sql Server - bidirectional")]
    [DocContent("EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`.")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Post>();

        var sql = context.Database.GenerateCreateScript();

        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE [Blog] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [Name] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Blog] PRIMARY KEY ([Id])", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE TABLE [Items] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [BlogId] int NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Items] PRIMARY KEY ([Id]),", reader.NextLine());
        Assert.Equal("    CONSTRAINT [FK_Items_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blog] ([Id]) ON DELETE CASCADE", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE INDEX [IX_Items_BlogId] ON [Items] ([BlogId]);", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
    }

    [Fact]
    [DocHeader("Sql Server - bidirectional - reversed")]
    [DocContent("EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`, but reversed this time. CREATE TABLE switches to different DBSets to create tables from.")]
    public void SqlServerreverse()
    {
        using var context = new TestSqlServerContext<Blog>();

        var sql = context.Database.GenerateCreateScript();

        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE [Items] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [Name] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Items] PRIMARY KEY ([Id])", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE TABLE [Post] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [BlogId] int NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Post] PRIMARY KEY ([Id]),", reader.NextLine());
        Assert.Equal("    CONSTRAINT [FK_Post_Items_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Items] ([Id]) ON DELETE CASCADE", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE INDEX [IX_Post_BlogId] ON [Post] ([BlogId]);", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
    }

    [Fact]
    [DocHeader("Sqlite - bidirectional")]
    [DocContent("Same behavior as SqlServer with bidirectional relationship, relationship discovery pulls in the `Blog` entity despite only registering `Post`")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Post>();

        var sql = context.Database.GenerateCreateScript();

        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE \"Blog\" (", reader.NextLine());
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Blog\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"Name\" TEXT NOT NULL", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE TABLE \"Items\" (", reader.NextLine());
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Items\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"BlogId\" INTEGER NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT \"FK_Items_Blog_BlogId\" FOREIGN KEY (\"BlogId\") REFERENCES \"Blog\" (\"Id\") ON DELETE CASCADE", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        reader.Skip(2);
        Assert.Equal("CREATE INDEX \"IX_Items_BlogId\" ON \"Items\" (\"BlogId\");", reader.NextLine());
    }

}