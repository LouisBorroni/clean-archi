using TierList.Domain.Common;
using TierList.Domain.Enums;

namespace TierList.Domain.Entities;

public class TierListItem : BaseEntity
{
    public Guid UserTierListId { get; private set; }
    public UserTierList UserTierList { get; private set; }

    public Guid CompanyLogoId { get; private set; }
    public CompanyLogo CompanyLogo { get; private set; }

    public TierRank Rank { get; private set; }

    private TierListItem() : base()
    {
        UserTierList = null!;
        CompanyLogo = null!;
    }

    public TierListItem(Guid userTierListId, Guid companyLogoId, TierRank rank) : base()
    {
        UserTierListId = userTierListId;
        CompanyLogoId = companyLogoId;
        Rank = rank;
        UserTierList = null!;
        CompanyLogo = null!;
    }

    public void UpdateRank(TierRank newRank)
    {
        Rank = newRank;
        SetUpdatedAt();
    }
}
