using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class WelcomeRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}
