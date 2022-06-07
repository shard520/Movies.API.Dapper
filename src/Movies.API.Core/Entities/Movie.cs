using System.Diagnostics.CodeAnalysis;

namespace Movies.API.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class Movie : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal? Rating { get; set; }
        public int? YearOfRelease { get; set; }

        public List<Actor> Actors { get; set; } = new List<Actor>();
    }
}
