
using ClinicalFlow.Enums;

namespace ClinicalFlow.DTOs.Encounters;

public class EncounterResponse
{
    public Guid EncounterId { get; set; }

    public Guid PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public string ChiefComplaint { get; set; } = string.Empty;

    public string? Diagnosis { get; set; }

    public string? ClinicalNotes { get; set; }

    public EncounterStatus Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}