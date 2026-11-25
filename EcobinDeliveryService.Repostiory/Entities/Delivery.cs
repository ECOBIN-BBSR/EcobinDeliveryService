using System;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Repostiory.Entities;

public class Delivery
{
    public long Id { get; set; }

    // External ID from Product Service
    public Guid ProductId { get; set; }

    // External IDs from Identity Service
    public long SellerId { get; set; }
    
    public long CollectorId { get; set; }
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Requested;
    // The schedule selected by the collector
    public long? SelectedScheduleId { get; set; }

    public DeliveryScheduleOption? SelectedSchedule { get; set; }

    public string PickupAddress { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<DeliveryScheduleOption> ScheduleOptions { get; set; }
        = new List<DeliveryScheduleOption>();

    public ICollection<DeliveryStatusHistory> StatusHistories { get; set; }
        = new List<DeliveryStatusHistory>();
}
