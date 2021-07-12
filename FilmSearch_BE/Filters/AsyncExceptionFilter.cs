using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FilmSearch_BE.Filters
{
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AsyncExceptionFilter> _logger;

        public AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogCritical(context.Exception, "This exception was handled by exception filter");

            context.Result = new ObjectResult("Admindistrator is waking up...") 
            { 
                StatusCode = (int)HttpStatusCode.InternalServerError 
            };

            return Task.CompletedTask;
        }
    }
}
