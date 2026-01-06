namespace TierList.Application.Ports.Services;

public interface ILogoApiService
{
    Task<string> GetLogoUrlAsync(string domain, int size = 400, CancellationToken cancellationToken = default);
    Task<bool> ValidateDomainAsync(string domain, CancellationToken cancellationToken = default);
}
