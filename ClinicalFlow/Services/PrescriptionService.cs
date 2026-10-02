using ClinicalFlow.Data;
using ClinicalFlow.Dtos.Prescriptions;
using ClinicalFlow.Enums;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicalFlow.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly ApplicationDbContext _context;

    public PrescriptionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PrescriptionResponse?> CreateAsync(
        int encounterId,
        CreatePrescriptionRequest request)
    {
        if (request.Medications is null || request.Medications.Count == 0)
        {
            throw new ArgumentException(
                "At least one medication is required.");
        }

        // Check the parent record before starting the write transaction.
        var encounter = await _context.Encounters
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EncounterId == encounterId);

        if (encounter is null)
            return null;

        if (encounter.Status != EncounterStatus.InProgress)
        {
            throw new InvalidOperationException(
                "Prescriptions can only be added to in-progress encounters.");
        }

        // Validate values that may also be supplied outside model validation.
        foreach (var medication in request.Medications)
        {
            if (string.IsNullOrWhiteSpace(medication.MedicationName) ||
                string.IsNullOrWhiteSpace(medication.Dosage) ||
                string.IsNullOrWhiteSpace(medication.Frequency))
            {
                throw new ArgumentException(
                    "Medication name, dosage, and frequency are required.");
            }

            if (medication.DurationDays <= 0 || medication.Quantity <= 0)
            {
                throw new ArgumentException(
                    "Medication duration and quantity must be greater than zero.");
            }
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var prescription = new Prescription
            {
                EncounterId = encounterId,
                CreatedAt = DateTime.UtcNow,
                Medications = request.Medications.Select(m =>
                    new PrescriptionMedication
                    {
                        MedicationName = m.MedicationName.Trim(),
                        Dosage = m.Dosage.Trim(),
                        Frequency = m.Frequency.Trim(),
                        DurationDays = m.DurationDays,
                        Quantity = m.Quantity,
                        Instructions = string.IsNullOrWhiteSpace(m.Instructions)
                            ? null
                            : m.Instructions.Trim()
                    }).ToList()
            };

            _context.Prescriptions.Add(prescription);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new PrescriptionResponse
            {
                PrescriptionId = prescription.PrescriptionId,
                EncounterId = prescription.EncounterId,
                CreatedAt = prescription.CreatedAt,
                Medications = prescription.Medications.Select(m =>
                    new PrescriptionMedicationResponse
                    {
                        PrescriptionMedicationId =
                            m.PrescriptionMedicationId,
                        MedicationName = m.MedicationName,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency,
                        DurationDays = m.DurationDays,
                        Quantity = m.Quantity,
                        Instructions = m.Instructions
                    }).ToList()
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<PrescriptionResponse>?> GetByEncounterIdAsync(
    int encounterId)
    {
        var encounterExists = await _context.Encounters
            .AnyAsync(e => e.EncounterId == encounterId);

        if (!encounterExists)
            return null;

        return await _context.Prescriptions
            .AsNoTracking()
            .Where(p => p.EncounterId == encounterId)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new PrescriptionResponse
            {
                PrescriptionId = p.PrescriptionId,
                EncounterId = p.EncounterId,
                CreatedAt = p.CreatedAt,

                Medications = p.Medications
                    .Select(m => new PrescriptionMedicationResponse
                    {
                        PrescriptionMedicationId =
                            m.PrescriptionMedicationId,
                        MedicationName = m.MedicationName,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency,
                        DurationDays = m.DurationDays,
                        Quantity = m.Quantity,
                        Instructions = m.Instructions
                    })
                    .ToList()
            })
            .ToListAsync();
    }
}
