using ClinicalFlow.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Models
{
    public class Encounter
    {
        public Guid EncounterId { get; set; } = Guid.NewGuid();

        // Foreign key to Patient
        public Guid PatientId {  get; set; }

        // Foreign key to Doctor
        public Guid DoctorId {  get; set; }

        public string ChiefComplaint {  get; set; } = string.Empty;

        public string? Diagnosis { get; set; } = string.Empty;

        public string? ClinicalNotes {  get; set; } = string.Empty;

        public EncounterStatus Status { get; set; } = EncounterStatus.InProgress;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    }
}
