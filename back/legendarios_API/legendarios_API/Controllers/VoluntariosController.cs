using legendarios_API.DTO;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/voluntarios")]
    public class VoluntariosController : ControllerBase
    {
        private readonly IVoluntariosService _service;

        public VoluntariosController(IVoluntariosService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetTodos()
        {
            return Ok(ApiResponse<object>.Ok(_service.GetTodos()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            if (result == null)
                return NotFound(ApiResponse<object>.Erro("Voluntário não encontrado."));
            return Ok(ApiResponse<VoluntarioResponseDTO>.Ok(result));
        }

        [Authorize]
        [HttpGet("paginado")]
        public IActionResult GetPaginado([FromQuery] PaginacaoParams param)
        {
            return Ok(_service.GetPaginado(param));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Criar([FromBody] VoluntarioRequestDTO dto)
        {
            var result = _service.Criar(dto);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public IActionResult Atualizar([FromBody] VoluntarioRequestDTO dto)
        {
            var result = _service.Atualizar(dto);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var result = _service.Deletar(id);
            if (!result.Sucesso)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
