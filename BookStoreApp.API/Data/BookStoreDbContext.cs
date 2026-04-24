using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApiUser> //cip...29
{
  public BookStoreDbContext()
  {
  }

  public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Author> Authors { get; set; }

  public virtual DbSet<Book> Books { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
      => optionsBuilder.UseSqlServer("Server=localhost,1450;Database=BookStoreDb;User Id=sa;Password=Str0ngPa$$w0rd;TrustServerCertificate=True;MultipleActiveResultSets=true");

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder); //cip...28

    modelBuilder.Entity<Author>(entity =>
    {
      entity.Property(e => e.FirstName).HasMaxLength(60);
      entity.Property(e => e.LastName).HasMaxLength(60);
    });

    modelBuilder.Entity<Book>(entity =>
    {
      entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA553BAFCA").IsUnique();

      entity.Property(e => e.Isbn)
              .HasMaxLength(20)
              .HasColumnName("ISBN");
      entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
      entity.Property(e => e.Summary).HasMaxLength(60);
      entity.Property(e => e.Title).HasMaxLength(30);

      entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
    });

    //cip...30 moved seeding of roles/users to a separate class to avoid issues with hardcoding hashes, migration churn, and EF warnings. this seeds at runtime and not in migrations, which is a much cleaner approach.

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
