using Microsoft.EntityFrameworkCore;
using TierList.Application.Ports.Repositories;
using TierList.Domain.Entities;
using TierList.Infrastructure.Persistence;

namespace TierList.Infrastructure.Repositories;

public class CompanyLogoRepository : ICompanyLogoRepository
{
    private readonly TierListDbContext _context;

    public CompanyLogoRepository(TierListDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyLogo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos.FindAsync([id], cancellationToken);
    }

    public async Task<CompanyLogo?> GetByDomainAsync(string domain, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos
            .FirstOrDefaultAsync(cl => cl.Domain == domain, cancellationToken);
    }

    public async Task<IEnumerable<CompanyLogo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos.CountAsync(cancellationToken);
    }

    public async Task<CompanyLogo> CreateAsync(CompanyLogo logo, CancellationToken cancellationToken = default)
    {
        await _context.CompanyLogos.AddAsync(logo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return logo;
    }

    public async Task<CompanyLogo> UpdateAsync(CompanyLogo logo, CancellationToken cancellationToken = default)
    {
        _context.CompanyLogos.Update(logo);
        await _context.SaveChangesAsync(cancellationToken);
        return logo;
    }

    public async Task<bool> ExistsAsync(string domain, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyLogos.AnyAsync(cl => cl.Domain == domain, cancellationToken);
    }
}
