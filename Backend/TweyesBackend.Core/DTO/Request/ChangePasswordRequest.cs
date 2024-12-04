using System.ComponentModel.DataAnnotations;

namespace TweyesBackend.Core.DTO.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
