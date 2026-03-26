namespace PatientReferralManagementAPI.DTO.Patient
{
    public class PatientResponseDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
    }
}