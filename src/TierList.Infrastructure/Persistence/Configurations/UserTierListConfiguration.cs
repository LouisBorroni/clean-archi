using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierList.Domain.Entities;

namespace TierList.Infrastructure.Persistence.Configurations;

public class UserTierListConfiguration : IEntityTypeConfiguration<UserTierList>
{
    public void Configure(EntityTypeBuilder<UserTierList> builder)
    {
        builder.ToTable("UserTierLists");

        builder.HasKey(utl => utl.Id);

        builder.Property(utl => utl.UserId)
            .IsRequired();

        builder.Property(utl => utl.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(utl => utl.PdfUrl)
            .HasMaxLength(500);

        builder.Property(utl => utl.CreatedAt)
            .IsRequired();

        builder.Property(utl => utl.UpdatedAt);

        builder.HasOne(utl => utl.User)
            .WithMany(u => u.UserTierLists)
            .HasForeignKey(utl => utl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(utl => utl.Items)
            .WithOne(tli => tli.UserTierList)
            .HasForeignKey(tli => tli.UserTierListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
