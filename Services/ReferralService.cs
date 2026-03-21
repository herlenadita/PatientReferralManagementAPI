using AutoMapper;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using System.Drawing;

namespace PatientReferralManagementAPI.Services
{
    public class ReferralService
    {
        private readonly IReferralRepository _repo;
        private readonly IPatientRepository _patientRepo;
        private readonly IMapper _mapper;

        public ReferralService(
            IReferralRepository repo,
            IPatientRepository patientRepo,
            IMapper mapper)
        {
            _repo = repo;
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        // CREATE REFERRAL
        public async Task<ReferralResponseDto> CreateAsync(CreateReferralDto dto)
        {
            // Business rule: patient must exist
            var patient = await _patientRepo.GetByIdAsync(dto.PatientId);
            if (patient == null)
                throw new Exception("Patient not found");

            var referral = new Referral
            {
                PatientId = dto.PatientId,
                ReferralSource = dto.ReferralSource,
                ReferralType = dto.ReferralType,
                ReferralNote = dto.ReferralNote,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(referral);

            return _mapper.Map<ReferralResponseDto>(created);
        }

        // GET BY ID
        public async Task<ReferralResponseDto> GetByIdAsync(int id)
        {
            var referral = await _repo.GetByIdAsync(id);

            if (referral == null)
                throw new Exception("Referral not found");

            return _mapper.Map<ReferralResponseDto>(referral);
        }

        // GET ALL (PAGINATION)
        public async Task<PagedResponse<ReferralResponseDto>> GetByPatientIdAsync(int patientId, int page, int size)
        {
            var data = await _repo.GetByPatientIdAsync(patientId, page, size);
            var total = await _repo.CountByPatientIdAsync(patientId);

            var mapped = _mapper.Map<IEnumerable<ReferralResponseDto>>(data);

            return new PagedResponse<ReferralResponseDto>
            {
                Data = mapped,
                Page = page,
                Size = size,
                Total = total
            };
        }
    }
}
