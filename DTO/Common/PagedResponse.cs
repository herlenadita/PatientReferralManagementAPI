namespace PatientReferralManagementAPI.DTO.Common
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}
