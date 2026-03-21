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
        public async Task<PatientResponseDto> CreateAsync(CreateUpdatePatientDto dto)
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

        //DELETE PATIENT
        public async Task DeleteAsync(int id)
        {
            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
                throw new Exception("Patient not found");

            await _repo.DeleteAsync(patient);
        }


        // UPDATE PATIENT
        public async Task<PatientResponseDto> UpdateAsync(int id, CreateUpdatePatientDto dto)
        {
            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
                throw new Exception("Patient not found");

            // update field
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.DateOfBirth = dto.DateOfBirth;

            var updated = await _repo.UpdateAsync(patient);

            return _mapper.Map<PatientResponseDto>(updated);
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