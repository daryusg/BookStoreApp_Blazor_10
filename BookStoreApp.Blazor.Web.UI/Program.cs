using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Components;
using BookStoreApp.Blazor.Web.UI.Providers;
using BookStoreApp.Blazor.Web.UI.Services.Authentication;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage(); //cip...40
builder.Services.AddHttpClient<IClient, Client>(client =>
{
  client.BaseAddress = new Uri("https://localhost:7244");
}); //cip...38
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); //cip...40
builder.Services.AddScoped<AuthenticationStateProvider>(
    provider => provider.GetRequiredService<ApiAuthenticationStateProvider>()
  ); //cip...40

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
