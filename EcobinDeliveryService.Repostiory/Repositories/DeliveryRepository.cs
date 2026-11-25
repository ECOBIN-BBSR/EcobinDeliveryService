using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Repostiory.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcobinDeliveryService.Repostiory.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly ApplicationDbContext _context;

    public DeliveryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Delivery?> GetByIdAsync(long id)
    {
        return await _context.Deliveries
            .Include(d => d.ScheduleOptions)
            .Include(d => d.StatusHistories)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IReadOnlyList<Delivery>> GetBySellerIdAsync(long sellerId)
    {
        return await _context.Deliveries
            .AsNoTracking()
            .Where(d => d.SellerId == sellerId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Delivery>> GetByCollectorIdAsync(long collectorId)
    {
        return await _context.Deliveries
            .AsNoTracking()
            .Where(d => d.CollectorId == collectorId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Delivery delivery)
    {
        await _context.Deliveries.AddAsync(delivery);
    }
}