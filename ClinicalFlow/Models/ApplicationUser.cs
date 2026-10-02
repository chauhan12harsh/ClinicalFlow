namespace ClinicalFlow.Models;

public class ApplicationUser
{
    public int ApplicationUserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Stores a password hash, never the plain-text password.
    public string PasswordHash { get; set; } = string.Empty;

    // Examples: Doctor, Admin
    public string Role { get; set; } = "Doctor";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
