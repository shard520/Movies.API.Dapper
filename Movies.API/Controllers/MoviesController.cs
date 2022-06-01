using Microsoft.AspNetCore.Mvc;
using Movies.API.Application.Interfaces;
using Movies.API.Core.Entities;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Movies.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Movies.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Movie movie)
        {
            var data = await _unitOfWork.Movies.AddAsync(movie);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Movies.DeleteAsync(id);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Movie movie)
        {
            var data = await _unitOfWork.Movies.UpdateAsync(movie);
            return Ok(data);
        }
    }
}
