﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class OtpRequest
    {
        [Required]
        public string email { get; set; }
    }
}
