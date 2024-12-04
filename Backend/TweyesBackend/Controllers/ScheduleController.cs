using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweyesBackend.Core.Contract;
using TweyesBackend.Core.DTO.Request;

namespace TweyesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [Authorize(Roles = "Student")]

        [HttpPost("book")]
        public async Task<IActionResult> BookSchedule([FromBody] BookScheduleRequest request)
        {
            var response = await _scheduleService.BookScheduleAsync(request);
            return Ok(response);
        }
        [Authorize(Roles = "Tutor")]
        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcceptBooking(string id)
        {
            var response = await _scheduleService.AcceptBookingAsync(id);
            return Ok(response);
        }
        [Authorize(Roles = "Tutor")]

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectBooking(string id)
        {
            var response = await _scheduleService.RejectBookingAsync(id);
            return Ok(response);
        }

        [HttpGet("mybookings")]
        public async Task<IActionResult> GetUserBookings()
        {
            var response = await _scheduleService.GetUserBookingsAsync();
            return Ok(response);
        }
        [Authorize(Roles = "Tutor")]

        [HttpGet("allTutorsBookings")]
        public async Task<IActionResult> GetAllTutorsBookings()
        {
            var response = await _scheduleService.GetTutorBookingsAsync();
            return Ok(response);
        }
        [HttpGet("deleteBooking")]
        public async Task<IActionResult> DeleteBooking(string bookingId)
        {
            var response = await _scheduleService.DeleteBookingAsync(bookingId);
            return Ok(response);
        }
    }

}
