using Microsoft.AspNetCore.Mvc;
using StudyingTelegramApi.Models;
using StudyingTelegramApi.Services;

namespace StudyingTelegramApi.Controllers {
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

        [HttpGet]
        public async Task<ActionResult<Homework?>> GetHomeworkByUserId([FromQuery] Guid userId) {
            var homework = await _homeworkService.GetHomeworkByUserIdAsync(userId);

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
        public async Task<IActionResult> DeleteHomeworkAsync(Guid id) {
            bool homeworkDeleted = await _homeworkService.DeleteHomeworkAsync(id);

            if (!homeworkDeleted) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
