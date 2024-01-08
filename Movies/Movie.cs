using System.Runtime.CompilerServices;

namespace Movies;

public class Movie
{
    public int Id{get; set;}
    public string Title {get; set;}
    public int GenreId {get; set;}
    public string Language {get; set;}
    public DateTime PublishDate {get; set;}
}
