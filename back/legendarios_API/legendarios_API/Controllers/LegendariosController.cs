using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using legendarios_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("legendarios")]
    public class LegendariosController : ControllerBase
    {
        private readonly ILogger<LegendariosController> _logger;
        private readonly ILegendariosService _legendarioService;
        private readonly ILoginService _loginService;

        public LegendariosController(ILogger<LegendariosController> logger, ILegendariosService legendarioService, ILoginService loginService)
        {
            _logger = logger;
            _legendarioService = legendarioService;
            _loginService = loginService;
        }

        [HttpPost("trazer")]
        public ActionResult<ResponseListDTO> GetLegendarios([FromBody] LegendariosParams param)
        {
            var logado = _loginService.VerificaSeEstaLogado(param.Id_Usuario.ToString());

            if (!logado)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = "ACESSO NÃO AUTORIZADO" };
            }

            var result = _legendarioService.GetAllLegendarios(param);
            return Ok(result);
        }

        [HttpGet("trazer/{idlegendario}")]
        public ActionResult<ResponseOneDTO> GetLegendarioById(string idlegendario)
        {
            var result = _legendarioService.GetLegendarioById(idlegendario);
            return Ok(result);
        }

        [HttpGet("trazer-all")]
        public ActionResult<ResponseListDTO> GetLegendariosAll()
        {
            var result = _legendarioService.GetAllLegendariosAll();
            return Ok(result);
        }

        [HttpPut("salvar-legendario")]
        public ActionResult<ResponseOneDTO> PutAtualizarLegendario([FromBody] LegendariosDTO legendario)
        {
            var result = _legendarioService.SalvarLegendarioById(legendario);
            return Ok(result);
        }

        [HttpGet("logado/{idUsuario}")]
        public ActionResult<ResponseOneDTO> GetLogado(string idUsuario)
        {
            if (idUsuario == "undefined")
                idUsuario = "0";

            var logado = _loginService.VerificaSeEstaLogado(idUsuario);
            return Ok(new ResponseOneDTO { Sucesso = logado });
        }

        [HttpPost("cadastro-publico")]
        public ActionResult<ResponseOneDTO> PostCadastroPublico([FromBody] LegendariosDTO legendario)
        {
            var result = _legendarioService.CadastrarLegendario(legendario);
            return Ok(result);
        }

        [HttpGet("paginado")]
        public ActionResult GetLegendariosPaginado([FromQuery] PaginacaoParams param)
        {
            var result = _legendarioService.GetLegendariosPaginado(param);
            return Ok(result);
        }

        // ---- endpoints ADM ----

        [HttpGet("dashboard/estatisticas")]
        [Authorize]
        public ActionResult<ResponseListDTO> GetEstatisticasDashboard()
        {
            var result = _legendarioService.GetEstatisticasDashboard();
            return Ok(result);
        }

        [HttpGet("dashboard/por-status/{status}")]
        [Authorize]
        public ActionResult<ResponseListDTO> GetLegendariosPorStatus(string status)
        {
            var result = _legendarioService.GetLegendariosPorStatus(status);
            return Ok(result);
        }

        [HttpPatch("dashboard/status/{idLegendario}")]
        [Authorize]
        public ActionResult<ResponseOneDTO> PatchStatus(int idLegendario, [FromBody] AtualizarStatusDTO dto)
        {
            var result = _legendarioService.AtualizarStatusLegendario(idLegendario, dto.Status);
            return Ok(result);
        }
    }
}
