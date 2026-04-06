using AutoMapper;
using legendarios_API.Entity;
using legendarios_API.Models;
using legendarios_API.Repository;
using legendarios_API.Service;
using MercadoPago.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("legendarios")]
    public class LegendariosController : ControllerBase
    {

        private readonly ILogger<LegendariosController> _logger;
        private LegendariosService _LegendariosService = new LegendariosService();
        private readonly LoginService _LoginService;

        public LegendariosController(ILogger<LegendariosController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _LoginService = new LoginService(configuration);
        }

        [HttpPost("trazer")]
        public async Task<ActionResult<ResponseListDTO>> GetLegendarios([FromBody] LegendariosParams param)
        {
            var logado = _LoginService.VerificaSeEstaLogado(param.Id_Usuario.ToString());

            if (!logado)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = "ACESSO NÃO AUTORIZADO" };
            }

            var result = _LegendariosService.GetAllLegendarios(param);

            return Ok(result);
        }

        [HttpGet("trazer/{idlegendario}")]
        public async Task<ActionResult<ResponseOneDTO>> GetLegendarioById(string idlegendario)
        {
            var result = await _LegendariosService.GetLegendarioById(idlegendario);

            return Ok(result);
        }

        [HttpGet("trazer-all")]
        public async Task<ActionResult<ResponseListDTO>> GetLegendariosAll()
        {
            var result = await _LegendariosService.GetAllLegendariosAll();

            return Ok(result);
        }

        [HttpPut("salvar-legendario")]
        public async Task<ActionResult<ResponseOneDTO>> PutAtualizarLegendario([FromBody] LegendariosDTO legendario)
        {
            var result = await _LegendariosService.SalvarLegendarioById(legendario);

            return Ok(result);
        }

        [HttpGet("diretorio")]
        public async Task<ActionResult<string>> GetDiretorioAtual()
        {
            var Dir = Directory.GetCurrentDirectory();
            var cert = @$"{Dir}/certificate/certificado.pfx";
            return Dir + " --- " + cert;
        }

        [HttpGet("logado/{idUsuario}")]
        public async Task<ActionResult<ResponseOneDTO>> GetLogado(string idUsuario)
        {
            if (idUsuario == "undefined")
            {
                idUsuario = "0";
            }
            var logado = _LoginService.VerificaSeEstaLogado(idUsuario);

            var response = new ResponseOneDTO
            {
                Sucesso = logado
            };

            return Ok(response);
        }

    }
}
