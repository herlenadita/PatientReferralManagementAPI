using PatientReferralManagementAPI.Models;

namespace PatientReferralManagementAPI.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> CreateAsync(Patient patient);
        Task<Patient> UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task<Patient?> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> GetAllAsync(int page, int size);
        Task<int> CountAsync();

    }
}
