using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da", "3399449d-6185-4d98-bb99-77f443600803", "Administrator", "ADMINISTRATOR" },
                    { "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", "8f264ef3-4548-4f23-88f0-3ddd88ffce4a", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "44df811f-f198-4e8f-abdf-937a878695a5", 0, "9c023e80-81c0-406c-a568-9e9c0f402801", "admin1@bookstore.com", false, "Admin1", "Base", false, null, "ADMIN1@BOOKSTORE.COM", "ADMIN1@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEAT9g+f3RAkxEsIOu9jxMqvEdoGBwDi501moRzJHkSqWN0abu3FAS47Ul2fAlLwB6w==", null, false, "2bb119ea-5f74-4ccf-8ae5-18b3365d728c", false, "admin1@bookstore.com" },
                    { "635900df-1c6a-44b1-99ec-b17b3ce50f09", 0, "34ba2f93-e22a-4fa1-b9b7-2964d1b1a315", "user2@bookstore.com", false, "User2", "Base", false, null, "USER2@BOOKSTORE.COM", "USER2@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEKN7UXJ9C6e0wdt5kg5nvKjKGyyj9l1CmybyGyK3ELZIFJz1h/0wCHAnasQbk6rPGA==", null, false, "c4e7cd4a-a9ae-4cd8-9b63-0655937efe4b", false, "user2@bookstore.com" },
                    { "80694358-624b-4319-ae5e-a1b274898944", 0, "3962354b-f9fd-4527-905f-23d391098eb8", "user1@bookstore.com", false, "User1", "Base", false, null, "USER1@BOOKSTORE.COM", "USER1@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEDqWaST4ylDTsil73/Y3hi1KQ3IAxm0HX9KNfxd06DEZpe3Za363Kz5vYORwsXZTBQ==", null, false, "3f535804-8dcc-4995-a304-817d7925fd65", false, "user1@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da", "44df811f-f198-4e8f-abdf-937a878695a5" },
                    { "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", "635900df-1c6a-44b1-99ec-b17b3ce50f09" },
                    { "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", "80694358-624b-4319-ae5e-a1b274898944" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da", "44df811f-f198-4e8f-abdf-937a878695a5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", "635900df-1c6a-44b1-99ec-b17b3ce50f09" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9", "80694358-624b-4319-ae5e-a1b274898944" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b85bba7-cb77-4a37-81ab-bfcdd6efe4da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cddf5286-1e11-49cb-8e8d-b3edb0cbc0c9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44df811f-f198-4e8f-abdf-937a878695a5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "635900df-1c6a-44b1-99ec-b17b3ce50f09");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80694358-624b-4319-ae5e-a1b274898944");
        }
    }
}
