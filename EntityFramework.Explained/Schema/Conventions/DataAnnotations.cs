using System.ComponentModel.DataAnnotations;
using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
public class DataAnnotations
{
    public class Thing
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public int SecondInt { get; set; }
    }

    [Fact]
    [DocHeader("Sql Server - data annotations")]
    [DocContent("looking for differences when using data annotations")]
    public void SqlServer()
    {
        using var context = new TestSqlServerContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE [Items] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [SecondInt] int NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Items] PRIMARY KEY ([Id])", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("GO", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.True(reader.EndOfContent());

        //conclusion: Range(0,10) gets ignored
    }

    [Fact]
    [DocHeader("Sqlite - data annotations")]
    [DocContent("looking for differences when using data annotations")]
    public void Sqlite()
    {
        using var context = new TestSqliteContext<Thing>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE \"Items\" (", reader.NextLine());
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Items\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Equal("    \"SecondInt\" INTEGER NOT NULL", reader.NextLine());
        Assert.Equal(");", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.Equal("", reader.NextLine());
        Assert.True(reader.EndOfContent());

        //conslusion: Range(0,10) gets ignored
    }
}