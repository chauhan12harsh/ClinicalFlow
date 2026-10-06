namespace ClinicalFlow.Dtos.Prescriptions;

public class PrescriptionResponse
{
    public Guid PrescriptionId { get; set; }
    public Guid EncounterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PrescriptionMedicationResponse> Medications { get; set; } = [];
}

public class PrescriptionMedicationResponse
{
    public Guid PrescriptionMedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int Quantity { get; set; }
    public string? Instructions { get; set; }
}