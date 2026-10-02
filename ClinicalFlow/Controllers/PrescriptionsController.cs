using ClinicalFlow.Dtos.Prescriptions;
using ClinicalFlow.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalFlow.Controllers;

[ApiController]
[Route("api/encounters/{encounterId:int}/prescriptions")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        int encounterId,
        [FromBody] CreatePrescriptionRequest request)
    {
        try
        {
            var result = await _prescriptionService.CreateAsync(
                encounterId, request);

            if (result is null)
                return NotFound(new { message = "Encounter not found." });

            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByEncounterId(int encounterId)
    {
        var prescriptions =
            await _prescriptionService.GetByEncounterIdAsync(encounterId);

        if (prescriptions is null)
        {
            return NotFound(new
            {
                message = "Encounter not found."
            });
        }

        return Ok(prescriptions);
    }
}
