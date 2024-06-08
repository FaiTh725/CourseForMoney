namespace StudentPlacement.Backend.Domain.Response
{
    public class DataResponse<T> : BaseResponse 
    {
        public T Data { get; set; }
    }
}
