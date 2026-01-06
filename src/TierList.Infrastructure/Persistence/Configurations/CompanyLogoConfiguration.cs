using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierList.Domain.Entities;

namespace TierList.Infrastructure.Persistence.Configurations;

public class CompanyLogoConfiguration : IEntityTypeConfiguration<CompanyLogo>
{
    public void Configure(EntityTypeBuilder<CompanyLogo> builder)
    {
        builder.ToTable("CompanyLogos");

        builder.HasKey(cl => cl.Id);

        builder.Property(cl => cl.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cl => cl.Domain)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(cl => cl.Domain)
            .IsUnique();

        builder.Property(cl => cl.LogoUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cl => cl.CreatedAt)
            .IsRequired();

        builder.Property(cl => cl.UpdatedAt);
    }
}
