namespace TierList.Application.Ports.Services;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
    Task<bool> ValidatePaymentAsync(string paymentIntentId, CancellationToken cancellationToken = default);
}
