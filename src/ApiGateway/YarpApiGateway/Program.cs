

var builder = WebApplication.CreateBuilder(args);

// dotnet add package Microsoft.Extensions.ServiceDiscovery
// dotnet add package Microsoft.Extensions.ServiceDiscovery.Yarp

// Dodaj odnajdywanie us³ug
builder.Services.AddServiceDiscovery();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver(); // Dodaj YARP z odnajdywaniem us³ug

var app = builder.Build();

// dotnet add package Yarp.ReverseProxy

app.MapReverseProxy();

app.MapGet("/ping", () => "Pong!");

app.Run();
