using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Models
{
    public class PrescriptionMedication
    {
        public int PrescriptionMedicationId { get; set; }

        public int PrescriptionId { get; set; }

        [Required]
        [MaxLength(150)]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Frequency { get; set; } = string.Empty;

        public int DurationDays { get; set; }

        public int Quantity { get; set; }

        [MaxLength(500)]
        public string? Instructions { get; set; }

        public Prescription Prescription { get; set; } = null!;
    }
}
