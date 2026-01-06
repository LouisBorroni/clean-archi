using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierList.Domain.Entities;

namespace TierList.Infrastructure.Persistence.Configurations;

public class TierListItemConfiguration : IEntityTypeConfiguration<TierListItem>
{
    public void Configure(EntityTypeBuilder<TierListItem> builder)
    {
        builder.ToTable("TierListItems");

        builder.HasKey(tli => tli.Id);

        builder.Property(tli => tli.UserTierListId)
            .IsRequired();

        builder.Property(tli => tli.CompanyLogoId)
            .IsRequired();

        builder.Property(tli => tli.Rank)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(tli => tli.CreatedAt)
            .IsRequired();

        builder.Property(tli => tli.UpdatedAt);

        builder.HasOne(tli => tli.UserTierList)
            .WithMany(utl => utl.Items)
            .HasForeignKey(tli => tli.UserTierListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tli => tli.CompanyLogo)
            .WithMany()
            .HasForeignKey(tli => tli.CompanyLogoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
