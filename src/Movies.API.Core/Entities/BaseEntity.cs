using System.Diagnostics.CodeAnalysis;

namespace Movies.API.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
