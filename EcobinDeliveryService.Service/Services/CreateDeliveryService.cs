using System;
using EcobinDeliveryService.Repostiory.Abstractions;
using EcobinDeliveryService.Repostiory.Entities;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.DTOs.Request;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Service.Services;

public class CreateDeliveryService : ICreateDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeliveryService(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> HandleAsync(
        CreateDeliveryRequestDto request,
        long sellerId)
    {
        // Basic validation
        if (request.ProductId == Guid.Empty)
            throw new ArgumentException("Invalid product ID.");

        if (request.CollectorId <= 0)
            throw new ArgumentException("Invalid collector ID.");

        if (string.IsNullOrWhiteSpace(request.PickupAddress))
            throw new ArgumentException("Pickup address is required.");

        if (request.ProposedSchedules == null ||
            request.ProposedSchedules.Count == 0)
        {
            throw new ArgumentException(
                "At least one proposed schedule is required.");
        }

        var now = DateTimeOffset.UtcNow;

        // Validate proposed schedules
        foreach (var schedule in request.ProposedSchedules)
        {
            if (schedule.StartAt >= schedule.EndAt)
            {
                throw new ArgumentException(
                    "Schedule start time must be before end time.");
            }

            if (schedule.StartAt <= now)
            {
                throw new ArgumentException(
                    "Proposed schedules must be in the future.");
            }
        }

        // Create delivery
        var delivery = new Delivery
        {
            ProductId = request.ProductId,
            SellerId = sellerId,
            CollectorId = request.CollectorId,
            PickupAddress = request.PickupAddress.Trim(),
            Notes = request.Notes?.Trim(),
            Status = DeliveryStatus.Requested,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Add proposed schedule options
        foreach (var schedule in request.ProposedSchedules)
        {
            delivery.ScheduleOptions.Add(
                new DeliveryScheduleOption
                {
                    StartAt = schedule.StartAt,
                    EndAt = schedule.EndAt,
                    CreatedByUserId = sellerId,
                    CreatedAt = now
                });
        }

        // Add initial status history
        delivery.StatusHistories.Add(
            new DeliveryStatusHistory
            {
                PreviousStatus = null,
                NewStatus = DeliveryStatus.Requested,
                ChangedByUserId = sellerId,
                ChangedAt = now,
                Remarks = "Delivery request created."
            });

        // Save everything
        await _deliveryRepository.AddAsync(delivery);

        await _unitOfWork.SaveChangesAsync();

        return delivery.Id;
    }
}
