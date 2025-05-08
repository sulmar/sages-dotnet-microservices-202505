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
}
