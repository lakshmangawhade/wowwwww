using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Configurations;

public class PFAOnboardingRequestConfiguration : IEntityTypeConfiguration<PFAOnboardingRequest>
{
    public void Configure(EntityTypeBuilder<PFAOnboardingRequest> entity)
    {
        entity.ToTable("PFAOnboardingRequests", DatabaseSchema.Cc);
        entity.HasKey(e => e.RequestId);
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Mobile).HasMaxLength(OnboardingConstants.MobileMaxLength).IsRequired();
        entity.Property(e => e.EmailId).HasMaxLength(256).IsRequired();
        entity.Property(e => e.PanNo).HasMaxLength(10).IsRequired();
        entity.Property(e => e.AadhaarNumber).HasMaxLength(12).IsRequired();
        entity.Property(e => e.UanNumber).HasMaxLength(12);
        entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");

        entity.HasIndex(e => e.Mobile);
        entity.HasIndex(e => e.TerritoryId);

        entity.HasOne(e => e.Territory)
            .WithMany(t => t.OnboardingRequests)
            .HasForeignKey(e => e.TerritoryId)
            .HasPrincipalKey(t => t.TerritoryId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.LinkedUser)
            .WithMany()
            .HasForeignKey(e => e.UserDetailsId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
