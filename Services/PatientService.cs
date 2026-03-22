using AutoMapper;
using PatientReferralManagementAPI.DTO.Common;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.Repositories;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagementAPI.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(
            IPatientRepository repo, 
            IMapper mapper,
            ILogger<PatientService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        // CREATE PATIENT
        public async Task<PatientResponseDto> CreateAsync(CreateUpdatePatientDto dto)
        {
            _logger.LogInformation("Creating patient");
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = DateTime.Parse(dto.DateOfBirth)
            };

            var created = await _repo.CreateAsync(patient);

            _logger.LogInformation("Patient created with ID {PatientId}", created.PatientId);

            return _mapper.Map<PatientResponseDto>(created);
        }

        //DELETE PATIENT
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Delete patient {PatientId}", id);

            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", id);
                throw new NotFoundException("Patient not found");
            }

            await _repo.DeleteAsync(patient);

            _logger.LogInformation("Patient deleted with ID {PatientId}", id);
        }


        // UPDATE PATIENT
        public async Task<PatientResponseDto> UpdateAsync(int id, CreateUpdatePatientDto dto)
        {
            _logger.LogInformation("Update patient {PatientId}", id);

            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", id);
                throw new NotFoundException("Patient not found");
            }

            // update field
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.DateOfBirth = DateTime.Parse(dto.DateOfBirth);

            var updated = await _repo.UpdateAsync(patient);

            _logger.LogInformation("Patient updated with ID {PatientId}", id);

            return _mapper.Map<PatientResponseDto>(updated);
        }

        // GET ALL (PAGINATION)
        public async Task<PagedResponse<PatientResponseDto>> GetAllAsync(int page, int size)
        {
            _logger.LogInformation("Fetching patients. Page: {Page}, Size: {Size}", page, size);

            var data = await _repo.GetAllAsync(page, size);
            var total = await _repo.CountAsync();

            _logger.LogInformation("Fetched {Count} patients from database (Total: {Total})", data.Count(), total);

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
            _logger.LogInformation("Fetching patient by ID: {PatientId}", id);

            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", id);
                throw new NotFoundException("Patient not found");
            }

            _logger.LogInformation("Patient found with ID: {PatientId}", id);

            return _mapper.Map<PatientResponseDto>(patient);
        }
    }
}