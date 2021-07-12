using FilmSearchClasses.Enums;

namespace FilmSearchClasses.Dtos.SearchParams
{
    public class MovieTypeSearchParam :SearchParam
    {
        public MovieTypeSearchParam()
        {
            CriteriaType = SearchCriteriaType.MovieType;
        }

        public MovieType Type { get; set; }
    }
}
