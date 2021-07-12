using FilmSearchClasses.Dtos;
using FilmSearchClasses.Dtos.SearchParams;
using FilmSearchClasses.ServiceInterfaces;
using FilmSearchClasses.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FilmSearch_BE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmSearchController : ControllerBase
    {
        private readonly IFilmSearchService _filmSearchService;
        private readonly ILogger<FilmSearchController> _logger;

        public FilmSearchController(
            IFilmSearchService filmSearchService,
            ILogger<FilmSearchController> logger)
        {
            _filmSearchService = filmSearchService;
            _logger = logger;
        }

        /// <summary>
        /// Get full film data by imdb id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="fullFilmData"></param>
        /// <returns>Returns full film data or an error</returns>
        /// <response code="200">Full dta was found</response>
        /// <response code="400">Wrong parameters were sent</response>
        /// <response code="500">Something went wrong</response>
        [HttpGet("fullFilmData/{imdbId}")]
        [ProducesResponseType(typeof(FilmFullData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> FullFilmData([FromRoute]string imdbId)
        {
            var fullFilmResult = await _filmSearchService.FullFilmData(imdbId, HttpContext.RequestAborted);

            return PrepareResponse(fullFilmResult);
        }

        /// <summary>
        /// Search for films short data by params
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="search"></param>
        /// <returns>Returns films search result or an error</returns>
        /// <response code="200">Search was done successfully</response>
        /// <response code="400">Wrong parameters were sent</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost("/search/{page?}")]
        [ProducesResponseType(typeof(SearchResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Search([FromBody] SearchParam[] searchParams, [FromRoute] int? page)
        {
            var pageToSearch = 1;
            if (page != null && page > 0)
                pageToSearch = (int)page;

            var searchResult = await _filmSearchService.Search(
                searchParams,
                pageToSearch,
                HttpContext.RequestAborted);

            return PrepareResponse(searchResult);
        }

        private IActionResult PrepareResponse<TResult>(ServiceResponse<TResult> serviceResponse)
        {
            switch (serviceResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return Ok(serviceResponse.Result);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(serviceResponse.ErrorMessage);
                default:
                    return new StatusCodeResult((int)serviceResponse.StatusCode);
            }
        }
    }
}
