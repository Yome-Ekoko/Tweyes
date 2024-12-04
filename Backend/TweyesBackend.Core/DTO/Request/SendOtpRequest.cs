using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class SendOtpRequest
    {
        public string email { get; set; }
        public string Otp { get; set; }

        public string firstname { get; set; }
    }
}
