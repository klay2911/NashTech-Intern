using Microsoft.EntityFrameworkCore;

namespace EFCore2.Models;

public class StoreContext: DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product")
                .HasKey(a => a.ProductId);

            entity.Property(x => x.ProductId)
                .UseIdentityColumn(1)
                .HasColumnName("ProductId")
                .IsRequired();

            entity.Property(x => x.ProductName)
                .HasMaxLength(20);
            
            entity.HasOne(a => a.Category)
                .WithMany(b => b.Products)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired();
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category")
                .HasKey(a => a.CategoryId);

            entity.Property(x => x.CategoryId)
                .UseIdentityColumn(1)
                .HasColumnName("CategoryId")
                .IsRequired();
        });
        modelBuilder.Entity<Product>().HasData(
            new Product{ 
                ProductId = 1,ProductName = "Television", Manufacture = "Viet Nam", CategoryId = 1
            });
        modelBuilder.Entity<Category>().HasData(
            new Category{ 
                CategoryId = 1,CategoryName = "Household"
            });
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}