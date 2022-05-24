using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.API.Core.Entities
{
    public class Movie : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Rating { get; set; }
        public int? YearOfRelease { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; }

        public Movie()
        {
            CreatedDate = DateTimeOffset.Now;
            UpdatedDate = DateTimeOffset.Now;
        }
    }
}
