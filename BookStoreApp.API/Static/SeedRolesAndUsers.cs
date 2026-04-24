using BookStoreApp.API.Data;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.API.Static
{
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
  }
}
