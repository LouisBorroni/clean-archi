using TierList.Domain.Enums;

namespace TierList.Application.DTOs;

public record TierListDto(
    Guid Id,
    Guid UserId,
    bool IsPaid,
    string? PdfUrl,
    List<TierListItemResponse> Items
);

public record TierListItemResponse(
    Guid Id,
    CompanyLogoDto CompanyLogo,
    TierRank Rank
);
