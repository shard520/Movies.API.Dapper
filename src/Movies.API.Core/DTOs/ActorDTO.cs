using System.Diagnostics.CodeAnalysis;

namespace Movies.API.Core.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<MovieDTO> Movies { get; set; } = new List<MovieDTO>();
    }
}
