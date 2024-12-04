using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;

namespace TweyesBackend.Core.Contract
{
    public interface IScheduleService
    {
        Task<Response<string>> BookScheduleAsync(BookScheduleRequest request);
        Task<Response<string>> AcceptBookingAsync(string bookingId);
        Task<Response<string>> RejectBookingAsync(string bookingId);
        Task<Response<List<BookingResponse>>> GetUserBookingsAsync();
        Task<Response<List<BookingResponse>>> GetTutorBookingsAsync();
        Task<Response<string>> DeleteBookingAsync(string bookingId);
    }
}
