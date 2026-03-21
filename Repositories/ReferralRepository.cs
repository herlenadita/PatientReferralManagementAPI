using Microsoft.EntityFrameworkCore;
using PatientReferralManagementAPI.Data;
using PatientReferralManagementAPI.Models;

namespace PatientReferralManagementAPI.Repositories
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly AppDbContext _context;

        public ReferralRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Referral> CreateAsync(Referral referral)
        {
            _context.Referrals.Add(referral);
            await _context.SaveChangesAsync();
            return referral;
        }

        public async Task<Referral?> GetByIdAsync(int id)
        {
            return await _context.Referrals.FindAsync(id);
        }

        public async Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, int page, int size)
        {
            return await _context.Referrals
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> CountByPatientIdAsync(int patientId)
        {
            return await _context.Referrals
                .CountAsync(x => x.PatientId == patientId);
        }
    }
}
