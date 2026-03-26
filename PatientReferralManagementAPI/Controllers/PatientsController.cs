using Microsoft.AspNetCore.Mvc;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Services;

namespace PatientReferralManagementAPI.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;
        private readonly ReferralService _referralService;

        public PatientsController(PatientService service, ReferralService referralService)
        {
            _service = service;
            _referralService = referralService;
        }

        // CREATE PATIENT
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), 201)]
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
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), 200)]
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
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
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
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<PatientResponseDto>>), 200)]
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
        [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), 200)]
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

        [HttpGet("{id}/referrals")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ReferralResponseDto>>), 200)]
        public async Task<IActionResult> GetReferralsByPatient(int id, int page = 1, int size = 10)
        {
            var result = await _referralService.GetByPatientIdAsync(id, page, size);

            return Ok(new ApiResponse<PagedResponse<ReferralResponseDto>>
            {
                Success = true,
                Message = "Referrals retrieved successfully",
                Data = result
            });
        }
    }
}
