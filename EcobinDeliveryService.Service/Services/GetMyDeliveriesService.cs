using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.DTOs.Response;

namespace EcobinDeliveryService.Service.Services;

public class GetMyDeliveriesService : IGetMyDeliveriesService
{
    private readonly IDeliveryRepository _deliveryRepository;

    public GetMyDeliveriesService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<IReadOnlyList<DeliverySummaryResponseDto>> HandleAsync(
        long userId,
        string role)
    {
        if (role == "Seller")
        {
            var deliveries =
                await _deliveryRepository.GetBySellerIdAsync(userId);

            return deliveries.Select(d => new DeliverySummaryResponseDto
            {
                Id = d.Id,
                ProductId = d.ProductId,
                SellerId = d.SellerId,
                CollectorId = d.CollectorId,
                Status = d.Status,
                PickupAddress = d.PickupAddress,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList();
        }

        if (role == "Collector")
        {
            var deliveries =
                await _deliveryRepository.GetByCollectorIdAsync(userId);

            return deliveries.Select(d => new DeliverySummaryResponseDto
            {
                Id = d.Id,
                ProductId = d.ProductId,
                SellerId = d.SellerId,
                CollectorId = d.CollectorId,
                Status = d.Status,
                PickupAddress = d.PickupAddress,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList();
        }

        throw new UnauthorizedAccessException("Invalid user role.");
    }
}
