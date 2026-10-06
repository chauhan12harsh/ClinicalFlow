namespace ClinicalFlow.Models
{
    public class Prescription
    {
        public Guid PrescriptionId{  get; set; }  = Guid.NewGuid();

        public Guid EncounterId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Encounter Encounter { get; set; } = null!;

        public ICollection<PrescriptionMedication> Medications { get; set; } = new List<PrescriptionMedication>();
    }
}
