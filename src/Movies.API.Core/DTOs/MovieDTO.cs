using System.Diagnostics.CodeAnalysis;

namespace Movies.API.Core.DTOs
{
    [ExcludeFromCodeCoverage]
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal? Rating { get; set; }
        public int? YearOfRelease { get; set; }

        public List<ActorDTO> Actors { get; set; } = new List<ActorDTO>();
    }
}
