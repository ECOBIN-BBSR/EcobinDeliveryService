using System;

namespace EcobinDeliveryService.Shared.DTOs.Request;

public class CancelDeliveryRequestDto
{
    public string Reason { get; set; } = string.Empty;
}
