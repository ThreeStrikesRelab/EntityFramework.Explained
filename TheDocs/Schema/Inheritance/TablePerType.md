# Inheritance:`Table per type`
**If base class is made and derived classes too, table per type is generated like this:**  
```csharp
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
```
## Sql Server
Sql Server creates tables for the base AND derived classes using the ToTable() method. The tables are 1:1 related with each other with the property Id. UseTptMappingStrategy() is recommended by Microsoft but this doesn't work here.  
## Sqlite
Sqlite creates tables for the base AND derived classes (=tpt) using the ToTable() method. The tables are 1:1 related with each other with the property Id. UseTptMappingStrategy() is not compatible with Sqlite. It will only create one table of the base class.  
