using System;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Shared.DTOs.Response;

public class DeliveryStatusHistoryResponseDto
{
    public DeliveryStatus? PreviousStatus { get; set; }
    
    public DeliveryStatus NewStatus { get; set; }

    public long ChangedByUserId { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public string? Remarks { get; set; }
}
