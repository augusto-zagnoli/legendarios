using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace legendarios_API.Service
{
    public class LoginService
    {
        private readonly IConfiguration _configuration;
        private readonly LoginRepository loginRepository = new LoginRepository();

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDTO RealizarLoguin(LoginDTO parans)
        {
            var usuario = loginRepository.GetUsuariosIdChave(parans.login, parans.password);

            if (usuario == null || usuario.id_usuario == 0)
                return null;

            var token = GerarToken(usuario, parans.rememberUser);

            loginRepository.AtualizaToken(usuario.id_usuario.ToString(), token);

            return new LoginResponseDTO
            {
                Token = token,
                IdUsuario = usuario.id_usuario,
                NomeUsuario = usuario.n_lgnd,
                NivelPermissao = usuario.nivel_permissao
            };
        }

        private string GerarToken(Usuarios usuario, bool rememberUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.id_usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.n_lgnd ?? ""),
                new Claim("nivel_permissao", usuario.nivel_permissao.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = rememberUser ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerificaSeEstaLogado(string id_usuario)
        {
            var tokens = loginRepository.GetTokens(id_usuario);
            var token = tokens.OrderByDescending(obj => obj.dt_acesso).FirstOrDefault();

            if (token == null) return false;

            TimeSpan diferenca = DateTime.Now - token.dt_acesso;
            return diferenca <= TimeSpan.FromMinutes(15);
        }
    }
}
