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
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.EmailId).HasMaxLength(256);
        entity.Property(e => e.PanNo).HasMaxLength(10);
        entity.Property(e => e.AadhaarNumber).HasMaxLength(12);
        entity.Property(e => e.UanNumber).HasMaxLength(12);

        entity.HasIndex(e => e.Mobile);
    }
}
