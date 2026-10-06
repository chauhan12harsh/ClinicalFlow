using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Dtos.Auth
{
    public class LoginRequest
    {
        [Required]        
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;
    }
}
