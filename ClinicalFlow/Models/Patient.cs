namespace ClinicalFlow.Models
{
    public class Patient
    {
        public Guid PatientId { set; get; } = Guid.NewGuid();

        public Guid ApplicationUserId { get; set; }

        public string MedicalRecordNumber { set; get; } = string.Empty;

        public string FirstName { set; get; } = string.Empty;

        public string LastName { set; get; } = string.Empty;

        public DateOnly? DateOfBirth { set; get; }

        public string? Gender { set; get; }

        public string? PhoneNumber { set; get; }

        public string? Email { set; get; }


        public DateTime CreatedAt = DateTime.UtcNow;

        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<Encounter> Encounters { set; get; } = new List<Encounter>();     


    }
}
