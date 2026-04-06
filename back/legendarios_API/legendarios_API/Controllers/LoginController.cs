using legendarios_API.DTO;
using legendarios_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using legendarios_API.Entity;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly LoginService _loginService;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _loginService = new LoginService(configuration);
        }

        [HttpPost("/adm-login")]
        public IActionResult Login([FromBody] LoginDTO parans)
        {
            try
            {
                var result = _loginService.RealizarLoguin(parans);

                if (result == null)
                    return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login");
                return StatusCode(500, new { mensagem = "Erro interno ao processar o login." });
            }
        }

        [Authorize]
        [HttpPost("/adm-usuarios")]
        public IActionResult CriarUsuario([FromBody] CriarUsuarioDTO dto)
        {
            try
            {
                var idCriador = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                var (sucesso, erro) = _loginService.CriarUsuario(dto, idCriador);

                if (!sucesso)
                    return BadRequest(new { mensagem = erro });

                return Ok(new { mensagem = "Usuário criado com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                return StatusCode(500, new { mensagem = "Erro interno ao criar usuário." });
            }
        }

        [Authorize]
        [HttpGet("/adm-usuarios")]
        public IActionResult ListarUsuarios()
        {
            try
            {
                var lista = _loginService.GetTodosUsuarios();
                return Ok(new { sucesso = true, data = lista });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários");
                return StatusCode(500, new { mensagem = "Erro interno ao listar usuários." });
            }
        }

        [Authorize]
        [HttpPut("/adm-usuarios")]
        public IActionResult AtualizarUsuario([FromBody] AtualizarUsuarioDTO dto)
        {
            try
            {
                var idEditor = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                var (sucesso, erro) = _loginService.AtualizarUsuario(dto, idEditor);

                if (!sucesso)
                    return BadRequest(new { mensagem = erro });

                return Ok(new { mensagem = "Usuário atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário");
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar usuário." });
            }
        }

        [Authorize]
        [HttpDelete("/adm-usuarios/{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            try
            {
                var idDelecao = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                _loginService.DeletarUsuario(id, idDelecao);
                return Ok(new { mensagem = "Usuário removido com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário");
                return StatusCode(500, new { mensagem = "Erro interno ao remover usuário." });
            }
        }
    }
}
