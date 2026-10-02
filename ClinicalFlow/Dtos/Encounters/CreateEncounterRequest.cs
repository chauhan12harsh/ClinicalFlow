
using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.DTOs.Encounters;

public class CreateEncounterRequest
{
    [Range(1, int.MaxValue)]
    public int PatientId { get; set; }

    [Range(1, int.MaxValue)]
    public int DoctorId { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string ChiefComplaint { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Diagnosis { get; set; }

    public string? ClinicalNotes { get; set; }
}