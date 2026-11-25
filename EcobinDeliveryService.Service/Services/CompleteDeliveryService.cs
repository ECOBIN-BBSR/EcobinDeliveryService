using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class CompleteDeliveryService : ICompleteDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteDeliveryService(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(long deliveryId, long collectorId)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

        if (delivery == null)
            return false;

        if (delivery.CollectorId != collectorId)
            return false;

        if (delivery.Status != DeliveryStatus.Pickup)
            throw new InvalidOperationException(
                "Only deliveries in Pickup status can be marked as Delivered.");

        var now = DateTimeOffset.UtcNow;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            delivery.Status = DeliveryStatus.Delivered;
            delivery.UpdatedAt = now;

            delivery.StatusHistories.Add(new DeliveryStatusHistory
            {
                DeliveryId = delivery.Id,
                PreviousStatus = DeliveryStatus.Pickup,
                NewStatus = DeliveryStatus.Delivered,
                ChangedByUserId = collectorId,
                ChangedAt = now,
                Remarks = "Collector completed the delivery."
            });

            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }
}
