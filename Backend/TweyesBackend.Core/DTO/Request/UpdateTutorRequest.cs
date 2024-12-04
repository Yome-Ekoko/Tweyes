using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class UpdateTutorRequest
    {
        [Required]
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
