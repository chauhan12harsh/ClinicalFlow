using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Dtos.Prescriptions;

public class CreatePrescriptionRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one medication is required.")]
    [MaxLength(50, ErrorMessage = "A prescription cannot contain more than 50 medications.")]
    public List<CreatePrescriptionMedicationRequest> Medications { get; set; } = [];
}

public class CreatePrescriptionMedicationRequest
{
    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string MedicationName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Dosage { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Frequency { get; set; } = string.Empty;

    [Range(1, 3650)]
    public int DurationDays { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? Instructions { get; set; }
}