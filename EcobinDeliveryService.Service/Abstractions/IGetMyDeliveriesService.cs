using System;
using EcobinDeliveryService.Shared.DTOs.Response;

namespace EcobinDeliveryService.Service.Abstractions;

public interface IGetMyDeliveriesService
{
    Task<IReadOnlyList<DeliverySummaryResponseDto>> HandleAsync(
        long userId,
        string role);
}
