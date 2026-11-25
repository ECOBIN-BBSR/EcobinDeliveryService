using System;
using EcobinDeliveryService.Shared.Enums;

namespace EcobinDeliveryService.Shared.DTOs.Response;

public class DeliverySummaryResponseDto
{
    public long Id { get; set; }
    public Guid ProductId { get; set; }
    public long SellerId { get; set; }
    public long CollectorId { get; set; }
    public DeliveryStatus Status { get; set; }
    public string PickupAddress { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
