using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class FailDeliveryService : IFailDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FailDeliveryService(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(
        long deliveryId,
        long collectorId,
        string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required.");

        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

        if (delivery == null)
            return false;

        if (delivery.CollectorId != collectorId)
            return false;

        if (delivery.Status != DeliveryStatus.Pickup)
            throw new InvalidOperationException(
                "Only deliveries in Pickup status can be marked as Failed.");

        var now = DateTimeOffset.UtcNow;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            delivery.Status = DeliveryStatus.Failed;
            delivery.UpdatedAt = now;

            delivery.StatusHistories.Add(new DeliveryStatusHistory
            {
                DeliveryId = delivery.Id,
                PreviousStatus = DeliveryStatus.Pickup,
                NewStatus = DeliveryStatus.Failed,
                ChangedByUserId = collectorId,
                ChangedAt = now,
                Remarks = $"Delivery failed. Reason: {reason.Trim()}"
            });

            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }
}
