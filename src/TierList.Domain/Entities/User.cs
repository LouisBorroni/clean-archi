using TierList.Domain.Common;

namespace TierList.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Username { get; private set; }
    public bool IsAdmin { get; private set; }

    public ICollection<UserTierList> UserTierLists { get; private set; }

    private User() : base()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        Username = string.Empty;
        UserTierLists = new List<UserTierList>();
    }

    public User(string email, string passwordHash, string username, bool isAdmin = false) : base()
    {
        Email = email;
        PasswordHash = passwordHash;
        Username = username;
        IsAdmin = isAdmin;
        UserTierLists = new List<UserTierList>();
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SetUpdatedAt();
    }
}
