using System;
using EcobinDeliveryService.Shared.DTOs.Request;

namespace EcobinDeliveryService.Service.Abstractions;

public interface ICreateDeliveryService
{
    Task<long> HandleAsync(
        CreateDeliveryRequestDto request,
        long sellerId);
}
