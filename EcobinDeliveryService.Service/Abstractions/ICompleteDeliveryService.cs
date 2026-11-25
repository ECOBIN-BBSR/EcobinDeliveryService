using System;

namespace EcobinDeliveryService.Service.Abstractions;

public interface ICompleteDeliveryService
{
    Task<bool> HandleAsync(long deliveryId, long collectorId);
}
