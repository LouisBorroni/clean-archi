using TierList.Application.DTOs;
using TierList.Application.Ports.Repositories;

namespace TierList.Application.UseCases;

public class GetAllLogosUseCase
{
    private readonly ICompanyLogoRepository _logoRepository;

    public GetAllLogosUseCase(ICompanyLogoRepository logoRepository)
    {
        _logoRepository = logoRepository;
    }

    public async Task<IEnumerable<CompanyLogoDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var logos = await _logoRepository.GetAllAsync(cancellationToken);
        return logos.Select(logo => new CompanyLogoDto(logo.Id, logo.CompanyName, logo.Domain, logo.LogoUrl));
    }
}
