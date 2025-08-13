using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema.SqlGeneration;

[DocFile]
public class ValueObjectProperties
{
    public class CustomerName
    {
        public string FullName { get; private set; }
        private CustomerName() { }
        public CustomerName(string fullname)
        {
            FullName = fullname;
        }
    }

    // Value Object: Address
    public class Customer
    {
        public int Id { get; set; }

        public CustomerName Name { get; set; }

        public Customer() { }

        public Customer(int id, CustomerName name)
        {
            Id = id;
            Name = name;
        }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("DoesNotMatter"); // Required by EF, never actually used
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(customer =>
            {
                customer.HasKey(o => o.Id);

                customer.OwnsOne(o => o.Name, cn =>
                {
                    cn.Property(c => c.FullName).HasColumnName("CustomerFullName").IsRequired();
                });
            });
        }
    }


    [Fact]
    [DocHeader("Value Objects")]
    [DocContent("checking what SQL is generated for value objects in model")]
    public void VOChecker()
    {
        using var context = new AppDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("CREATE TABLE [Customers] (", reader.NextLine());
        Assert.Equal("    [Id] int NOT NULL IDENTITY,", reader.NextLine());
        Assert.Equal("    [CustomerFullName] nvarchar(max) NOT NULL,", reader.NextLine());
        Assert.Equal("    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])", reader.NextLine());
    }
}