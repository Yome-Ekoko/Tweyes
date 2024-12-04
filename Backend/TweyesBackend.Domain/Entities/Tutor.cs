using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.Entities.Base;

namespace TweyesBackend.Domain.Entities
{
    public class Tutor : EntityBase<string>
    {
        public Tutor()
        {
            SetNewId();
        }

        public string Introduction { get; set; } = string.Empty;
        public List<string> Qualifications { get; set; } = new List<string>();
        public List<string> Languages { get; set; } = new List<string>();
        public string PreferredCommunication { get; set; } = string.Empty;
        public string TargetedClass { get; set; } = string.Empty;
        public List<string> AvailableDays { get; set; } = new List<string>();
        public List<string> AvailableTime1 { get; set; } = new List<string>();
        public List<string> AvailableTime { get; set; } = new List<string>();
        public string HourlyRate { get; set; } = string.Empty;
        public string PreferredCurrency { get; set; } = string.Empty;
        public T_User? User { get; set; }
        public string? UserId { get; set; }= string.Empty;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();


        public override void SetNewId()
        {
            Id = $"TUT_{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }
   
}
