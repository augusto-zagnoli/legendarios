using legendarios_API.DTO;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/checkins")]
    public class CheckinsController : ControllerBase
    {
        private readonly ICheckinService _service;

        public CheckinsController(ICheckinService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("evento/{idEvento}")]
        public IActionResult GetByEvento(int idEvento)
        {
            return Ok(ApiResponse<object>.Ok(_service.GetByEvento(idEvento)));
        }

        [Authorize(Roles = "Admin,Lider,Voluntario")]
        [HttpPost]
        public IActionResult RegistrarCheckin([FromBody] CheckinRequestDTO dto)
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var result = _service.RegistrarCheckin(dto, idUsuario);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Lider,Voluntario")]
        [HttpPatch("{id}/checkout")]
        public IActionResult RegistrarCheckout(int id)
        {
            var result = _service.RegistrarCheckout(id);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
