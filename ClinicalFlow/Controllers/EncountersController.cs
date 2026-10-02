
using ClinicalFlow.DTOs.Encounters;
using ClinicalFlow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EncountersController : ControllerBase
{
    private readonly IEncounterService _encounterService;

    public EncountersController(IEncounterService encounterService)
    {
        _encounterService = encounterService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEncounterRequest request)
    {
        try
        {
            var encounter = await _encounterService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = encounter.EncounterId },
                encounter);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var encounter = await _encounterService.GetByIdAsync(id);

        if (encounter is null)
        {
            return NotFound(new
            {
                message = $"Encounter {id} was not found."
            });
        }

        return Ok(encounter);
    }

    [HttpGet("patient/{patientId:int}")]
    public async Task<IActionResult> GetByPatientId(int patientId)
    {
        var encounters =
            await _encounterService.GetByPatientIdAsync(patientId);

        return Ok(encounters);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateEncounterRequest request)
    {
        try
        {
            var encounter =
                await _encounterService.UpdateAsync(id, request);

            if (encounter is null)
            {
                return NotFound(new
                {
                    message = $"Encounter {id} was not found."
                });
            }

            return Ok(encounter);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        try
        {
            var encounter =
                await _encounterService.CompleteAsync(id);

            if (encounter is null)
            {
                return NotFound(new
                {
                    message = $"Encounter {id} was not found."
                });
            }

            return Ok(encounter);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}