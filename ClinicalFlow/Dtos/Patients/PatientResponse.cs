
namespace ClinicalFlow.DTOs.Patients;

public class PatientResponse
{
    public int PatientId { get; set; }

    public string MedicalRecordNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }
}