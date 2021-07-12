using FilmSearchClasses.Dtos;
using FilmSearchClasses.Dtos.SearchParams;
using FilmSearchClasses.Services;
using System.Threading;
using System.Threading.Tasks;

namespace FilmSearchClasses.ServiceInterfaces
{
    public interface IFilmSearchService
    {
        Task<ServiceResponse<SearchResult>> Search(SearchParam[] searchParams, int page, CancellationToken ct);
        Task<ServiceResponse<FilmFullData>> FullFilmData(string imdbID, CancellationToken ct);
    }
}
