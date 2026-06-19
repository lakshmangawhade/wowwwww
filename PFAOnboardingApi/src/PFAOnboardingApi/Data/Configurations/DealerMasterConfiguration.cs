using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Configurations;

public class DealerMasterConfiguration : IEntityTypeConfiguration<DealerMaster>
{
    public void Configure(EntityTypeBuilder<DealerMaster> entity)
    {
        entity.ToTable("DealerMaster", DatabaseSchema.Cc);
        entity.HasKey(e => e.ContactId);
        entity.Property(e => e.ContactId).HasMaxLength(OnboardingConstants.DistributorIdMaxLength).IsRequired();
        entity.Property(e => e.RetailerShopName).HasMaxLength(300).IsRequired();
        entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

        entity.HasOne(e => e.Territory)
            .WithMany(t => t.Dealers)
            .HasForeignKey(e => e.TerritoryId)
            .HasPrincipalKey(t => t.TerritoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
