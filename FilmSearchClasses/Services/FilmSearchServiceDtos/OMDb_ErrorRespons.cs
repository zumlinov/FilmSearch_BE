using Newtonsoft.Json;

namespace FilmSearchClasses.Services.FilmSearchServiceDtos
{
    public class OMDb_ErrorRespons
    {
        [JsonProperty(PropertyName = "Response")]
        public bool IsSuccessful { get; set; }
        [JsonProperty(PropertyName = "Error")]
        public string ErrorMessage { get; set; }
    }
}
