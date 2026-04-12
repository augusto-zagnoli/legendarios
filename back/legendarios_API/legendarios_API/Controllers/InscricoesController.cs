using legendarios_API.DTO;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/inscricoes")]
    public class InscricoesController : ControllerBase
    {
        private readonly IInscricoesService _service;

        public InscricoesController(IInscricoesService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("evento/{idEvento}")]
        public IActionResult GetByEvento(int idEvento)
        {
            return Ok(ApiResponse<object>.Ok(_service.GetByEvento(idEvento)));
        }

        [Authorize]
        [HttpGet("legendario/{idLegendario}")]
        public IActionResult GetByLegendario(int idLegendario)
        {
            return Ok(ApiResponse<object>.Ok(_service.GetByLegendario(idLegendario)));
        }

        [HttpPost]
        public IActionResult Inscrever([FromBody] InscricaoRequestDTO dto)
        {
            int? inscritoPor = null;
            var subClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(subClaim))
                inscritoPor = int.Parse(subClaim);

            var result = _service.Inscrever(dto, inscritoPor);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Lider")]
        [HttpPatch("{id}/status")]
        public IActionResult AtualizarStatus(int id, [FromBody] AtualizarStatusDTO dto)
        {
            var result = _service.AtualizarStatus(id, dto.Status);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Cancelar(int id)
        {
            var result = _service.Cancelar(id);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
