using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.DTO.Request
{
    public class RegisterRequest
    {
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }       
        [EmailAddress]
        public string Email { get; set; }
       
        [Phone]
        public string PhoneNumber { get; set; }
        
        public string Role { get; set; }
       
        public string ContactAddress { get; set; }
       
        public string State { get; set; }

        public string Image { get; set; }


    }
    //public class UpdateUserRequest
    //{
    //    public IFormFile Image
    //    {
    //        get; set;
    //    }

    //}
}
