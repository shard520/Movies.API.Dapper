namespace Movies.API.Core.Entities
{
    public class Actor : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
