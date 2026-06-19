using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Configurations;

public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
{
    public void Configure(EntityTypeBuilder<UserDetails> entity)
    {
        entity.ToTable("UserDetails", DatabaseSchema.Cc);
        entity.HasKey(e => e.UserId);
        entity.Property(e => e.Mobile).HasMaxLength(OnboardingConstants.MobileMaxLength).IsRequired();
        entity.Property(e => e.FirstName).HasColumnName("FirstName").HasMaxLength(200);
        entity.Property(e => e.EmailId).HasMaxLength(256);
        entity.Property(e => e.Active).HasColumnName("Active");

        entity.HasIndex(e => e.Mobile);
    }
}
