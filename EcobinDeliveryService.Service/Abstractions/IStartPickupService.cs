using System;

namespace EcobinDeliveryService.Service.Abstractions;

public interface IStartPickupService
{
    Task<bool> HandleAsync(long deliveryId, long collectorId);
}
