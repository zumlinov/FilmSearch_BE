using AutoMapper;
using FilmSearchClasses.Dtos;
using FilmSearchClasses.Dtos.SearchParams;
using FilmSearchClasses.Exceptions;
using FilmSearchClasses.Helpers;
using FilmSearchClasses.ServiceInterfaces;
using FilmSearchClasses.Services.FilmSearchServiceDtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FilmSearchClasses.Services
{
    internal class FilmSearchService : IFilmSearchService
    {
        private const int FilmsPerPage = 10;
        OpenMovieDtabaseSettings _settings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public FilmSearchService(
            IOptions<OpenMovieDtabaseSettings> settings,
            IHttpClientFactory clientFactory,
            IMapper mapper)
        {
            _clientFactory = clientFactory;
            _settings = settings?.Value;
            _mapper = mapper;

            if (!IsValidSettings(_settings))
                throw new ArgumentNullException("Films search service settings is not valid");
        }

        public async Task<ServiceResponse<FilmFullData>> FullFilmData(string imdbID, CancellationToken ct)
        {
            var requestParam = new Dictionary<string, string>();
            requestParam.Add("i", imdbID);

            var fullFilmStrContent = await CallOpenFilmDb(requestParam, ct);
            var errorCheck = IsErrorRespons(fullFilmStrContent);
            if (errorCheck.isError)
            {
                return new ServiceResponse<FilmFullData>
                {
                    ErrorMessage = errorCheck.errMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = null
                };
            }

            var omdb_FilmFullData = JsonConvert.DeserializeObject<OMDb_FilmFullData>(fullFilmStrContent);

            return new ServiceResponse<FilmFullData>
            {
                Result = _mapper.Map<FilmFullData>(omdb_FilmFullData),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<SearchResult>> Search(SearchParam[] searchParams, int page, CancellationToken ct)
        {
            if (!searchParams.Any())
                return new ServiceResponse<SearchResult>
                {
                    ErrorMessage = "Search params list can't be null",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = null
                };

            var preparedSearchParams = SearchParamConverter.PrepareSearchParams(searchParams);
            preparedSearchParams.Add("page", page.ToString());

            var strContent = await CallOpenFilmDb(preparedSearchParams, ct);

            var errorCheck = IsErrorRespons(strContent);
            if (errorCheck.isError)
                return new ServiceResponse<SearchResult>
                {
                    ErrorMessage = errorCheck.errMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = null
                };

            var omdb_searchResult = JsonConvert.DeserializeObject<OMDb_SearchResult>(strContent);
            if (omdb_searchResult == null)
                throw new ArgumentException($"Search result deserialization error.\nContent:{omdb_searchResult}");

            var searchResult = _mapper.Map<SearchResult>(omdb_searchResult);
            searchResult.CurrentPage = page;
            searchResult.PagesCount = omdb_searchResult.TotalResults / FilmsPerPage + 1;
            searchResult.Films = searchResult.Films.OrderByDescending(f => f.Year).ToArray();

            return new ServiceResponse<SearchResult>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = searchResult
            };
        }

        private (bool isError, string errMessage) IsErrorRespons(string response)
        {
            var errCheckResult = JsonConvert.DeserializeObject<OMDb_ErrorRespons>(response);
            return !errCheckResult.IsSuccessful && !string.IsNullOrEmpty(errCheckResult.ErrorMessage)
                ? (true, errCheckResult.ErrorMessage)
                : (false, string.Empty);
        }

        private bool IsValidSettings(OpenMovieDtabaseSettings settings)
        {
            return !string.IsNullOrEmpty(settings?.BaseUrl) && !string.IsNullOrEmpty(settings?.Apikey);
        }

        private async Task<string> CallOpenFilmDb(Dictionary<string, string> parameters, CancellationToken ct)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["apikey"] = _settings.Apikey;

            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }

            var builder = new UriBuilder(_settings.BaseUrl);
            builder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                throw new OMDbCallException(response);
            }

            var strContent =  await response.Content.ReadAsStringAsync();
            return strContent;
        }

    }
}
