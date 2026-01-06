using TierList.Domain.Entities;

namespace TierList.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(TierListDbContext context)
    {
        // Si des logos existent déjà, ne rien faire
        if (context.CompanyLogos.Any())
        {
            return;
        }

        var logos = new List<CompanyLogo>
        {
            new CompanyLogo("Google", "google.com", "https://logo.clearbit.com/google.com"),
            new CompanyLogo("Microsoft", "microsoft.com", "https://logo.clearbit.com/microsoft.com"),
            new CompanyLogo("Apple", "apple.com", "https://logo.clearbit.com/apple.com"),
            new CompanyLogo("Amazon", "amazon.com", "https://logo.clearbit.com/amazon.com"),
            new CompanyLogo("Meta (Facebook)", "meta.com", "https://logo.clearbit.com/meta.com"),
            new CompanyLogo("Netflix", "netflix.com", "https://logo.clearbit.com/netflix.com"),
            new CompanyLogo("Twitter (X)", "twitter.com", "https://logo.clearbit.com/twitter.com"),
            new CompanyLogo("Spotify", "spotify.com", "https://logo.clearbit.com/spotify.com"),
            new CompanyLogo("Adobe", "adobe.com", "https://logo.clearbit.com/adobe.com"),
            new CompanyLogo("Nike", "nike.com", "https://logo.clearbit.com/nike.com")
        };

        await context.CompanyLogos.AddRangeAsync(logos);
        await context.SaveChangesAsync();
    }
}
