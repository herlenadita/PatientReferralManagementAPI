using Microsoft.EntityFrameworkCore;
using PatientReferralManagementAPI.Models;
namespace PatientReferralManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Referral> Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.PatientId).HasColumnName("patient_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<Referral>(entity =>
            {
                entity.ToTable("referral");

                entity.HasKey(e => e.ReferralId);

                entity.Property(e => e.ReferralId).HasColumnName("referral_id");
                entity.Property(e => e.PatientId).HasColumnName("patient_id");
                entity.Property(e => e.ReferralSource).HasColumnName("referral_source");
                entity.Property(e => e.ReferralType).HasColumnName("referral_type");
                entity.Property(e => e.ReferralNote).HasColumnName("referral_note");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Referrals)
                      .HasForeignKey(e => e.PatientId);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditRules()
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    entry.Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedDate").IsModified = false;

                    entry.Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
                    entry.Property("UpdatedDate").IsModified = true;
                }
            }
        }

    }
}
