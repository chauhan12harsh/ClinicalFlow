using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Speciality { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    }
}
