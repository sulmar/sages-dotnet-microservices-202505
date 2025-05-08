using PaymentService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGet("/", () => "Hello PaymentService!");

app.MapGrpcService<PaymentServiceImplementation>();

app.Run();
