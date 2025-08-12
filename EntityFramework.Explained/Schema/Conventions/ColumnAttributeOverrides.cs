using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Explained._Tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.Conventions;

[DocFile]
public class ColumnAttributesOverrides
{
    public class Student
    {
        [Column("Id", Order = 0)]
        public int StudentId { get; set; } = default!;

        [Column("Age", Order = 2)]
        public int StudentAge { get; set; }

        [Column("Name", Order = 1)]
        public string StudentName { get; set; }

        [Column("Birthdate", Order = 3)]
        public DateTime StudentBirthdate { get; set; }

    }

    [Fact]
    [DocHeader("Sql Server")]
    [DocContent("Generates table with chosen column names of the right type in the given order.")]
    public void SqlServerFollowsOrder_ChangesColumnName()
    {
        using var context = new TestSqlServerContext<Student>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE [Items] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [Name] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    [Age] int NOT NULL,", reader.NextLine());
        Assert.Equal("    [Birthdate] datetime2 NOT NULL,", reader.NextLine());

    }

    [Fact]
    [DocHeader("Sqlite")]
    [DocContent("Generates table with chosen column names of the right type (look at date :O) in the given order.")]
    public void SqliteFollowsOrder_ChangesColumnName_AndHasType()
    {
        using var context = new TestSqliteContext<Student>();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);

        Assert.Equal("CREATE TABLE \"Items\" (", reader.NextLine());
        Assert.Equal("    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Items\" PRIMARY KEY AUTOINCREMENT,", reader.NextLine());
        Assert.Contains("\"Name\" TEXT NOT NULL", reader.NextLine());
        Assert.Contains("\"Age\" INTEGER NOT NULL", reader.NextLine());
        Assert.Contains("\"Birthdate\" TEXT NOT NULL", reader.NextLine());
    }
}