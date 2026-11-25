using System;

namespace EcobinDeliveryService.Shared.DTOs.Response;

public class DeliveryScheduleResponseDto
{
    public long Id { get; set; }
    
    public DateTimeOffset StartAt { get; set; }

    public DateTimeOffset EndAt { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
