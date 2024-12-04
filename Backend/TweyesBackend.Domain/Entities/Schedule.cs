using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Domain.Enum;
using TweyesBackend.Domain.Entities.Base;
using TweyesBackend.Domain.Common;

namespace TweyesBackend.Domain.Entities
{
    public class Schedule : EntityBase<string>
    {
        public Schedule()
        {
            SetNewId();
        }
        [Required]
       [StringLength(450)]
        public string? TutorId { get; set; }=string.Empty;

        [Required]
        [StringLength(450)]
        public string? StudentId { get; set; }= string.Empty;

        [Required]
        public string Day { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string Time1 { get; set; }

        public BookingStatus Status { get; set; }

        public virtual Tutor? Tutor { get; set; }

        public virtual T_User? Student { get; set; }

        public override void SetNewId()
        {
            Id = $"TUT_{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }
}

