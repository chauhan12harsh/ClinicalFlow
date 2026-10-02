
using ClinicalFlow.DTOs.Patients;
using ClinicalFlow.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePatientRequest request)
    {
        try
        {
            var patient = await _patientService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = patient.PatientId },
                patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientService.GetAllAsync();

        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient is null)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        return Ok(patient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePatientRequest request)
    {
        var patient = await _patientService.UpdateAsync(id, request);

        if (patient is null)
        {
            return NotFound(new
            {
                message = $"Patient with ID {id} was not found."
            });
        }

        return Ok(patient);
    }
}