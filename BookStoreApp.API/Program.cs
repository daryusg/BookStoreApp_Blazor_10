using AutoMapper;
using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
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
