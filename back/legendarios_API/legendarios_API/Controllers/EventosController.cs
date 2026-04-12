using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/eventos")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _service;

        public EventosController(IEventosService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTodos([FromQuery] string status = null)
        {
            return Ok(ApiResponse<object>.Ok(_service.GetTodos(status)));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            if (result == null)
                return NotFound(ApiResponse<object>.Erro("Evento não encontrado."));
            return Ok(ApiResponse<EventoResponseDTO>.Ok(result));
        }

        [HttpGet("paginado")]
        public IActionResult GetPaginado([FromQuery] PaginacaoParams param, [FromQuery] string status = null)
        {
            return Ok(_service.GetPaginado(param, status));
        }

        [Authorize(Roles = "Admin,Lider")]
        [HttpPost]
        public IActionResult Criar([FromBody] EventoRequestDTO dto)
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var result = _service.Criar(dto, idUsuario);
            return Ok(ApiResponse<EventoResponseDTO>.Ok(result, "Evento criado com sucesso."));
        }

        [Authorize(Roles = "Admin,Lider")]
        [HttpPut]
        public IActionResult Atualizar([FromBody] EventoRequestDTO dto)
        {
            var result = _service.Atualizar(dto);
            if (!result)
                return NotFound(ApiResponse<object>.Erro("Evento não encontrado."));
            return Ok(ApiResponse<bool>.Ok(true, "Evento atualizado com sucesso."));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var result = _service.Deletar(id);
            if (!result)
                return NotFound(ApiResponse<object>.Erro("Evento não encontrado."));
            return Ok(ApiResponse<bool>.Ok(true, "Evento removido com sucesso."));
        }
    }
}
