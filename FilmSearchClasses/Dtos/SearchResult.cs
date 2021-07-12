using System.Linq;

namespace FilmSearchClasses.Dtos
{
    public class SearchResult
    {
        public SearchResult()
        {
            Films = Enumerable.Empty<FilmShortData>().ToArray();
        }

        public FilmShortData[] Films { get; set; }
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
