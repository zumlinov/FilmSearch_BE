using FilmSearchClasses.Dtos.SearchParams;
using FilmSearchClasses.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch_BE.Binders
{
    public class FilmSearchCriteriaModelBinder : IModelBinder
    {
        const string SearchCriteriaDetectionField = nameof(TitleSearchParam.CriteriaType);

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ModelStateDictionary modelState = null;
            try
            {
                if (bindingContext == null)
                    throw new ArgumentNullException(nameof(bindingContext));

                modelState = bindingContext.ModelState;

                var bodyStr = await ReadRequestString(bindingContext.HttpContext);
                if (string.IsNullOrEmpty(bodyStr))
                    throw new ArgumentException("Bodies string is empty");

                var requestBody = JsonConvert.DeserializeObject(bodyStr);
                var searchParamJObjects = GetJObjectSearchParams(requestBody);
                var tradeCaptureAssociationCommands = ParseSearchParams(searchParamJObjects);

                bindingContext.Result = ModelBindingResult.Success(tradeCaptureAssociationCommands.ToArray());
            }
            catch (Exception ex)
            {
                modelState?.TryAddModelError(nameof(SearchParam), ex.Message);
            }
        }

        private async Task<string> ReadRequestString(HttpContext httpContext)
        {
            var httpRequest = httpContext.Request;
            var bodyString = string.Empty;

            using (var sr = new StreamReader(httpRequest.Body))
                bodyString = await sr.ReadToEndAsync();

            return bodyString;
        }

        private JObject[] GetJObjectSearchParams(object bodyModel)
        {
            var searchParamJObjects = (bodyModel as JArray)?.Children().Cast<JObject>().ToArray();
            if (searchParamJObjects == null)
                throw new ArgumentException("Can't get search params from request");

            return searchParamJObjects;
        }

        private IEnumerable<SearchParam> ParseSearchParams(JObject[] jobjectCommands)
        {
            var searchParams = new List<SearchParam>();

            foreach (var jobjectCommand in jobjectCommands)
            {
                var commandType = jobjectCommand.Children().First(c => (c as JProperty)?.Name == SearchCriteriaDetectionField);
                if (commandType == null)
                    throw new ArgumentException($"Can't extract {SearchCriteriaDetectionField} field");

                SearchCriteriaType criteriaType;
                if (!Enum.TryParse((commandType as JProperty).Value.ToString(), out criteriaType))
                    throw new ArgumentException($"Unsuportd search criteria type: {(commandType as JProperty).Value}");

                switch (criteriaType)
                {
                    case SearchCriteriaType.Title:
                        searchParams.Add(jobjectCommand.ToObject<TitleSearchParam>());
                        break;
                    case SearchCriteriaType.MovieType:
                        searchParams.Add(jobjectCommand.ToObject<MovieTypeSearchParam>());
                        break;
                    case SearchCriteriaType.YearOfReleased:
                        searchParams.Add(jobjectCommand.ToObject<YearOfReleaseSearchParam>());
                        break;
                    default:
                        break;
                }
            }

            return searchParams;
        }
    }
}
