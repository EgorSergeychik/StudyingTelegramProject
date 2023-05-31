using Microsoft.AspNetCore.Mvc;
using StudyingTelegramBot.Models;
using StudyingTelegramBot.Services;

namespace StudyingTelegramBot.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkController : ControllerBase {
        private readonly ILogger<HomeworkController> _logger;
        private readonly HomeworkService _homeworkService;

        public HomeworkController(ILogger<HomeworkController> logger, HomeworkService homeworkService) {
            _logger = logger;
            _homeworkService = homeworkService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Homework>> GetHomeworkByIdAsync(Guid id) {
            var homework = await _homeworkService.GetHomeworkAsync(id);

            if (homework == null) {
                return NotFound();
            }

            return Ok(homework);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomeworkAsync(Homework homework) {
            await _homeworkService.CreateHomeworkAsync(homework);

            return CreatedAtRoute(new { id = homework.Id }, homework);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Homework>> UpdateHomeworkAsync(Guid id, Homework homework) {
            if (id != homework.Id) {
                return BadRequest();
            }

            await _homeworkService.UpdateHomeworkAsync(homework);

            return Ok(homework);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id) {
            bool homeworkDeleted = await _homeworkService.DeleteHomeworkAsync(id);

            if (!homeworkDeleted) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
