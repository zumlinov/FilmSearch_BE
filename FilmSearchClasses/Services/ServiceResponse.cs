using System.Net;

namespace FilmSearchClasses.Services
{
    public class ServiceResponse<TResult>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public TResult Result { get; set; }
    }
}
