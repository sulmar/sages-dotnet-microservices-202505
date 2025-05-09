using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// dotnet add package AspNetCore.HealthChecks.UI
// dotnet add package AspNetCore.HealthChecks.UI.Client
// dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage 
// dotnet add package AspNetCore.HealthChecks.UI.SQLite.Storage

builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("ProductCatalog", "https://localhost:7291/health");
}).AddSqliteStorage("Data Source=healthchecks.db;Pooling=true;Cache=Shared;");

//.AddInMemoryStorage();

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();

// app.MapGet("/monitoring", () => "Hello Monitoring.Api!");

// default is /healthchecks-ui/
//app.MapHealthChecksUI();

app.MapHealthChecksUI(options => options.UIPath = "/monitoring");


app.Run();
