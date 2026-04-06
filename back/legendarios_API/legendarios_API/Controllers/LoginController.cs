using legendarios_API.DTO;
using legendarios_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

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
    }
}
