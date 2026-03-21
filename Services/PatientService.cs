using AutoMapper;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;

namespace PatientReferralManagementAPI.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // CREATE PATIENT
        public async Task<PatientResponseDto> CreateAsync(CreatePatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth
            };

            var created = await _repo.CreateAsync(patient);

            return _mapper.Map<PatientResponseDto>(created);
        }

        // GET ALL (PAGINATION)
        public async Task<PagedResponse<PatientResponseDto>> GetAllAsync(int page, int size)
        {
            var data = await _repo.GetAllAsync(page, size);
            var total = await _repo.CountAsync();

            var mapped = _mapper.Map<IEnumerable<PatientResponseDto>>(data);

            return new PagedResponse<PatientResponseDto>
            {
                Data = mapped,
                Page = page,
                Size = size,
                Total = total
            };
        }

        // GET BY ID
        public async Task<PatientResponseDto> GetByIdAsync(int id)
        {
            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
                throw new Exception("Patient not found");

            return _mapper.Map<PatientResponseDto>(patient);
        }
    }
}