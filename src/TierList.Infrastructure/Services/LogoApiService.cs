using TierList.Application.Ports.Services;

namespace TierList.Infrastructure.Services;

public class LogoApiService : ILogoApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public LogoApiService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public Task<string> GetLogoUrlAsync(string domain, int size = 400, CancellationToken cancellationToken = default)
    {
        var url = $"https://img.logo.dev/{domain}?token={_apiKey}&size={size}&retina=true";
        return Task.FromResult(url);
    }

    public async Task<bool> ValidateDomainAsync(string domain, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = GetLogoUrlAsync(domain, 100, cancellationToken);
            var response = await _httpClient.GetAsync(await url, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
