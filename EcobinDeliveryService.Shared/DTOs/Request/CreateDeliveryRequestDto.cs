using System;

namespace EcobinDeliveryService.Shared.DTOs.Request;

public class CreateDeliveryRequestDto
{
    public Guid ProductId { get; set; }

    public long CollectorId { get; set; }

    public string PickupAddress { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public List<ProposedScheduleDto> ProposedSchedules { get; set; }
        = new();
}
