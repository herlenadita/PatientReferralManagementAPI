using AutoMapper;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;

namespace PatientReferralManagementAPI.Services
{
    public class ReferralService
    {
        private readonly IReferralRepository _repo;
        private readonly IPatientRepository _patientRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ReferralService> _logger;

        public ReferralService(
            IReferralRepository repo,
            IPatientRepository patientRepo,
            IMapper mapper,
            ILogger<ReferralService> logger)
        {
            _repo = repo;
            _patientRepo = patientRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE REFERRAL
        public async Task<ReferralResponseDto> CreateAsync(CreateReferralDto dto)
        {
            _logger.LogInformation("Creating referral for patient {PatientId}", dto.PatientId);

            // Business rule: patient must exist
            var patient = await _patientRepo.GetByIdAsync(dto.PatientId);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", dto.PatientId);
                throw new Exception("Patient not found");
            }

            var referral = new Referral
            {
                PatientId = dto.PatientId,
                ReferralSource = dto.ReferralSource,
                ReferralType = dto.ReferralType,
                ReferralNote = dto.ReferralNote,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(referral);

            _logger.LogInformation("Referral created with ID {ReferralId}", created.ReferralId);

            return _mapper.Map<ReferralResponseDto>(created);
        }

        // GET BY ID
        public async Task<ReferralResponseDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching referral by ID: {ReferralId}", id);

            var referral = await _repo.GetByIdAsync(id);

            if (referral == null)
            {
                _logger.LogWarning("Referral not found with ID: {ReferralId}", id);
                throw new Exception("Referral not found");
            }

            _logger.LogInformation("Referral found with ID: {ReferralId}", id);

            return _mapper.Map<ReferralResponseDto>(referral);
        }

        // GET ALL (PAGINATION)
        public async Task<PagedResponse<ReferralResponseDto>> GetByPatientIdAsync(int patientId, int page, int size)
        {
            _logger.LogInformation(
                "Fetching referrals for PatientId: {PatientId}, Page: {Page}, Size: {Size}",
                patientId, page, size);

            var data = await _repo.GetByPatientIdAsync(patientId, page, size);
            var total = await _repo.CountByPatientIdAsync(patientId);

            _logger.LogInformation("Fetched {Count} patients from database (Total: {Total})", data.Count(), total);

            var mapped = _mapper.Map<IEnumerable<ReferralResponseDto>>(data);

            return new PagedResponse<ReferralResponseDto>
            {
                Data = mapped,
                Page = page,
                Size = size,
                Total = total
            };
        }

        // GET ALL + PAGINATION
        public async Task<PagedResponse<ReferralResponseDto>> GetAllAsync(int page, int size)
        {
            _logger.LogInformation("Fetching all referrals. Page: {Page}, Size: {Size}", page, size);

            var data = await _repo.GetAllAsync(page, size);
            var total = await _repo.CountAsync();

            _logger.LogInformation("Fetching all referrals. Page: {Page}, Size: {Size}", page, size);

            var mapped = _mapper.Map<IEnumerable<ReferralResponseDto>>(data);

            return new PagedResponse<ReferralResponseDto>
            {
                Data = mapped,
                Page = page,
                Size = size,
                Total = total
            };
        }

        //UPDATE REFERRAL
        public async Task<ReferralResponseDto> UpdateAsync(int id, UpdateReferralDto dto)
        {
            _logger.LogInformation("Updating referral with ID: {ReferralId}", id);

            var referral = await _repo.GetByIdAsync(id);

            if (referral == null)
            {
                _logger.LogWarning("Referral not found for update. ID: {ReferralId}", id);
                throw new Exception("Referral not found");
            }

            referral.ReferralSource = dto.ReferralSource;
            referral.ReferralType = dto.ReferralType;
            referral.ReferralNote = dto.ReferralNote;

            var updated = await _repo.UpdateAsync(referral);

            _logger.LogInformation("Referral updated successfully. ID: {ReferralId}", id);

            return _mapper.Map<ReferralResponseDto>(updated);
        }

        // DELETE REFERRAL
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Referral updated successfully. ID: {ReferralId}", id);

            var referral = await _repo.GetByIdAsync(id);

            if (referral == null)
            {
                _logger.LogWarning("Referral not found for deletion. ID: {ReferralId}", id);
                throw new Exception("Referral not found");
            }

            await _repo.DeleteAsync(referral);

            _logger.LogInformation("Referral deleted successfully. ID: {ReferralId}", id);
        }

    }
}
