using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Domain.Response
{
    public class BaseResponse
    {
        public string Description {  get; set; }

        public StatusCode StatusCode { get; set; }
    }
}
