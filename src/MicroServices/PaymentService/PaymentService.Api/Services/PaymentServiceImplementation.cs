using Grpc.Core;
using PaymentService.Grpc;

namespace PaymentService.Api.Services;

// {csharp_namespace}.{service}.{service}Base
public class PaymentServiceImplementation : PaymentService.Grpc.PaymentService.PaymentServiceBase
{
    public override Task<PaymentResponse> Process(PaymentRequest request, ServerCallContext context)
    {
        var status = request.Amount < 1000 ? PaymentStatus.Accepted : PaymentStatus.Declined;

        var response = new PaymentResponse
        {
            Status = status,
            Reason = status == PaymentStatus.Declined ? "Limit exceeded" : string.Empty
        };

        return Task.FromResult(response);
    }

    public override async Task ProcessStream(PaymentRequest request, IServerStreamWriter<PaymentStage> responseStream, ServerCallContext context)
    {

        await responseStream.WriteAsync(new PaymentStage
        {
            Stage = "Initialized",
            Description = "Payment initialized"
        });

        await Task.Delay(3000);

        await responseStream.WriteAsync(new PaymentStage
        {
            Stage = "Processing",
            Description = "Payment processing"
        });

        await Task.Delay(Random.Shared.Next(5000, 10000));

        var status = request.Amount < 1000 ? PaymentStatus.Accepted : PaymentStatus.Declined;

        await responseStream.WriteAsync(new PaymentStage
        {
            Stage = status.ToString(),
            Description = status == PaymentStatus.Declined ? "Limit exceeded" : "Payment accepted"
        });


    }

}
