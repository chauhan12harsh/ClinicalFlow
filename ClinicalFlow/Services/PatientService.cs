
using ClinicalFlow.Data;
using ClinicalFlow.Models;
using ClinicalFlow.DTOs.Patients;
using ClinicalFlow.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Services;

public class PatientService : IPatientService
{
    private readonly ApplicationDbContext _context;

    public PatientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request)
    {
        string medicalRecordNumber =
            request.MedicalRecordNumber.Trim();

        bool exists = await _context.Patients.AnyAsync(
            p => p.MedicalRecordNumber == medicalRecordNumber);

        if (exists)
        {
            throw new InvalidOperationException(
                "A patient with this medical record number already exists.");
        }

        var patient = new Patient
        {
            MedicalRecordNumber = medicalRecordNumber,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Email = request.Email?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        return MapToResponse(patient);
    }

    public async Task<List<PatientResponse>> GetAllAsync()
    {
        return await _context.Patients
            .AsNoTracking()
            .OrderBy(p => p.PatientId)
            .Select(p => new PatientResponse
            {
                PatientId = p.PatientId,
                MedicalRecordNumber = p.MedicalRecordNumber,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PatientResponse?> GetByIdAsync(int id)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PatientId == id);

        return patient is null ? null : MapToResponse(patient);
    }

    public async Task<PatientResponse?> UpdateAsync(
        int id,
        UpdatePatientRequest request)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PatientId == id);

        if (patient is null)
        {
            return null;
        }

        patient.FirstName = request.FirstName.Trim();
        patient.LastName = request.LastName.Trim();
        patient.DateOfBirth = request.DateOfBirth;
        patient.Gender = request.Gender?.Trim();
        patient.PhoneNumber = request.PhoneNumber?.Trim();
        patient.Email = request.Email?.Trim();

        await _context.SaveChangesAsync();

        return MapToResponse(patient);
    }

    private static PatientResponse MapToResponse(Patient patient)
    {
        return new PatientResponse
        {
            PatientId = patient.PatientId,
            MedicalRecordNumber = patient.MedicalRecordNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            CreatedAt = patient.CreatedAt
        };
    }
}