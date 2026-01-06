using TierList.Domain.Common;

namespace TierList.Domain.Entities;

public class CompanyLogo : BaseEntity
{
    public string CompanyName { get; private set; }
    public string Domain { get; private set; }
    public string LogoUrl { get; private set; }

    private CompanyLogo() : base()
    {
        CompanyName = string.Empty;
        Domain = string.Empty;
        LogoUrl = string.Empty;
    }

    public CompanyLogo(string companyName, string domain, string logoUrl) : base()
    {
        CompanyName = companyName;
        Domain = domain;
        LogoUrl = logoUrl;
    }

    public void UpdateLogoUrl(string newLogoUrl)
    {
        LogoUrl = newLogoUrl;
        SetUpdatedAt();
    }
}
