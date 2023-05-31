using Microsoft.AspNetCore.Mvc;
using StudyingTelegramBot.Services;
using StudyingTelegramBot.Models;

namespace StudyingTelegramBot.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase {
        private readonly ILogger<LessonController> _looger;
        private readonly LessonService _lessonService;

        public LessonController(ILogger<LessonController> looger, LessonService lessonService) {
            _looger = looger;
            _lessonService = lessonService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Lesson>> GetLessonById(Guid id) {
            var lesson = await _lessonService.GetLesson(id);

            if (lesson == null) {
                return NotFound();
            }

            return Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(Lesson lesson) {
            await _lessonService.CreateLesson(lesson);

            return CreatedAtRoute(new { id = lesson.Id }, lesson);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Lesson>> UpdateLesson(Guid id, Lesson lesson) {
            if (id != lesson.Id) {
                return BadRequest();
            }

            await _lessonService.UpdateLesson(lesson);

            return Ok(lesson);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(Guid id) {
            bool lessonDeleted = await _lessonService.DeleteLesson(id);

            if (!lessonDeleted) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
