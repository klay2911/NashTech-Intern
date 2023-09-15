using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Models;

public class LibraryContext: DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book")
                .HasKey(a => a.BookId);

            entity.Property(a => a.BookId)
                .UseIdentityColumn(1)
                .HasColumnName("ProductId")
                .IsRequired();

            entity.Property(a => a.Title)
                .HasMaxLength(20);

            entity.HasOne(a => a.Category)
                .WithMany(b => b.Books)
                .HasForeignKey(a => a.CategoryId)
                .IsRequired();

            entity.HasMany(a => a.BookBorrowingRequestDetails)
                .WithMany(d => d.Books);
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category").HasKey(b => b.CategoryId);

            entity.Property(b => b.CategoryId)
                .UseIdentityColumn(1)
                .HasColumnName("CategoryId")
                .IsRequired();

            entity.Property(b => b.CategoryName)
                .HasMaxLength(20);
        });
        modelBuilder.Entity<BookBorrowingRequest>(entity =>
        {
            entity.ToTable("BookBorrowingRequest").HasKey(c => c.RequestId);

            entity.Property(c => c.RequestId)
                .UseIdentityColumn(1)
                .HasColumnName("RequestId")
                .IsRequired();

            entity.HasOne(c => c.User)
                .WithMany(e => e.BookBorrowingRequests)
                .HasForeignKey(c => c.UserId);

            entity.HasMany(c => c.BookBorrowingRequestDetails)
                .WithOne(d => d.BookBorrowingRequest)
                .HasForeignKey(c => c.RequestId);
        });
        modelBuilder.Entity<BookBorrowingRequestDetails>(entity =>
        {
            entity.ToTable("BookBorrowingRequestDetails").HasKey(d => d.RequestDetailsId);

            entity.Property(d => d.RequestDetailsId)
                .UseIdentityColumn(1)
                .HasColumnName("RequestDetailsId")
                .IsRequired();
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User").HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .UseIdentityColumn(1)
                .HasColumnName("UserId")
                .IsRequired();
            entity.HasData(
                new User
                {
                    UserId = 1, FirstName = "La ", LastName = "Vu", Email = "thienvu2911@gmail.com",
                    UserName = "vu2911", Password = "29112002", Role = "SuperUser", Dob = new DateTime(2002, 11, 29)
                },
                new User
                {
                    UserId = 2, FirstName = "Do ", LastName = "Duc", Email = "tuanduc@gmail.com",
                    UserName = "duc0605", Password = "06052002", Role = "NormalUser", Dob = new DateTime(2002, 05, 06)
                });
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookBorrowingRequest> BorrowingRequests { get; set; }
    public DbSet<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }

}