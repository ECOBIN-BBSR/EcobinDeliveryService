using System;

namespace EcobinDeliveryService.Repostiory.Entities;

public class DeliveryScheduleOption
{
    public long Id { get; set; }

    public long DeliveryId { get; set; }

    public DateTimeOffset StartAt { get; set; }

    public DateTimeOffset EndAt { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public Delivery Delivery { get; set; } = null!;
}
