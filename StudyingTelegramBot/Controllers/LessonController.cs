using Microsoft.AspNetCore.Mvc;
using StudyingTelegramApi.Services;
using StudyingTelegramApi.Models;

namespace StudyingTelegramApi.Controllers {
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
        public async Task<ActionResult<Lesson?>> GetLessonById(Guid id) {
            var lesson = await _lessonService.GetLessonByIdAsync(id);

            if (lesson == null) {
                return NotFound();
            }

            return Ok(lesson);
        }

        [HttpGet]
        public async Task<ActionResult<Lesson?>> GetLessonByUserId([FromQuery] Guid userId) {
            var lessons = await _lessonService.GetLessonsByUserIdAsync(userId);

            if (lessons == null) {
                return NotFound();
            }

            return Ok(lessons);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(Lesson lesson) {
            await _lessonService.CreateLessonAsync(lesson);

            return CreatedAtRoute(new { id = lesson.Id }, lesson);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Lesson>> UpdateLesson(Guid id, Lesson lesson) {
            if (id != lesson.Id) {
                return BadRequest();
            }

            await _lessonService.UpdateLessonAsync(lesson);

            return Ok(lesson);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(Guid id) {
            bool lessonDeleted = await _lessonService.DeleteLessonAsync(id);

            if (!lessonDeleted) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
