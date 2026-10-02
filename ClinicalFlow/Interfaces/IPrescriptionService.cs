using ClinicalFlow.Dtos.Prescriptions;

namespace ClinicalFlow.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponse?> CreateAsync(int encounterId, CreatePrescriptionRequest request);
        Task<List<PrescriptionResponse>?> GetByEncounterIdAsync(int encounterId);
    }
}
