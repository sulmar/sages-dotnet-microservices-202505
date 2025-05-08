using Grpc.Core;
using Grpc.Net.Client;
using PaymentService.Grpc;
using System.Security.Cryptography.Xml;
using static PaymentService.Grpc.PaymentService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Ordering.Api!");

app.MapGet("/api/orders/count", () => Results.Ok(100));

app.MapPost("/api/orders", async (OrderItem[] reqest) =>
{
    var orderId = "ZAM1/2025";
    var paymentMethod = "blik";
    var totalAmount = 100;

    // TODO: Wywo³aj proces p³atnoœci

    var channel = GrpcChannel.ForAddress("https://localhost:7044");

    var paymentClient = new PaymentServiceClient(channel);

    var call = paymentClient.ProcessStream(new PaymentRequest
    {
        Amount = totalAmount,
        PaymentMethod = paymentMethod,
        OrderId = orderId
    });

    await foreach(var stage in call.ResponseStream.ReadAllAsync<PaymentStage>())
    {
        Console.WriteLine($"Stage: {stage.Stage}, Description: {stage.Description}");
    }    

    var response = await paymentClient.ProcessAsync(new PaymentRequest
    {
        Amount = totalAmount,
        PaymentMethod = paymentMethod,
        OrderId = orderId
    });

    if (response.Status == PaymentStatus.Accepted)
    {
        // Simulate creating an order
        return Results.Created("/api/orders/1", new { OrderId = 1, Status = "Created" });
    }
    else
    {
        return Results.BadRequest(new { Error = response.Reason });
    }


});

app.Run();


public record OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}
