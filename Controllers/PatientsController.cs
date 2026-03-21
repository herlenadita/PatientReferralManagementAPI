using Microsoft.AspNetCore.Mvc;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.Services;

namespace PatientReferralManagementAPI.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        // CREATE PATIENT
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdatePatientDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = result.PatientId },
                new ApiResponse<PatientResponseDto>
                {
                    Success = true,
                    Message = "Patient created successfully",
                    Data = result
                });
        }

        // UPDATE PATIENT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUpdatePatientDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<PatientResponseDto>
            {
                Success = true,
                Message = "Patient updated successfully",
                Data = result
            });
        }

        //DELETE PATIENT
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Patient deleted successfully",
                Data = null
            });
        }

        // GET ALL (WITH PAGINATION)
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int size = 10)
        {
            var result = await _service.GetAllAsync(page, size);

            return Ok(new ApiResponse<PagedResponse<PatientResponseDto>>
            {
                Success = true,
                Message = "Patients retrieved successfully",
                Data = result
            });
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(new ApiResponse<PatientResponseDto>
            {
                Success = true,
                Message = "Patient retrieved successfully",
                Data = result
            });
        }
    }
}
