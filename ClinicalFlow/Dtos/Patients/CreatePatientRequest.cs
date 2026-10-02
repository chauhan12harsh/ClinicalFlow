
using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.DTOs.Patients;

public class CreatePatientRequest
{
    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string MedicalRecordNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }
}