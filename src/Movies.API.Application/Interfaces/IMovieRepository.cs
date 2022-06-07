using Movies.API.Core.DTOs;
using Movies.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.API.Application.Interfaces
{
    public interface IMovieRepository : IGenericRepository<MovieDTO>
    {
    }
}
