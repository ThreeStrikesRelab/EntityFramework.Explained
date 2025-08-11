using System.ComponentModel.DataAnnotations;
using EntityFramework.Explained._Tools.Helpers;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
[DocFileHeader("Data Annotations: `[Range(...)]`")]
public class DataAnnotations
{
    [DocExample]
    public class Thing
    {
        public int Id { get; set; }
        [Range(0, 10)] // <= We are checking this 
        public int SecondInt { get; set; }
    }
    
    private LinesReader GetReader(DbContext context) 
    {
        using var context = context;
        var sql = context.Database.GenerateCreateScript();
        return LinesReader.FromText(sql);
    }
    
    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("`[Range(0,10)]` gets ignored")]
    public void SqlServer()
    {
        var result = GetReader(new TestSqlServerContext<Thing>()).SkipToLineContaining("SecondInt");
        Assert.Equal("    [SecondInt] int NOT NULL,", result);
    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Same behaviour as Sql Server.")]
    public void Sqlite()
    {
        var result = GetReader(new TestSqliteContext<Thing>()).SkipToLineContaining("SecondInt");
        Assert.Equal("    \"SecondInt\" INTEGER NOT NULL", result);
    }
}
