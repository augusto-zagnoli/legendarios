using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IEventosRepository _eventosRepo;

        public DashboardController(IEventosRepository eventosRepo)
        {
            _eventosRepo = eventosRepo;
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = _eventosRepo.GetDashboardStats();
            return Ok(ApiResponse<DashboardDTO>.Ok(stats));
        }
    }
}
