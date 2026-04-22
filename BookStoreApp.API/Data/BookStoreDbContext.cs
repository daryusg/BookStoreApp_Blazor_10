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

    //cip...30 seeding roles, users, and user-role relationships
    modelBuilder.Entity<IdentityRole>().HasData( //cip...30
      new IdentityRole
      {
        Name = "User",
        NormalizedName = "USER",
        Id = "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9",
        ConcurrencyStamp = "8f264ef3-4548-4f23-88f0-3ddd88ffce4a" //.net9+ issue. copied from migration.
      },
      new IdentityRole
      {
        Name = "Administrator",
        NormalizedName = "ADMINISTRATOR",
        Id = "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da",
        ConcurrencyStamp = "3399449d-6185-4d98-bb99-77f443600803" //.net9+ issue. copied from migration.
      }
    );

    var hasher = new PasswordHasher<ApiUser>(); //cip...30
    // Seed initial users
    modelBuilder.Entity<ApiUser>().HasData(
      new ApiUser
      {
        Id = "80694358-624b-4319-ae5e-a1b274898944",
        Email = "user1@bookstore.com",
        NormalizedEmail = "USER1@BOOKSTORE.COM",
        UserName = "user1@bookstore.com",
        NormalizedUserName = "USER1@BOOKSTORE.COM",
        FirstName = "User1",
        LastName = "Base",
        PasswordHash = "AQAAAAIAAYagAAAAEDqWaST4ylDTsil73/Y3hi1KQ3IAxm0HX9KNfxd06DEZpe3Za363Kz5vYORwsXZTBQ==", /*.net9+ issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
        ConcurrencyStamp = "3962354b-f9fd-4527-905f-23d391098eb8", /*.net9+ issue. copied from migration*/
        SecurityStamp = "3f535804-8dcc-4995-a304-817d7925fd65" /*.net9+ issue. copied from migration*/
      },
      new ApiUser
      {
        Id = "635900df-1c6a-44b1-99ec-b17b3ce50f09",
        Email = "user2@bookstore.com",
        NormalizedEmail = "USER2@BOOKSTORE.COM",
        UserName = "user2@bookstore.com",
        NormalizedUserName = "USER2@BOOKSTORE.COM",
        FirstName = "User2",
        LastName = "Base",
        PasswordHash = "AQAAAAIAAYagAAAAEKN7UXJ9C6e0wdt5kg5nvKjKGyyj9l1CmybyGyK3ELZIFJz1h/0wCHAnasQbk6rPGA==", /*.net9+ issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
        ConcurrencyStamp = "34ba2f93-e22a-4fa1-b9b7-2964d1b1a315", /*.net9+ issue. copied from migration*/
        SecurityStamp = "c4e7cd4a-a9ae-4cd8-9b63-0655937efe4b" /*.net9+ issue. copied from migration*/
      }
      ,
      new ApiUser
      {
        Id = "44df811f-f198-4e8f-abdf-937a878695a5",
        Email = "admin1@bookstore.com",
        NormalizedEmail = "ADMIN1@BOOKSTORE.COM",
        UserName = "admin1@bookstore.com",
        NormalizedUserName = "ADMIN1@BOOKSTORE.COM",
        FirstName = "Admin1",
        LastName = "Base",
        PasswordHash = "AQAAAAIAAYagAAAAEAT9g+f3RAkxEsIOu9jxMqvEdoGBwDi501moRzJHkSqWN0abu3FAS47Ul2fAlLwB6w==", /*.net9+ issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
        ConcurrencyStamp = "9c023e80-81c0-406c-a568-9e9c0f402801", /*.net9+ issue. copied from migration*/
        SecurityStamp = "2bb119ea-5f74-4ccf-8ae5-18b3365d728c" /*.net9+ issue. copied from migration*/
      }
    );

    modelBuilder.Entity<IdentityUserRole<string>>().HasData( //cip...30
      new IdentityUserRole<string>
      {
        RoleId = "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", // User role
        UserId = "80694358-624b-4319-ae5e-a1b274898944" // user1
      },
      new IdentityUserRole<string>
      {
        RoleId = "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", // User role
        UserId = "635900df-1c6a-44b1-99ec-b17b3ce50f09" // user2
      },
      new IdentityUserRole<string>
      {
        RoleId = "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da", // Administrator role
        UserId = "44df811f-f198-4e8f-abdf-937a878695a5" // admin1
      }
    );

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
