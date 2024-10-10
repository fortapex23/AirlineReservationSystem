namespace TravelProgram.MVC.ApiResponseMessages
{
    public class ApiResponseMessage<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public string PropertyName { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
