namespace ClinicalFlow.Models
{
    public class PrescriptionMedication
    {
        public Guid PrescriptionMedicationId { get; set; } = Guid.NewGuid();

        public Guid PrescriptionId { get; set; }

        public string MedicationName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;

        public int DurationDays { get; set; }

        public int Quantity { get; set; }

        public string? Instructions { get; set; }

        // Navigation Property
        public Prescription Prescription { get; set; } = null!;
    }
}
