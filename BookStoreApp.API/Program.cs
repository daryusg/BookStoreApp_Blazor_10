using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(connString)); //cip...12
builder.Services.AddIdentityCore<ApiUser>() //cip...29
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<BookStoreDbContext>(); //cip...28

//builder.Services.AddAutoMapper(typeof(MapperConfig)); //deprecated in favor of the following line, which is more concise and achieves the same result.
builder.Services.AddAutoMapper(cfg => //cip...12 + chatgpt
{
  cfg.AddProfile<MapperConfig>();
}, typeof(Program));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration)); //cip...12

//cip...12. esssentially, this is the ootb (default) policy.
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

//cip...30 seeding roles, users, and user-role relationships
//IMPORTANT: chatgpt advised me to move this seeding logic to a separate class to avoid issues with the PasswordHash, ConcurrencyStamp, SecurityStamp values using tw's method. this seed users/roles at runtime and not in migrations. this completely avoids: Hardcoding hashes, Migration churn, EF warnings. i previously had to run add-migration, copy the generated hashes, and then update the migration to insert those hashes. this is a much cleaner approach.
using (var scope = app.Services.CreateScope())
{
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  await SeedRolesAndUsers.SeedAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); //cip...12

app.UseAuthorization();

app.MapControllers();

app.Run();
