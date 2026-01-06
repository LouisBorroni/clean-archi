using TierList.Application.DTOs;
using TierList.Application.Ports.Repositories;
using TierList.Application.Ports.Services;
using TierList.Domain.Entities;

namespace TierList.Application.UseCases;

public class AddCompanyLogoUseCase
{
    private readonly ICompanyLogoRepository _logoRepository;
    private readonly ILogoApiService _logoApiService;
    private const int MaxLogosCount = 10;

    public AddCompanyLogoUseCase(
        ICompanyLogoRepository logoRepository,
        ILogoApiService logoApiService)
    {
        _logoRepository = logoRepository;
        _logoApiService = logoApiService;
    }

    public async Task<CompanyLogoDto> ExecuteAsync(AddCompanyLogoRequest request, CancellationToken cancellationToken = default)
    {
        var currentCount = await _logoRepository.CountAsync(cancellationToken);
        if (currentCount >= MaxLogosCount)
        {
            throw new InvalidOperationException($"Maximum number of logos ({MaxLogosCount}) already reached");
        }

        var exists = await _logoRepository.ExistsAsync(request.Domain, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException($"Logo for domain '{request.Domain}' already exists");
        }

        var logoUrl = await _logoApiService.GetLogoUrlAsync(request.Domain, 400, cancellationToken);

        var logo = new CompanyLogo(request.CompanyName, request.Domain, logoUrl);
        await _logoRepository.CreateAsync(logo, cancellationToken);

        return new CompanyLogoDto(logo.Id, logo.CompanyName, logo.Domain, logo.LogoUrl);
    }
}
