using System.Security.Claims;
using EcobinDeliveryService.Service.Abstractions;
using EcobinDeliveryService.Shared.DTOs.Request;
using EcobinDeliveryService.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcobinDeliveryService.Web.Controllers
{
    [Route("api/deliveries")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly ICreateDeliveryService _createDeliveryService;
        private readonly IGetDeliveryByIdService _getDeliveryByIdService;
        private readonly IGetMyDeliveriesService _getMyDeliveriesService;
        private readonly IAcceptDeliveryService _acceptDeliveryService;
        private readonly IStartPickupService _startPickupService;
        private readonly ICompleteDeliveryService _completeDeliveryService;
        private readonly ICancelDeliveryService _cancelDeliveryService;
        private readonly IFailDeliveryService _failDeliveryService;
        public DeliveryController(
            ICreateDeliveryService createDeliveryService,
            IGetDeliveryByIdService getDeliveryByIdService,
            IGetMyDeliveriesService getMyDeliveriesService,
            IAcceptDeliveryService acceptDeliveryService,
            IStartPickupService startPickupService,
            ICompleteDeliveryService completeDeliveryService,
            ICancelDeliveryService cancelDeliveryService,
            IFailDeliveryService failDeliveryService
            )
        {
            _createDeliveryService = createDeliveryService;
            _getDeliveryByIdService = getDeliveryByIdService;
            _getMyDeliveriesService = getMyDeliveriesService;
            _acceptDeliveryService = acceptDeliveryService;
            _startPickupService = startPickupService;
            _completeDeliveryService = completeDeliveryService;
            _cancelDeliveryService = cancelDeliveryService;
            _failDeliveryService = failDeliveryService;
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequestDto request)
        {
            var sellerIdClaim = User.FindFirst("sub")?.Value;

            if(!long.TryParse(sellerIdClaim, out var sellerId))
            {
                return Unauthorized("Invalid Seller Identity");
            }

            try
            {
                var deliveryId = await _createDeliveryService.HandleAsync(request, sellerId);
                return StatusCode(StatusCodes.Status201Created,
                new
                {
                    message = "Delivery request Created succesfully",
                    deliveryId
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Seller,Collector")]
        public async Task<IActionResult> GetDeliveryById(long id)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(role))
                return Forbid();

            var result = await _getDeliveryByIdService.HandleAsync(
                id,
                userId,
                role
            );

            if (result == null)
                return NotFound(new
                {
                    message = "Delivery not found."
                });

            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Seller,Collector")]
        public async Task<IActionResult> GetMyDeliveries()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(role))
                return Forbid();

            try
            {
                var result = await _getMyDeliveriesService.HandleAsync(
                    userId,
                    role
                );

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{id:long}/accept")]
        [Authorize(Roles = "Collector")]
        public async Task<IActionResult> AcceptDelivery(
            long id,
            [FromBody] AcceptDeliveryRequestDto request)
        {
            var collectorIdClaim = User.FindFirst("sub")?.Value;

            if (!long.TryParse(collectorIdClaim, out var collectorId))
                return Unauthorized("Invalid Collector Identity.");

            if (request.ScheduleOptionId <= 0)
                return BadRequest("Invalid schedule option ID.");

            try
            {
                var result = await _acceptDeliveryService.HandleAsync(
                    id,
                    collectorId,
                    request.ScheduleOptionId);

                if (!result)
                    return NotFound(new
                    {
                        message = "Delivery or schedule not found."
                    });

                return Ok(new
                {
                    message = "Delivery schedule accepted successfully.",
                    deliveryId = id,
                    selectedScheduleId = request.ScheduleOptionId,
                    status = DeliveryStatus.Accepted.ToString()
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("{id:long}/pickup")]
        [Authorize(Roles = "Collector")]
        public async Task<IActionResult> StartPickup(long id)
        {
            var collectorIdClaim = User.FindFirst("sub")?.Value;

            if (!long.TryParse(collectorIdClaim, out var collectorId))
                return Unauthorized("Invalid Collector Identity.");

            try
            {
                var result = await _startPickupService.HandleAsync(
                    id, collectorId);

                if (!result)
                    return NotFound(new
                    {
                        message = "Delivery not found."
                    });

                return Ok(new
                {
                    message = "Pickup started successfully.",
                    deliveryId = id,
                    status = DeliveryStatus.Pickup.ToString()
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("{id:long}/complete")]
        [Authorize(Roles = "Collector")]
        public async Task<IActionResult> CompleteDelivery(long id)
        {
            var collectorIdClaim = User.FindFirst("sub")?.Value;

            if (!long.TryParse(collectorIdClaim, out var collectorId))
                return Unauthorized("Invalid Collector Identity.");

            try
            {
                var result = await _completeDeliveryService.HandleAsync(
                    id, collectorId);

                if (!result)
                    return NotFound(new
                    {
                        message = "Delivery not found."
                    });

                return Ok(new
                {
                    message = "Delivery completed successfully.",
                    deliveryId = id,
                    status = DeliveryStatus.Delivered.ToString()
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("{id:long}/cancel")]
        [Authorize(Roles = "Seller,Collector")]
        public async Task<IActionResult> CancelDelivery(
            long id,
            [FromBody] CancelDeliveryRequestDto request)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(role))
                return Forbid();

            try
            {
                var result = await _cancelDeliveryService.HandleAsync(
                    id,
                    userId,
                    role,
                    request.Reason);

                if (!result)
                    return NotFound(new
                    {
                        message = "Delivery not found."
                    });

                return Ok(new
                {
                    message = "Delivery cancelled successfully.",
                    deliveryId = id,
                    status = DeliveryStatus.Cancelled.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("{id:long}/fail")]
        [Authorize(Roles = "Collector")]
        public async Task<IActionResult> FailDelivery(
            long id,
            [FromBody] FailDeliveryRequestDto request)
        {
            var collectorIdClaim = User.FindFirst("sub")?.Value;

            if (!long.TryParse(collectorIdClaim, out var collectorId))
                return Unauthorized("Invalid Collector Identity.");

            try
            {
                var result = await _failDeliveryService.HandleAsync(
                    id,
                    collectorId,
                    request.Reason);

                if (!result)
                    return NotFound(new
                    {
                        message = "Delivery not found."
                    });

                return Ok(new
                {
                    message = "Delivery marked as failed successfully.",
                    deliveryId = id,
                    status = DeliveryStatus.Failed.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
