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

                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Referrals)
                      .HasForeignKey(e => e.PatientId);
            });
        }

    }
}
