using System;

namespace FilmSearchClasses.Dtos
{
    public class FilmFullData
    {
        public string Title { get; set; }
        public string imdbID { get; set; }
        public string Genre { get; set; }
        public string Plot { get; set; }
        public Uri Poster { get; set; }
    }
}
