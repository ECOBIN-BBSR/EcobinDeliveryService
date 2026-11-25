using System;
using EcobinDeliveryService.Shared.DTOs.Response;

namespace EcobinDeliveryService.Service.Abstractions;

public interface IGetDeliveryByIdService
{
    Task<DeliveryDetailsResponseDto?> HandleAsync(
        long deliveryId,
        long userId,
        string role);
}
