using System;

namespace EcobinDeliveryService.Shared.DTOs.Request;

public class FailDeliveryRequestDto
{
    public string Reason {get; set;} = string.Empty;
}
