using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Explained._Tools.TestContexts;

public class TestSqliteContext<T> : DbContext where T : class
{
    public DbSet<T> Items => Set<T>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DoesNotMatter"); // Required by EF, never actually used

}