using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Configurations;

public class PFAOnboardingRequestDistributorConfiguration : IEntityTypeConfiguration<PFAOnboardingRequestDistributor>
{
    public void Configure(EntityTypeBuilder<PFAOnboardingRequestDistributor> entity)
    {
        entity.ToTable("PFAOnboardingRequestDistributors", DatabaseSchema.Cc);
        entity.HasKey(e => e.Id);
        entity.Property(e => e.DistributorId).HasMaxLength(OnboardingConstants.DistributorIdMaxLength).IsRequired();
        entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");

        entity.HasIndex(e => new { e.RequestId, e.DistributorId }).IsUnique();
        entity.HasIndex(e => e.RequestId);

        entity.HasOne(e => e.Request)
            .WithMany(r => r.SelectedDistributors)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Distributor)
            .WithMany(d => d.OnboardingSelections)
            .HasForeignKey(e => e.DistributorId)
            .HasPrincipalKey(d => d.ContactId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
