using ClinicalFlow.Data;
using ClinicalFlow.Enums;
using ClinicalFlow.Models;
using ClinicalFlow.DTOs.Encounters;
using ClinicalFlow.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Services;

public class EncounterService : IEncounterService
{
    private readonly ApplicationDbContext _context;

    public EncounterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EncounterResponse> CreateAsync(CreateEncounterRequest request)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.PatientId == request.PatientId);

        if (!patientExists)
        {
            throw new KeyNotFoundException($"Patient {request.PatientId} was not found.");
        }

        bool doctorExists = await _context.Doctors
            .AnyAsync(d => d.DoctorId == request.DoctorId);

        if (!doctorExists)
        {
            throw new KeyNotFoundException($"Doctor {request.DoctorId} was not found.");
        }

        var encounter = new Encounter
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ChiefComplaint = request.ChiefComplaint.Trim(),
            Diagnosis = request.Diagnosis?.Trim(),
            ClinicalNotes = request.ClinicalNotes?.Trim(),
            Status = EncounterStatus.InProgress,
            StartedAt = DateTime.UtcNow,
            CompletedAt = null
        };

        _context.Encounters.Add(encounter);

        await _context.SaveChangesAsync();

        return await GetByIdAsync(encounter.EncounterId) 
            ?? throw new InvalidOperationException("Encounter was created but could not be retrieved.");
    }

    public async Task<EncounterResponse?> GetByIdAsync(int id)
    {
        return await _context.Encounters
            .AsNoTracking()
            .Where(e => e.EncounterId == id)
            .Select(e => new EncounterResponse
            {
                EncounterId = e.EncounterId,
                PatientId = e.PatientId,
                PatientName = e.Patient.FirstName + " " + e.Patient.LastName,
                DoctorId = e.DoctorId,
                DoctorName = e.Doctor.FullName,
                ChiefComplaint = e.ChiefComplaint,
                Diagnosis = e.Diagnosis,
                ClinicalNotes = e.ClinicalNotes,
                Status = e.Status,
                StartedAt = e.StartedAt,
                CompletedAt = e.CompletedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<EncounterResponse>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Encounters
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.StartedAt)
            .Select(e => new EncounterResponse
            {
                EncounterId = e.EncounterId,
                PatientId = e.PatientId,
                PatientName = e.Patient.FirstName + " " + e.Patient.LastName,
                DoctorId = e.DoctorId,
                DoctorName = e.Doctor.FullName,
                ChiefComplaint = e.ChiefComplaint,
                Diagnosis = e.Diagnosis,
                ClinicalNotes = e.ClinicalNotes,
                Status = e.Status,
                StartedAt = e.StartedAt,
                CompletedAt = e.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<EncounterResponse?> UpdateAsync(int id, UpdateEncounterRequest request)
    {
        var encounter = await _context.Encounters
            .FirstOrDefaultAsync(e => e.EncounterId == id);

        if (encounter is null)
        {
            return null;
        }

        if (encounter.Status != EncounterStatus.InProgress)
        {
            throw new InvalidOperationException("Only in-progress encounters can be updated.");
        }

        encounter.ChiefComplaint = request.ChiefComplaint.Trim();
        encounter.Diagnosis = request.Diagnosis?.Trim();
        encounter.ClinicalNotes = request.ClinicalNotes?.Trim();

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<EncounterResponse?> CompleteAsync(int id)
    {
        var encounter = await _context.Encounters
            .FirstOrDefaultAsync(e => e.EncounterId == id);

        if (encounter is null)
        {
            return null;
        }
       
        if (encounter.Status != EncounterStatus.InProgress)
        {
            throw new InvalidOperationException("Only in-progress encounters can be completed.");
        }

        encounter.Status = EncounterStatus.Completed;
        encounter.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }
}