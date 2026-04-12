using legendarios_API.DTO;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ILoginService _loginService;

        public AuthController(ILogger<AuthController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO parans)
        {
            var result = _loginService.RealizarLogin(parans);
            if (result == null)
                return Unauthorized(ApiResponse<object>.Erro("Usuário ou senha inválidos."));

            return Ok(ApiResponse<AuthResponseDTO>.Ok(result));
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var result = _loginService.RefreshToken(request);
            if (result == null)
                return Unauthorized(ApiResponse<object>.Erro("Refresh token inválido ou expirado."));

            return Ok(ApiResponse<AuthResponseDTO>.Ok(result));
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RefreshTokenRequestDTO request)
        {
            _loginService.RevogarToken(request.RefreshToken);
            return Ok(ApiResponse<bool>.Ok(true, "Token revogado com sucesso."));
        }

        [Authorize]
        [HttpPost("usuarios")]
        public IActionResult CriarUsuario([FromBody] CriarUsuarioDTO dto)
        {
            var idCriador = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var (sucesso, erro) = _loginService.CriarUsuario(dto, idCriador);
            if (!sucesso)
                return BadRequest(ApiResponse<object>.Erro(erro));

            return Ok(ApiResponse<object>.Ok(null, "Usuário criado com sucesso."));
        }

        [Authorize]
        [HttpGet("usuarios")]
        public IActionResult ListarUsuarios()
        {
            var lista = _loginService.GetTodosUsuarios();
            return Ok(ApiResponse<object>.Ok(lista));
        }

        [Authorize]
        [HttpPut("usuarios")]
        public IActionResult AtualizarUsuario([FromBody] AtualizarUsuarioDTO dto)
        {
            var idEditor = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var (sucesso, erro) = _loginService.AtualizarUsuario(dto, idEditor);
            if (!sucesso)
                return BadRequest(ApiResponse<object>.Erro(erro));

            return Ok(ApiResponse<object>.Ok(null, "Usuário atualizado com sucesso."));
        }

        [Authorize]
        [HttpDelete("usuarios/{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var idDelecao = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            _loginService.DeletarUsuario(id, idDelecao);
            return Ok(ApiResponse<object>.Ok(null, "Usuário removido com sucesso."));
        }
    }
}
