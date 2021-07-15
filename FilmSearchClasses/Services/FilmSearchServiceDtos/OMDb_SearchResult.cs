using Newtonsoft.Json;

namespace FilmSearchClasses.Services.FilmSearchServiceDtos
{
    public class OMDb_SearchResult
    {
        public OMDb_FilmShortData[] Search { get; set; }
        public int TotalResults { get; set; }
        [JsonProperty(PropertyName = "Response")]
        public bool IsSuccessful { get; set; }
    }
}
