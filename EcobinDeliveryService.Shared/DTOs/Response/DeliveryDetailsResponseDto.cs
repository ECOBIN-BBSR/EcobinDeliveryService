using System;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Shared.DTOs.Response;

public class DeliveryDetailsResponseDto
{
    public long Id { get; set; }
    
    public Guid ProductId { get; set; }

    public long SellerId { get; set; }

    public long CollectorId { get; set; }

    public DeliveryStatus Status { get; set; }

    public long? SelectedScheduleId { get; set; }

    public string PickupAddress { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public List<DeliveryScheduleResponseDto> ScheduleOptions { get; set; } = [];
    
    public List<DeliveryStatusHistoryResponseDto> StatusHistories { get; set; } = [];
}
