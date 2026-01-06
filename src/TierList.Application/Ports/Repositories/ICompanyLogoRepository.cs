using TierList.Domain.Entities;

namespace TierList.Application.Ports.Repositories;

public interface ICompanyLogoRepository
{
    Task<CompanyLogo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CompanyLogo?> GetByDomainAsync(string domain, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyLogo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<CompanyLogo> CreateAsync(CompanyLogo logo, CancellationToken cancellationToken = default);
    Task<CompanyLogo> UpdateAsync(CompanyLogo logo, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string domain, CancellationToken cancellationToken = default);
}
