using TierList.Domain.Enums;

namespace TierList.Application.DTOs;

public record UpdateTierListRequest(
    Dictionary<Guid, TierRank> Items
);

public record TierListItemDto(
    Guid CompanyLogoId,
    TierRank Rank
);
