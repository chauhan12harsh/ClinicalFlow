namespace ClinicalFlow.Models
{
    public class Doctor
    {
        public Guid DoctorId { get; set; } = Guid.NewGuid();

        public Guid ApplicationUserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string? PhoneNumber { set; get; }

        public string? Speciality { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    }
}
