using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.DTOs.Response;

namespace EcobinDeliveryService.Service.Services;

public class GetDeliveryByIdService : IGetDeliveryByIdService
{
    private readonly IDeliveryRepository _deliveryRepository;

    public GetDeliveryByIdService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<DeliveryDetailsResponseDto?> HandleAsync(
        long deliveryId,
        long userId,
        string role)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

        if (delivery == null)
            return null;

        // Only the seller or assigned collector can view this delivery.
        bool isAuthorized =
            (role == "Seller" && delivery.SellerId == userId) ||
            (role == "Collector" && delivery.CollectorId == userId);

        if (!isAuthorized)
            return null;

        return new DeliveryDetailsResponseDto
        {
            Id = delivery.Id,
            ProductId = delivery.ProductId,
            SellerId = delivery.SellerId,
            CollectorId = delivery.CollectorId,
            Status = delivery.Status,
            SelectedScheduleId = delivery.SelectedScheduleId,
            PickupAddress = delivery.PickupAddress,
            Notes = delivery.Notes,
            CreatedAt = delivery.CreatedAt,
            UpdatedAt = delivery.UpdatedAt,

            ScheduleOptions = delivery.ScheduleOptions
                .OrderBy(s => s.StartAt)
                .Select(s => new DeliveryScheduleResponseDto
                {
                    Id = s.Id,
                    StartAt = s.StartAt,
                    EndAt = s.EndAt,
                    CreatedByUserId = s.CreatedByUserId,
                    CreatedAt = s.CreatedAt
                })
                .ToList(),

            StatusHistories = delivery.StatusHistories
                .OrderBy(h => h.ChangedAt)
                .Select(h => new DeliveryStatusHistoryResponseDto
                {
                    PreviousStatus = h.PreviousStatus,
                    NewStatus = h.NewStatus,
                    ChangedByUserId = h.ChangedByUserId,
                    ChangedAt = h.ChangedAt,
                    Remarks = h.Remarks
                })
                .ToList()
        };
    }
}
