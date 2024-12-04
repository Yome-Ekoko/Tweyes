using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Domain.Entities;

namespace TweyesBackend.Core.DTO.Response
{
    public class AddTutorResponse
    {
        public string Id { get; set; }
        public List<string> Qualifications { get; set; }
        public string Introduction { get; set; } = string.Empty;
        public List<string> Languages { get; set; }
        public string PreferredCommunication { get; set; } = string.Empty;
        public string TargetedClass { get; set; } = string.Empty;
        public List<string> AvailableDays { get; set; }
        public List<string> AvailableTime1 { get; set; }
        public List<string> AvailableTime { get; set; }
        public string HourlyRate { get; set; } = string.Empty;
        public string PreferredCurrency { get; set; } = string.Empty;
    }
}
