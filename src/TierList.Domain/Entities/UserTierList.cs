using TierList.Domain.Common;

namespace TierList.Domain.Entities;

public class UserTierList : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public bool IsPaid { get; private set; }
    public string? PdfUrl { get; private set; }

    public ICollection<TierListItem> Items { get; private set; }

    private UserTierList() : base()
    {
        User = null!;
        Items = new List<TierListItem>();
    }

    public UserTierList(Guid userId) : base()
    {
        UserId = userId;
        User = null!;
        IsPaid = false;
        Items = new List<TierListItem>();
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
        SetUpdatedAt();
    }

    public void SetPdfUrl(string pdfUrl)
    {
        PdfUrl = pdfUrl;
        SetUpdatedAt();
    }

    public void AddItem(TierListItem item)
    {
        Items.Add(item);
        SetUpdatedAt();
    }

    public void RemoveItem(TierListItem item)
    {
        Items.Remove(item);
        SetUpdatedAt();
    }
}
