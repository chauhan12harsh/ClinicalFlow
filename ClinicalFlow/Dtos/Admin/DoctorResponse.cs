namespace ClinicalFlow.Dtos.Admin
{
    public class DoctorResponse
    {
        public int DoctorId { get; set; }

        public int ApplicationUserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Speciality { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
