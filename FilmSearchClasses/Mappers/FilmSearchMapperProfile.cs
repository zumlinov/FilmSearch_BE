using AutoMapper;
using FilmSearchClasses.Dtos;
using FilmSearchClasses.Enums;
using FilmSearchClasses.Services.FilmSearchServiceDtos;
using System;

namespace FilmSearchClasses.Mappers
{
    public class FilmSearchMapperProfile : Profile
    {
        public FilmSearchMapperProfile()
        {
            CreateMap<OMDb_FilmShortData, FilmShortData>()
                .ForMember(x => x.Type, opt => opt.MapFrom(ParseMovieType))
                .ForMember(x => x.Poster, 
                    opt => opt.MapFrom(
                        x => Uri.IsWellFormedUriString(x.Poster, UriKind.Absolute) ? new Uri(x.Poster) : null));

            CreateMap<OMDb_SearchResult, SearchResult>()
                .ForMember(x => x.Films, opt => opt.MapFrom(x => x.Search))
                .ForMember(x => x.PagesCount, opt => opt.Ignore())
                .ForMember(x => x.CurrentPage, opt => opt.Ignore());

            CreateMap<OMDb_FilmFullData, FilmFullData>()
                .ForMember(x => x.Poster,
                    opt => opt.MapFrom(
                        x => Uri.IsWellFormedUriString(x.Poster, UriKind.Absolute) ? new Uri(x.Poster) : null));
        }

        private MovieType ParseMovieType(OMDb_FilmShortData omdb_FilmShortData, FilmShortData filmShortData)
        {
            MovieType movieType;
            return Enum.TryParse(omdb_FilmShortData.Type, out movieType) ? movieType : MovieType.unknown;
        }
    }
}
