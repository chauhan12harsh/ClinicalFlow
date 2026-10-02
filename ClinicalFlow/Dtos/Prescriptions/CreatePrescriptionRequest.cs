using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Dtos.Prescriptions;

public class CreatePrescriptionRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one medication is required.")]
    public List<CreatePrescriptionMedicationRequest> Medications { get; set; } = [];
}

public class CreatePrescriptionMedicationRequest
{
    [Required]
    [MaxLength(150)]
    public string MedicationName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Dosage { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Frequency { get; set; } = string.Empty;

    [Range(1, 3650)]
    public int DurationDays { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [MaxLength(500)]
    public string? Instructions { get; set; }
}