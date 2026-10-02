using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Models
{
    public class Patient
    {
        public int PatientId { set; get; }

        [Required]
        [MaxLength(30)]
        public string MedicalRecordNumber { set; get; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { set; get; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { set; get; } = string.Empty;

        public DateOnly? DateOfBirth { set; get; }

        [MaxLength(20)]
        public string? Gender { set; get; }

        [MaxLength(20)]
        public string? PhoneNumber { set; get; }

        [MaxLength(200)]
        public string? Email { set; get; }

        public DateTime CreatedAt = DateTime.UtcNow;

        public ICollection<Encounter> Encounters { set; get; } = new List<Encounter>();

    }
}
