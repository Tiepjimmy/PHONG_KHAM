namespace ClinicManagement.Model.Response
{
    public class Response<T> : CommonResponse
    {
        public T? Data { get; set; }
    }
}
