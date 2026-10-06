using ClinicalFlow.Dtos.Admin;

namespace ClinicalFlow.Interfaces;

public interface IAdminService
{
    Task<DoctorResponse> CreateDoctorAsync(CreateDoctorRequest request);
    Task<List<DoctorResponse>> GetAllAsync();
}