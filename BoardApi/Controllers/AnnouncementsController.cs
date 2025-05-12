using Microsoft.AspNetCore.Mvc;
using BoardApi.Models;
using BoardApi.Data;

namespace BoardApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly AnnouncementRepository _repository;

        public AnnouncementsController(AnnouncementRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var announcement = await _repository.GetByIdAsync(id);
            return announcement == null ? NotFound() : Ok(announcement);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            await _repository.AddAsync(announcement);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement announcement)
        {
            if (id != announcement.Id)
                return BadRequest();
            await _repository.UpdateAsync(announcement);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}

