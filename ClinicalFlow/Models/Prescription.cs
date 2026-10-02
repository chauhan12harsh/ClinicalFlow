namespace ClinicalFlow.Models
{
    public class Prescription
    {

        public int PrescriptionId{  get; set; }

        public int EncounterId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Encounter Encounter { get; set; } = null!;

        public ICollection<PrescriptionMedication> Medications { get; set; }
            = new List<PrescriptionMedication>();
    }
}
