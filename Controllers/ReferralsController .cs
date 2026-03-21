using Microsoft.AspNetCore.Mvc;
using PatientReferralManagementAPI.DTO.Common;
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
    }
}
