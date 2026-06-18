using Microsoft.EntityFrameworkCore;
using PFAOnboardingApi.Entities;

namespace PFAOnboardingApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TerritoryMaster> TerritoryMaster => Set<TerritoryMaster>();
    public DbSet<DealerMaster> DealerMaster => Set<DealerMaster>();
    public DbSet<UserDetails> UserDetails => Set<UserDetails>();
    public DbSet<PFAOnboardingRequest> PFAOnboardingRequests => Set<PFAOnboardingRequest>();
    public DbSet<PFAOnboardingRequestDistributor> PFAOnboardingRequestDistributors => Set<PFAOnboardingRequestDistributor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TerritoryMaster>(entity =>
        {
            entity.ToTable("TerritoryMaster");
            entity.HasKey(e => e.TerritoryId);
            entity.Property(e => e.TerritoryName).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<DealerMaster>(entity =>
        {
            entity.ToTable("DealerMaster");
            entity.HasKey(e => e.DealerId);
            entity.Property(e => e.RetailerShopName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

            entity.HasOne(e => e.Territory)
                .WithMany(t => t.Dealers)
                .HasForeignKey(e => e.TerritoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserDetails>(entity =>
        {
            entity.ToTable("UserDetails");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Mobile).HasMaxLength(15).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.EmailId).HasMaxLength(256);
            entity.Property(e => e.PanNo).HasMaxLength(10);
            entity.Property(e => e.AadhaarNumber).HasMaxLength(12);
            entity.Property(e => e.UanNumber).HasMaxLength(12);

            entity.HasIndex(e => e.Mobile);
        });

        modelBuilder.Entity<PFAOnboardingRequest>(entity =>
        {
            entity.ToTable("PFAOnboardingRequests");
            entity.HasKey(e => e.RequestId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Mobile).HasMaxLength(15).IsRequired();
            entity.Property(e => e.EmailId).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PanNo).HasMaxLength(10).IsRequired();
            entity.Property(e => e.AadhaarNumber).HasMaxLength(12).IsRequired();
            entity.Property(e => e.UanNumber).HasMaxLength(12);
            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.Mobile);

            entity.HasOne(e => e.Territory)
                .WithMany(t => t.OnboardingRequests)
                .HasForeignKey(e => e.TerritoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.LinkedUser)
                .WithMany()
                .HasForeignKey(e => e.UserDetailsId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PFAOnboardingRequestDistributor>(entity =>
        {
            entity.ToTable("PFAOnboardingRequestDistributors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.RequestId, e.DealerId }).IsUnique();

            entity.HasOne(e => e.Request)
                .WithMany(r => r.SelectedDistributors)
                .HasForeignKey(e => e.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Dealer)
                .WithMany(d => d.OnboardingSelections)
                .HasForeignKey(e => e.DealerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
