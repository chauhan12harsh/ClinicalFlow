using ClinicalFlow.Enums;

namespace ClinicalFlow.Models;

public class ApplicationUser
{
    public Guid ApplicationUserId { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;

    // Stores a password hash, never the plain-text password.
    public string PasswordHash { get; set; } = string.Empty;

    // Examples: Doctor, Admin, Nurse, Patient
    public UserRole Role { get; set; } = UserRole.Doctor;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
