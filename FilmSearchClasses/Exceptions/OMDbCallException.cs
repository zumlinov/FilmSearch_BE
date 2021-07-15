using System;
using System.Net.Http;

namespace FilmSearchClasses.Exceptions
{
    public class OMDbCallException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; set; }

        public OMDbCallException(HttpResponseMessage responseMessage)
            : base($"{responseMessage.StatusCode}: {responseMessage.ReasonPhrase}")
        {
            ResponseMessage = responseMessage;
        }
    }
}
