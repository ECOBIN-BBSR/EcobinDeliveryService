using System;

namespace EcobinDeliveryService.Shared.DTOs.Request;

public class ProposedScheduleDto
{
    public DateTimeOffset StartAt { get; set; }

    public DateTimeOffset EndAt { get; set; }
}
