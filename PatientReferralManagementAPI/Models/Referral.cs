namespace PatientReferralManagementAPI.Models
{
    public class Referral
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public string ReferralSource { get; set; }
        public string ReferralType { get; set; }
        public string ReferralNote { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; }
    }
}
