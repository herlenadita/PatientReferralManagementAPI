using PatientReferralManagementAPI.Models;

namespace PatientReferralManagementAPI.Repositories
{
    public interface IReferralRepository
    {
        Task<Referral> CreateAsync(Referral referral);
        Task<Referral?> GetByIdAsync(int id);
        Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, int page, int size);
        Task<int> CountByPatientIdAsync(int patientId);
    }
}
