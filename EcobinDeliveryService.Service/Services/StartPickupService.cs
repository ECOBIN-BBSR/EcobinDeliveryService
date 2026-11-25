using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class StartPickupService : IStartPickupService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartPickupService(
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

        if (delivery.Status != DeliveryStatus.Accepted)
            throw new InvalidOperationException(
                "Pickup can only be started for an accepted delivery.");

        var now = DateTimeOffset.UtcNow;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            delivery.Status = DeliveryStatus.Pickup;
            delivery.UpdatedAt = now;

            delivery.StatusHistories.Add(new DeliveryStatusHistory
            {
                DeliveryId = delivery.Id,
                PreviousStatus = DeliveryStatus.Accepted,
                NewStatus = DeliveryStatus.Pickup,
                ChangedByUserId = collectorId,
                ChangedAt = now,
                Remarks = "Collector started the pickup."
            });

            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }
}
