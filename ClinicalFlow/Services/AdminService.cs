using ClinicalFlow.Data;
using ClinicalFlow.Dtos.Admin;
using ClinicalFlow.Enums;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public AdminService(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<DoctorResponse> CreateDoctorAsync(CreateDoctorRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _context.ApplicationUsers
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);

        if (emailExists)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var applicationUser = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            Email = email,
            Role = UserRole.Doctor,
            CreatedAt = DateTime.UtcNow
        };

        applicationUser.PasswordHash = _passwordHasher.HashPassword( applicationUser, request.Password);

        _context.ApplicationUsers.Add(applicationUser);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            ApplicationUserId = applicationUser.ApplicationUserId,
            FullName = request.FullName.Trim(),
            Email = email,
            Speciality = string.IsNullOrWhiteSpace(request.Speciality) ? null : request.Speciality.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return new DoctorResponse
        {
            DoctorId = doctor.DoctorId,
            ApplicationUserId = doctor.ApplicationUserId,
            FullName = doctor.FullName,
            Email = doctor.Email,
            Speciality = doctor.Speciality,
            CreatedAt = doctor.CreatedAt
        };
    }

    public async Task<List<DoctorResponse>> GetAllAsync()
    {
        return await _context.Doctors
            .AsNoTracking()
            .OrderBy(d => d.DoctorId)
            .Select(d => new DoctorResponse
            {
                DoctorId = d.DoctorId,
                ApplicationUserId = d.ApplicationUserId,
                FullName = d.FullName,
                Email = d.Email,
                Speciality = d.Speciality,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();
    }
}