using Microsoft.EntityFrameworkCore;
using TierList.Domain.Entities;

namespace TierList.Infrastructure.Persistence;

public class TierListDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<CompanyLogo> CompanyLogos => Set<CompanyLogo>();
    public DbSet<UserTierList> UserTierLists => Set<UserTierList>();
    public DbSet<TierListItem> TierListItems => Set<TierListItem>();

    public TierListDbContext(DbContextOptions<TierListDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TierListDbContext).Assembly);
    }
}
