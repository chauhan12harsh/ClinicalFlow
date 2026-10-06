using ClinicalFlow.Enums;

namespace ClinicalFlow.Models;

public class ApplicationUser
{
    public int ApplicationUserId { get; set; }

    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Doctor;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
