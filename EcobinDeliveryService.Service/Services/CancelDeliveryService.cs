using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class CancelDeliveryService : ICancelDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelDeliveryService(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(
        long deliveryId,
        long userId,
        string role,
        string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.");

        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

        if (delivery == null)
            return false;

        // Verify that the user owns or is assigned to this delivery.
        bool isAuthorized =
            (role == "Seller" && delivery.SellerId == userId) ||
            (role == "Collector" && delivery.CollectorId == userId);

        if (!isAuthorized)
            return false;

        // Only Requested and Accepted deliveries can be cancelled.
        if (delivery.Status != DeliveryStatus.Requested &&
            delivery.Status != DeliveryStatus.Accepted)
        {
            throw new InvalidOperationException(
                "This delivery cannot be cancelled in its current status.");
        }

        var previousStatus = delivery.Status;
        var now = DateTimeOffset.UtcNow;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            delivery.Status = DeliveryStatus.Cancelled;
            delivery.UpdatedAt = now;

            delivery.StatusHistories.Add(new DeliveryStatusHistory
            {
                DeliveryId = delivery.Id,
                PreviousStatus = previousStatus,
                NewStatus = DeliveryStatus.Cancelled,
                ChangedByUserId = userId,
                ChangedAt = now,
                Remarks = $"Cancelled by {role}. Reason: {reason.Trim()}"
            });

            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }
}
