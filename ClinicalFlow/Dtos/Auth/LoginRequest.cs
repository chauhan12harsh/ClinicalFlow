using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Dtos.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;
    }
}
