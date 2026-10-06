using ClinicalFlow.Enums;

namespace ClinicalFlow.Models
{
    public class Encounter
    {
        public int EncounterId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string ChiefComplaint { get; set; } = string.Empty;

        public string? Diagnosis { get; set; } = string.Empty;

        public string? ClinicalNotes { get; set; } = string.Empty;

        public EncounterStatus Status { get; set; } = EncounterStatus.InProgress;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        // Navigation properties
        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

    }
}
