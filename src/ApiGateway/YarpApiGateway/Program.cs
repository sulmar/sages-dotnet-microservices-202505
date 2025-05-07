var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();

// dotnet add package Yarp.ReverseProxy

app.MapReverseProxy();

app.MapGet("/ping", () => "Pong!");

app.Run();
