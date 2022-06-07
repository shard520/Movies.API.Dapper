using Movies.API.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IMovieRepository movieRepository)
        {
            Movies = movieRepository;
        }
        public IMovieRepository Movies { get; }
    }
}
