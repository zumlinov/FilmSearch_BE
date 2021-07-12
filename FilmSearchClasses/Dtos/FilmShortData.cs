using FilmSearchClasses.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FilmSearchClasses.Dtos
{
    public class FilmShortData
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string ImdbID { get; set; }
        public MovieType Type { get; set; }
        public Uri Poster { get; set; }
    }
}
