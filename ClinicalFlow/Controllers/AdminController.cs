using ClinicalFlow.Dtos.Admin;
using ClinicalFlow.Enums;
using ClinicalFlow.Interfaces;
using ClinicalFlow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalFlow.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("createdoctor")]
    public async Task<IActionResult> CreateDoctor(CreateDoctorRequest request)
    {
        var doctor = await _adminService.CreateDoctorAsync(request);

        return Created($"/api/doctors/{doctor.DoctorId}",doctor);
    }

    [HttpGet("doctors")]    
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _adminService.GetAllAsync();
        return Ok(doctors);
    }

}