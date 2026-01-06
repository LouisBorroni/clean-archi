using TierList.Domain.Entities;

namespace TierList.Application.Ports.Services;

public interface IPdfGeneratorService
{
    Task<byte[]> GenerateTierListPdfAsync(UserTierList tierList, CancellationToken cancellationToken = default);
}
