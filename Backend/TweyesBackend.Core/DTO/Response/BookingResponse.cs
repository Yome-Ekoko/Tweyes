using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Domain.Enum;

namespace TweyesBackend.Core.DTO.Response
{
    public class BookingResponse
    {
        public string BookingId { get; set; }
        public string TutorId { get; set; }
        public string StudentId { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Time1 { get; set; }
        public BookingStatus Status { get; set; }
    }
}
