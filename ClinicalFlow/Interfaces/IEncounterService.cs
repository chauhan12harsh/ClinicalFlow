
using ClinicalFlow.DTOs.Encounters;

namespace ClinicalFlow.Interfaces;

public interface IEncounterService
{
    Task<EncounterResponse> CreateAsync(CreateEncounterRequest request);

    Task<EncounterResponse?> GetByIdAsync(int id);

    Task<List<EncounterResponse>> GetByPatientIdAsync(int patientId);

    Task<EncounterResponse?> UpdateAsync(int id, UpdateEncounterRequest request);

    Task<EncounterResponse?> CompleteAsync(int id);
}