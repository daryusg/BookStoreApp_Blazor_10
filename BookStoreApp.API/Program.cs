using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.Data.SqlClient; //cip...71 troubleshooting deployment connection issues


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}"); //cip...71 troubleshooting deployment connection issues
Console.WriteLine($"Connection string (before AddDbContext): {Misc.GetRedactedConnectionString(connString)}"); //cip...71 troubleshooting deployment connection issues

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(connString)); //cip...12

Console.WriteLine($"Connection string (after AddDbContext): {Misc.GetRedactedConnectionString(connString)}"); //cip...71 troubleshooting deployment connection issues

builder.Services.AddIdentityCore<ApiUser>() //cip...29
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<BookStoreDbContext>(); //cip...28

builder.Services.AddScoped<IAuthorsRepository, AuthorsRepository>(); //cip...65
builder.Services.AddScoped<IBooksRepository, BooksRepository>(); //cip...65

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

builder.Services.AddAuthentication(options => //cip...32
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
    ValidAudience = builder.Configuration["JwtSettings:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
  };
});

var app = builder.Build();

//cip...30 seeding roles, users, and user-role relationships
//IMPORTANT: chatgpt advised me to move this seeding logic to a separate class to avoid issues with the PasswordHash, ConcurrencyStamp, SecurityStamp values using tw's method. this seed users/roles at runtime and not in migrations. this completely avoids: Hardcoding hashes, Migration churn, EF warnings. i previously had to run add-migration, copy the generated hashes, and then update the migration to insert those hashes. this is a much cleaner approach.
using (var scope = app.Services.CreateScope())
{
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  await SeedRolesAndUsers.SeedAsync(userManager, roleManager);
  var services = scope.ServiceProvider;

  //seed the authors
  var context = services.GetRequiredService<BookStoreDbContext>();
  await SeedRolesAndUsers.SeedAuthorsAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger(); //cip...71 troubleshooting deployment connection issues 
app.UseSwaggerUI(); //cip...71 troubleshooting deployment connection issues 
`
app.UseHttpsRedirection();
app.UseStaticFiles(); //cip...56. used to store images locally.

app.UseCors("AllowAll"); //cip...12

app.UseAuthentication(); //cip...32
app.UseAuthorization();

app.MapControllers();

app.MapGet("/ping", () => "pong"); //cip...71 troubleshooting deployment connection issues (https://bookstoreappkevapi.azurewebsites.net/ping should return "pong")

app.Run();
