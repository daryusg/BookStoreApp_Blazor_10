using BookStoreApp.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Static;

//cip...30 seeding roles, users, and user-role relationships
//IMPORTANT: chatgpt advised me to move this seeding logic to a separate class to avoid issues with the PasswordHash, ConcurrencyStamp, SecurityStamp values using tw's method. this seed users/roles at runtime and not in migrations. this completely avoids: Hardcoding hashes, Migration churn, EF warnings. i previously had to run add-migration, copy the generated hashes, and then update the migration to insert those hashes. this is a much cleaner approach.
public static class SeedRolesAndUsers
{
  public static async Task SeedAsync(UserManager<ApiUser> userManager, RoleManager<IdentityRole> roleManager)
  {
    // Roles
    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
      await roleManager.CreateAsync(new IdentityRole("Administrator"));
    }

    if (!await roleManager.RoleExistsAsync("User"))
    {
      await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Users
    var user1 = await userManager.FindByEmailAsync("user1@bookstore.com");
    if (user1 == null)
    {
      user1 = new ApiUser
      {
        UserName = "user1@bookstore.com",
        Email = "user1@bookstore.com",
        FirstName = "User1",
        LastName = "Sys"
      };

      await userManager.CreateAsync(user1, "P@ssw0rd");
      await userManager.AddToRoleAsync(user1, "User");
    }

    var user2 = await userManager.FindByEmailAsync("user2@bookstore.com");
    if (user2 == null)
    {
      user2 = new ApiUser
      {
        UserName = "user2@bookstore.com",
        Email = "user2@bookstore.com",
        FirstName = "User2",
        LastName = "Sys"
      };

      await userManager.CreateAsync(user2, "P@ssw0rd");
      await userManager.AddToRoleAsync(user2, "User");
    }

    var admin = await userManager.FindByEmailAsync("admin1@bookstore.com");
    if (admin == null)
    {
      admin = new ApiUser
      {
        UserName = "admin1@bookstore.com",
        Email = "admin1@bookstore.com",
        FirstName = "Admin1",
        LastName = "Sys"
      };

      await userManager.CreateAsync(admin, "P@ssw0rd");
      await userManager.AddToRoleAsync(admin, "Administrator");
    }
  }

  public static async Task SeedAuthorsAsync(BookStoreDbContext context)
  {
    var iDefaultAuthorCount = 333;

    // Already seeded?
    var authorCount = await context.Authors.CountAsync();

    if (authorCount >= iDefaultAuthorCount)
    {
      return;
    }

    var firstNames = new[]
    {
      "James", "Emma", "Liam", "Olivia", "Noah",
      "Sophia", "Daniel", "Charlotte", "Lucas", "Amelia"
  };

    var lastNames = new[]
    {
      "Smith", "Johnson", "Brown", "Taylor", "Wilson",
      "Davies", "Evans", "Thomas", "Roberts", "Walker"
  };

    var bios = new[]
    {
      "Writes historical fiction.",
      "Enjoys long walks and coffee.",
      "Researches medieval literature.",
      "Loves sci-fi and fantasy worlds.",
      "Part-time novelist and teacher.",
      "Collector of rare manuscripts.",
      "Passionate about storytelling.",
      "Travels frequently for inspiration.",
      "Fan of mystery and detective novels.",
      "Works on award-winning short stories."
  };

    var random = new Random();

    for (int x = 1; x <= iDefaultAuthorCount; x++)
    {
      await context.Authors.AddAsync(new Author
      {
        FirstName = $"({x}) {firstNames[random.Next(firstNames.Length)]}",
        LastName = $"({x}) {lastNames[random.Next(lastNames.Length)]}",
        Bio = $"({x}) {bios[random.Next(bios.Length)]}"
      });
    }

    await context.SaveChangesAsync();
  }
}