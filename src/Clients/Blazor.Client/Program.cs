using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAdress =  builder.HostEnvironment.BaseAddress;

 // var baseAdress = "https://localhost:7023";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAdress) });


await builder.Build().RunAsync();
