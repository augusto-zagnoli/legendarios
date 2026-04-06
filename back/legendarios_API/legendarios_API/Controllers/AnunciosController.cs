using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("anuncios")]
    public class AnunciosController : ControllerBase
    {
        private readonly AnunciosService _service;

        public AnunciosController(IConfiguration configuration)
        {
            _service = new AnunciosService(configuration);
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
            return Ok(_service.Criar(dto));
        }

        [HttpPut]
        [Authorize]
        public ActionResult<AnuncioResponseOneDTO> Put([FromBody] AnuncioDTO dto)
        {
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
