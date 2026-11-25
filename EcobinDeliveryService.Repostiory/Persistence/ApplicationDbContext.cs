using EcobinDeliveryService.Repostiory.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcobinDeliveryService.Repostiory.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Delivery> Deliveries => Set<Delivery>();

    public DbSet<DeliveryScheduleOption> DeliveryScheduleOptions =>
        Set<DeliveryScheduleOption>();

    public DbSet<DeliveryStatusHistory> DeliveryStatusHistories =>
        Set<DeliveryStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}
