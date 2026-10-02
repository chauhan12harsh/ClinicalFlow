
using ClinicalFlow.DTOs.Patients;

namespace ClinicalFlow.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(CreatePatientRequest request);

    Task<List<PatientResponse>> GetAllAsync();

    Task<PatientResponse?> GetByIdAsync(int id);

    Task<PatientResponse?> UpdateAsync(
        int id,
        UpdatePatientRequest request);
}