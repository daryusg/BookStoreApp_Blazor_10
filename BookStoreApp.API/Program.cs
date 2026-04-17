using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration)); //cip...12. Serilog configuration. We are using the Serilog.AspNetCore package, which provides the UseSerilog extension method for configuring Serilog as the logging provider for the application. The lambda expression passed to UseSerilog allows us to configure Serilog using the LoggerConfiguration object (lc) and the application configuration (ctx.Configuration). In this example, we are configuring Serilog to write logs to the console and read additional configuration settings from the application's configuration files (e.g., appsettings.json).

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
