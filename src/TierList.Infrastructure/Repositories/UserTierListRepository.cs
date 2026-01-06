using Microsoft.EntityFrameworkCore;
using TierList.Application.Ports.Repositories;
using TierList.Domain.Entities;
using TierList.Infrastructure.Persistence;

namespace TierList.Infrastructure.Repositories;

public class UserTierListRepository : IUserTierListRepository
{
    private readonly TierListDbContext _context;

    public UserTierListRepository(TierListDbContext context)
    {
        _context = context;
    }

    public async Task<UserTierList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserTierLists.FindAsync([id], cancellationToken);
    }

    public async Task<UserTierList?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserTierLists
            .FirstOrDefaultAsync(utl => utl.UserId == userId, cancellationToken);
    }

    public async Task<UserTierList?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserTierLists
            .Include(utl => utl.Items)
                .ThenInclude(item => item.CompanyLogo)
            .FirstOrDefaultAsync(utl => utl.UserId == userId, cancellationToken);
    }

    public async Task<UserTierList> CreateAsync(UserTierList tierList, CancellationToken cancellationToken = default)
    {
        await _context.UserTierLists.AddAsync(tierList, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return tierList;
    }

    public async Task<UserTierList> UpdateAsync(UserTierList tierList, CancellationToken cancellationToken = default)
    {
        _context.UserTierLists.Update(tierList);
        await _context.SaveChangesAsync(cancellationToken);
        return tierList;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tierList = await GetByIdAsync(id, cancellationToken);
        if (tierList != null)
        {
            _context.UserTierLists.Remove(tierList);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
