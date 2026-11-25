using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class AcceptDeliveryService : IAcceptDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptDeliveryService(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(
        long deliveryId,
        long collectorId,
        long scheduleOptionId)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);

        if (delivery == null)
            return false;

        // Only the assigned collector can accept the delivery.
        if (delivery.CollectorId != collectorId)
            return false;

        // A delivery can only be accepted while it is Requested.
        if (delivery.Status != DeliveryStatus.Requested)
            throw new InvalidOperationException(
                "This delivery cannot be accepted in its current status.");

        // The selected schedule must belong to this delivery.
        var selectedSchedule = delivery.ScheduleOptions
            .FirstOrDefault(s => s.Id == scheduleOptionId);

        if (selectedSchedule == null)
            return false;

        var now = DateTimeOffset.UtcNow;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            delivery.SelectedScheduleId = selectedSchedule.Id;
            delivery.Status = DeliveryStatus.Accepted;
            delivery.UpdatedAt = now;

            delivery.StatusHistories.Add(new DeliveryStatusHistory
            {
                DeliveryId = delivery.Id,
                PreviousStatus = DeliveryStatus.Requested,
                NewStatus = DeliveryStatus.Accepted,
                ChangedByUserId = collectorId,
                ChangedAt = now,
                Remarks = "Collector accepted the proposed schedule."
            });

            await _unitOfWork.SaveChangesAsync();
        });

        return true;
    }
}
