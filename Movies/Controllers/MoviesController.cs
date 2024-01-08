using System.Globalization;
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

    public MoviesController()
    {

    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(MovieList);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var movie = MovieList.FirstOrDefault(x=>x.Id == id);
        if(movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    [HttpGet("GetByIdQuery")]
    public IActionResult GetByIdQuery([FromQuery] int id)
    {
        var movie = MovieList.FirstOrDefault(x=>x.Id == id);
        if(movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    [HttpGet("GetByParameter")]
    public IActionResult GetByParameter([FromQuery] string? Title, [FromQuery] string? Language)
    {
        var movies = MovieList.AsQueryable();
        if(!string.IsNullOrEmpty(Title)){
            movies = movies.Where(x => x.Title.Contains(Title, StringComparison.OrdinalIgnoreCase));
        }
        if(!string.IsNullOrEmpty(Language)){
            movies = movies.Where(x => x.Language.Contains(Language, StringComparison.OrdinalIgnoreCase));
        }
        return Ok( movies.ToList<Movie>());
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

    [HttpGet]
    [Route("SortBy")]
    public IActionResult SortBy([FromQuery] string sorting)
    {
        var movie = MovieList.AsQueryable();

        if (!string.IsNullOrEmpty(sorting) && sorting.ToUpper(CultureInfo.InvariantCulture)== "TITLE")
        {
            movie = movie.OrderBy(x => x.Title);
        }
        
        else if (!string.IsNullOrEmpty(sorting) && sorting.ToUpper(CultureInfo.InvariantCulture) == "LANGUAGE")
        {
            movie = movie.OrderBy(x => x.Language);
        }

        else{
            return BadRequest();
        }
        return Ok(movie.ToList());
    }
    
}

