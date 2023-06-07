using Microsoft.AspNetCore.Mvc;
using StudyingTelegramApi.Controllers;
using StudyingTelegramApi.Models;
using StudyingTelegramApi.Services;

namespace StudyingTelegramApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        
        public UserController(ILogger<UserController> logger, UserService userService) {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(Guid id) {
            var user = await _userService.GetUserAsync(id);

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<User?>> GetUserByTelegramIdAsync([FromQuery] long telegramId) {
            User? user = await _userService.GetUserByFieldAsync("TelegramId", telegramId);

            if (user == null) {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(User user) {
            bool userExists = await _userService.GetUserByFieldAsync("TelegramId", user.TelegramId) != null;
            if (userExists) {
                return Conflict("User already exists.");
            }

            await _userService.CreateUserAsync(user);

            return CreatedAtRoute(new { id = user.Id }, user);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<User>> UpdateUserAsync(Guid id, User user) {
            if (id != user.Id) {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(user);

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id) {
            bool userDeleted = await _userService.DeleteUserAsync(id);

            if (!userDeleted) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
