using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;

namespace _10_Filmdatenbank.Application.Interfaces;

public interface ITmdbService
{
    Task<IEnumerable<SearchMovie>> SearchMoviesAsync(string query, string language = "de-DE");
    Task<Movie?> GetMovieDetailsAsync(int tmdbId, string language = "de-DE");
    
    Task<IEnumerable<SearchPerson>> SearchPersonsAsync(string query, string language = "de-DE");
    Task<Person?> GetPersonDetailsAsync(int tmdbId, string language = "de-DE");
    
    Task<IEnumerable<SearchCollection>> SearchCollectionsAsync(string query, string language = "de-DE");
    Task<Collection?> GetCollectionDetailsAsync(int tmdbId, string language = "de-DE");
    
    Task<IEnumerable<SearchCompany>> SearchCompaniesAsync(string query, string language = "de-DE");
    Task<TMDbLib.Objects.Companies.Company?> GetCompanyDetailsAsync(int tmdbId);
}
