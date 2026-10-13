namespace ClinicManagement.Data.Interface
{
    public interface IResult<T>
    {
        public T? Data { get; set; }
        public string? ResponseMessage { get; set; }
        public int ResponseCode { get; set; }
    }
}
