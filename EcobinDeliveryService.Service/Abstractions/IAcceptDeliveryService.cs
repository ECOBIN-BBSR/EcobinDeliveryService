using System;

namespace EcobinDeliveryService.Service.Abstractions;

public interface IAcceptDeliveryService
{
    Task<bool> HandleAsync(
        long deliveryId,
        long collectorId,
        long scheduleOptionId);
}
