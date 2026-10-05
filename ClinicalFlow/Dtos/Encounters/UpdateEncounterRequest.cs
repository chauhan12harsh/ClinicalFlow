
using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.DTOs.Encounters;

public class UpdateEncounterRequest
{
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string ChiefComplaint { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Diagnosis { get; set; }

    [StringLength(5000)]
    public string? ClinicalNotes { get; set; }
}