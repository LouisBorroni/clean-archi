using TierList.Domain.Entities;

namespace TierList.Application.Ports.Repositories;

public interface IUserTierListRepository
{
    Task<UserTierList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserTierList?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserTierList?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserTierList> CreateAsync(UserTierList tierList, CancellationToken cancellationToken = default);
    Task<UserTierList> UpdateAsync(UserTierList tierList, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
