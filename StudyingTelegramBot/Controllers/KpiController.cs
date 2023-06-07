using Microsoft.AspNetCore.Mvc;
using StudyingTelegramApi.Models;
using StudyingTelegramApi.Services;

namespace StudyingTelegramAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class KpiController : ControllerBase {
        private readonly ILogger<KpiService> _logger;
        private readonly KpiService _kpiService;

        public KpiController(ILogger<KpiService> logger, KpiService kpiService) {
            _logger = logger;
            _kpiService = kpiService;
        }

        [HttpGet]
        [Route("groups")]
        public async Task<ActionResult<List<Group>>> GetGroupsAsync() {
            var groupsList = await _kpiService.GetGroupsAsync();

            if (groupsList == null) {
                return BadRequest();
            }

            return Ok(groupsList);
        }

        [HttpGet]
        [Route("lessons")]
        public async Task<ActionResult<List<Pair>>> GetKpiScheduleAsync([FromQuery] Guid groupId) {
            var scheduleData = await _kpiService.GetKpiScheduleAsync(groupId);

            if (scheduleData == null) {
                return BadRequest();
            }

            var pairs = scheduleData.ScheduleFirstWeek.SelectMany(weekData => weekData.Pairs).ToList();

            return Ok(pairs);
        }
    }
}
