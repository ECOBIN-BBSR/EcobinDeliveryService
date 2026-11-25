using System;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Repostiory.Entities;

public class DeliveryStatusHistory
{
    public long Id { get; set; }

    public long DeliveryId { get; set; }

    public DeliveryStatus? PreviousStatus { get; set; }

    public DeliveryStatus NewStatus { get; set; }

    public long ChangedByUserId { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public string? Remarks { get; set; }

    public Delivery Delivery { get; set; } = null!;
}
