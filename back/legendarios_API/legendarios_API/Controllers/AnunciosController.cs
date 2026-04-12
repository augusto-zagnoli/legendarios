using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("anuncios")]
    public class AnunciosController : ControllerBase
    {
        private readonly IAnunciosService _service;

        public AnunciosController(IAnunciosService service)
        {
            _service = service;
        }

        // Público — retorna só ativos
        [HttpGet]
        public ActionResult<AnuncioResponseListDTO> GetPublico()
        {
            return Ok(_service.GetTodos());
        }

        // ADM — retorna todos (incluindo inativos)
        [HttpGet("adm")]
        [Authorize]
        public ActionResult<AnuncioResponseListDTO> GetAdm()
        {
            return Ok(_service.GetTodosAdm());
        }

        [HttpPost]
        [Authorize]
        public ActionResult<AnuncioResponseOneDTO> Post([FromBody] AnuncioDTO dto)
        {
            dto.criado_por = User.Identity?.Name;
            return Ok(_service.Criar(dto));
        }

        [HttpPut]
        [Authorize]
        public ActionResult<AnuncioResponseOneDTO> Put([FromBody] AnuncioDTO dto)
        {
            dto.modificado_por = User.Identity?.Name;
            return Ok(_service.Atualizar(dto));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<AnuncioResponseOneDTO> Delete(int id)
        {
            return Ok(_service.Deletar(id));
        }
    }
}
