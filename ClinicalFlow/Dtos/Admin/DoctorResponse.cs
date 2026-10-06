namespace ClinicalFlow.Dtos.Admin
{
    public class DoctorResponse
    {
        public Guid DoctorId { get; set; }

        public Guid ApplicationUserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Speciality { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
