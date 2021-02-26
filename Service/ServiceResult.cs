using AddressBookApi.Enum;

namespace AddressBookApi.Service
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public ServiceResultType ServiceResultType { get; set; }
        public ErrorModel ErrorModel { get; set; }
    }

    public class ErrorModel
    {
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
