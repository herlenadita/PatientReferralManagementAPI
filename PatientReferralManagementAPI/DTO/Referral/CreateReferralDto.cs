namespace PatientReferralManagementAPI.DTO.Referral
{
    public class CreateReferralDto
    {
        public int PatientId { get; set; }
        public string ReferralSource { get; set; }
        public string ReferralType { get; set; }
        public string ReferralNote { get; set; }
    }
}
