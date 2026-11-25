using EcobinDeliveryService.Repostiory.Entities;

namespace EcobinDeliveryService.Repostiory.Abstractions;

public interface IDeliveryRepository
{
    Task<Delivery?> GetByIdAsync(
        long id);

    Task<IReadOnlyList<Delivery>> GetBySellerIdAsync(
        long sellerId);

    Task<IReadOnlyList<Delivery>> GetByCollectorIdAsync(
        long collectorId);

    Task AddAsync(
        Delivery delivery);
}
