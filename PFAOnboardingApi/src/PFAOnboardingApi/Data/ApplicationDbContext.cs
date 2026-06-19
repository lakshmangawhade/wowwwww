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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
