using System;

namespace EcobinDeliveryService.Service.Abstractions;

public interface IFailDeliveryService
{
    Task<bool> HandleAsync(
        long deliveryId,
        long collectorId,
        string reason);
}
