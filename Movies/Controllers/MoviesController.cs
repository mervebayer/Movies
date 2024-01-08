using Microsoft.AspNetCore.Mvc;
using Movies.Dtos;

namespace Movies.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private static readonly List<Movie> MovieList = new(){
        new Movie{
            Id=1,
            Title="Lord Of The Rings",
            GenreId=1,
            Language="English",
            PublishDate = new DateTime(2000,01,01)
        },
        new Movie{
            Id=2,
            Title="StarWars",
            GenreId=1,
            Language="English",
            PublishDate = new DateTime(1980,03,04)
        },
        new Movie{
            Id=3,
            Title="Love101",
            GenreId=2,
            Language="Turkish",
            PublishDate = new DateTime(2010,05,03)
        }
    };

    private readonly ILogger<MoviesController> _logger;

    public MoviesController(ILogger<MoviesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public List<Movie> Get()
    {
        var movieList = MovieList.OrderBy(x=>x.Id).ToList<Movie>();
        return movieList;
    }

    [HttpGet("{id}")]
    public Movie GetById(int id)
    {
        var movie = MovieList.FirstOrDefault(x=>x.Id == id);
        return movie;
    }

    [HttpGet("GetByIdQuery")]
    public Movie GetByIdQuery([FromQuery] int id)
    {
        var movie = MovieList.FirstOrDefault(x=>x.Id == id);
        return movie;
    }

    [HttpGet("GetByParameter")]
    public List<Movie> GetByParameter([FromQuery] string? Title, [FromQuery] string? Language)
    {
        var movies = MovieList.AsQueryable();
        if(!string.IsNullOrEmpty(Title)){
            movies = movies.Where(x => x.Title.Contains(Title, StringComparison.OrdinalIgnoreCase));
        }
        if(!string.IsNullOrEmpty(Language)){
            movies = movies.Where(x => x.Language.Contains(Language, StringComparison.OrdinalIgnoreCase));
        }
        return movies.ToList<Movie>();
    }

    [HttpPost]
    public IActionResult AddMovie([FromBody] Movie movie){
        var movieCheck = MovieList.FirstOrDefault(x => x.Title == movie.Title);
        if(movieCheck != null){
            return BadRequest();
        }
        MovieList.Add(movie);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateBookDto movie){
       var movieCheck = MovieList.FirstOrDefault(x => x.Id == id);
        if(movieCheck == null){
            return BadRequest();
        }

        movieCheck.Title = movie.Title != default ? movie.Title : movieCheck.Title;
        movieCheck.GenreId = movie.GenreId != default ? movie.GenreId : movieCheck.GenreId;
        movieCheck.Language = movie.Language != default ? movie.Language : movieCheck.Language;
        movieCheck.PublishDate = movie.PublishDate != default ? movie.PublishDate : movieCheck.PublishDate;
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id){
        var movieCheck = MovieList.FirstOrDefault(x => x.Id == id);
        if(movieCheck == null){
            return BadRequest();
        }
        MovieList.Remove(movieCheck);
        return Ok();
    }
    
}

