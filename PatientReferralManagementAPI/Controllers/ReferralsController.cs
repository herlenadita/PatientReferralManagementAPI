using Microsoft.AspNetCore.Mvc;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Services;

namespace PatientReferralManagementAPI.Controllers
{
    [ApiController]
    [Route("api/referrals")]
    public class ReferralsController : ControllerBase
    {
        private readonly ReferralService _service;

        public ReferralsController(ReferralService service)
        {
            _service = service;
        }

        // CREATE
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ReferralResponseDto>), 201)]
        public async Task<IActionResult> Create(CreateReferralDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(new ApiResponse<ReferralResponseDto>
            {
                Success = true,
                Message = "Referral created successfully",
                Data = result
            });
        }

        // GET BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ReferralResponseDto>), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(new ApiResponse<ReferralResponseDto>
            {
                Success = true,
                Message = "Referral retrieved successfully",
                Data = result
            });
        }

        // GET ALL 
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ReferralResponseDto>>), 200)]
        public async Task<IActionResult> GetAll(int page = 1, int size = 10)
        {
            var result = await _service.GetAllAsync(page, size);

            return Ok(new ApiResponse<PagedResponse<ReferralResponseDto>>
            {
                Success = true,
                Message = "Referrals retrieved successfully",
                Data = result
            });
        }

        // UPDATE
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ReferralResponseDto>), 200)]
        public async Task<IActionResult> Update(int id, UpdateReferralDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<ReferralResponseDto>
            {
                Success = true,
                Message = "Referral updated successfully",
                Data = result
            });
        }

        // DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Referral deleted successfully",
                Data = null
            });
        }
    }
}
