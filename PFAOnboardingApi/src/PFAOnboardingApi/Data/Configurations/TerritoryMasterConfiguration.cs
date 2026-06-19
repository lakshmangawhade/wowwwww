using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFAOnboardingApi.Constants;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data.Configurations;

public class TerritoryMasterConfiguration : IEntityTypeConfiguration<TerritoryMaster>
{
    public void Configure(EntityTypeBuilder<TerritoryMaster> entity)
    {
        entity.ToTable("TerritoryMaster", DatabaseSchema.Cc);
        entity.HasKey(e => e.TerritoryId);
        entity.Property(e => e.TerritoryId).HasColumnName("territoryId");
        entity.Property(e => e.TerritoryName).HasMaxLength(200).IsRequired();
    }
}
