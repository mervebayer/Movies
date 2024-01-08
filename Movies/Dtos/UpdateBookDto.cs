namespace Movies.Dtos
{
    public class UpdateBookDto
    {
        public string Title {get; set;}
        public int GenreId {get; set;}
        public string Language {get; set;}
        public DateTime PublishDate {get; set;}
    }
}