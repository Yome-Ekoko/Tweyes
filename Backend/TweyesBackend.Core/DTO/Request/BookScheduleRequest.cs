using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class BookScheduleRequest
    {
        public string TutorId { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Time1 { get; set; }
    }
}
