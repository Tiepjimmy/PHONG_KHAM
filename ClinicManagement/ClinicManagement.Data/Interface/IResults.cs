namespace ClinicManagement.Data.Interface
{
    public interface IResults<T>
    {
        public List<T> ListData { get; set; }
        public long TotalRecords { get; set; }
        public string ResponseMessage { get; set; }
        public int ResponseCode { get; set; }
    }
}
