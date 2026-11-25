using System;

namespace EcobinDeliveryService.Service.Abstractions;

public interface ICancelDeliveryService
{
    Task<bool> HandleAsync(
        long deliveryId,
        long userId,
        string role,
        string reason);
}
