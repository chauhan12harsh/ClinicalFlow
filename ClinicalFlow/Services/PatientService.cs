
using ClinicalFlow.Data;
using ClinicalFlow.DTOs.Patients;
using ClinicalFlow.Enums;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Services;

public class PatientService : IPatientService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    public PatientService(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();
        string medicalRecordNumber = request.MedicalRecordNumber.Trim();

        bool emailExists = await _context.ApplicationUsers
            .AnyAsync(u => u.Email == email);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        bool medicalRecordNumberExists = await _context.Patients
            .AnyAsync(p => p.MedicalRecordNumber == medicalRecordNumber);

        if (medicalRecordNumberExists)
        {
            throw new InvalidOperationException(
                "A patient with this medical record number already exists.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var applicationUser = new ApplicationUser
            {
                Email = email,
                Role = UserRole.Patient,
                CreatedAt = DateTime.UtcNow
            };

            applicationUser.PasswordHash =
                _passwordHasher.HashPassword(
                    applicationUser,
                    request.Password);

            _context.ApplicationUsers.Add(applicationUser);

            var patient = new Patient
            {
                ApplicationUser = applicationUser,
                MedicalRecordNumber = medicalRecordNumber,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender?.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                Email = email?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return MapToResponse(patient);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    public async Task<PatientResponse?> UpdateAsync(int id, UpdatePatientRequest request)
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