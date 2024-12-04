using System.ComponentModel.DataAnnotations;

namespace TweyesBackend.Core.DTO.Request
{
    public class ResetUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
    }
}
