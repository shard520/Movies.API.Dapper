using Microsoft.AspNetCore.Mvc;
using Movies.API.Application.Interfaces;
using Movies.API.Core.DTOs;
using Movies.API.Core.Entities;
using Serilog;

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

        [HttpPost]
        public async Task<IActionResult> Add(MovieDTO movieDTO)
        {
            try
            {
                var data = await _unitOfWork.Movies.AddAsync(movieDTO);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Conflict($"A movie with the name \"{movieDTO.MovieName}\" already exists with an id of {ex.Message}. Please update the existing movie.");
                throw;
            }
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
                Log.Information("Movie with id {id} not found.", id);
                return NotFound($"Movie with id {id} not found.");
            }
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MovieDTO movieDTO)
        {
            var data = await _unitOfWork.Movies.UpdateAsync(movieDTO);
            if (data == null)
            {
                Log.Information("Update operation was unsuccesful, tried to update a movie with id: {id}", movieDTO.Id);
                return NotFound("Update operation was unsuccesful, please check the id supplied is valid.");
            }
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Movies.DeleteAsync(id);
            if (data == 0)
            {
                Log.Information("Delete operation was unsuccesful, tried to delete a movie with id: {id}", id);
                return NotFound("Delete operation was unsuccesful, please check the id supplied is valid.");
            }
            return Ok(data);
        }
    }
}
