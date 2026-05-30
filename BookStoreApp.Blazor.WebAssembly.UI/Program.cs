//cip...62 chatgpt (BC55+) assisted rehash of the BookStoreApp.Blazor.Web.UI.Program.cs file.
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI;
using BookStoreApp.Blazor.WebAssembly.UI.Configurations;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using BookStoreApp.Blazor.WebAssembly.UI.Services;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ----------------------------------------------------------
// HTTP CLIENT
// ----------------------------------------------------------
builder.Services.AddScoped(sp => new HttpClient
{
  BaseAddress = new Uri("https://localhost:7244")
});

// ----------------------------------------------------------
// LOCAL STORAGE
// ----------------------------------------------------------
builder.Services.AddBlazoredLocalStorage();
// ----------------------------------------------------------
// API CLIENT SERVICES
// ----------------------------------------------------------
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IClient, Client>();

// ----------------------------------------------------------
// AUTOMAPPER
// ----------------------------------------------------------
builder.Services.AddAutoMapper(cfg =>
{
  cfg.AddProfile<MapperConfig>();
}, typeof(Program));

// ----------------------------------------------------------
// AUTHENTICATION / AUTHORIZATION
// ----------------------------------------------------------
builder.Services.AddScoped<ApiAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    provider => provider.GetRequiredService<ApiAuthenticationStateProvider>()
);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// ----------------------------------------------------------

await builder.Build().RunAsync();