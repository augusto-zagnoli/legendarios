using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Helpers;
using legendarios_API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace legendarios_API.Service
{
    public class LoginServiceV2 : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginRepository _loginRepository;

        public LoginServiceV2(IConfiguration configuration, ILoginRepository loginRepository)
        {
            _configuration = configuration;
            _loginRepository = loginRepository;
        }

        public AuthResponseDTO RealizarLogin(LoginDTO parans)
        {
            var usuario = _loginRepository.GetUsuariosIdChave(parans.login, parans.password);
            if (usuario == null || usuario.id_usuario == 0)
                return null;

            var jti = Guid.NewGuid().ToString();
            var token = GerarToken(usuario, parans.rememberUser, jti);
            var refreshToken = GerarRefreshToken();

            _loginRepository.AtualizaToken(usuario.id_usuario.ToString(), jti);

            try
            {
                _loginRepository.SalvarRefreshToken(usuario.id_usuario, refreshToken, DateTime.UtcNow.AddDays(30));
            }
            catch (Exception)
            {
                // Tabela refresh_tokens pode não existir ainda — login continua funcionando
                refreshToken = null;
            }

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                IdUsuario = usuario.id_usuario,
                NomeUsuario = usuario.n_lgnd,
                NivelPermissao = usuario.nivel_permissao,
                Role = RoleHelper.GetRoleName(usuario.nivel_permissao)
            };
        }

        public AuthResponseDTO RefreshToken(RefreshTokenRequestDTO request)
        {
            var storedRefresh = _loginRepository.GetRefreshToken(request.RefreshToken);
            if (storedRefresh == null || !storedRefresh.IsActive)
                return null;

            var usuario = _loginRepository.GetUsuarioById(storedRefresh.id_usuario);
            if (usuario == null)
                return null;

            var newRefreshToken = GerarRefreshToken();
            _loginRepository.RevogarRefreshToken(request.RefreshToken, newRefreshToken);
            _loginRepository.SalvarRefreshToken(usuario.id_usuario, newRefreshToken, DateTime.UtcNow.AddDays(30));

            var jti = Guid.NewGuid().ToString();
            var newToken = GerarToken(usuario, false, jti);
            _loginRepository.AtualizaToken(usuario.id_usuario.ToString(), jti);

            return new AuthResponseDTO
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                IdUsuario = usuario.id_usuario,
                NomeUsuario = usuario.n_lgnd,
                NivelPermissao = usuario.nivel_permissao,
                Role = RoleHelper.GetRoleName(usuario.nivel_permissao)
            };
        }

        public void RevogarToken(string refreshToken)
        {
            _loginRepository.RevogarRefreshToken(refreshToken, null);
        }

        private string GerarToken(Usuarios usuario, bool rememberUser, string jti)
        {
            var roleName = RoleHelper.GetRoleName(usuario.nivel_permissao);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.id_usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.n_lgnd ?? ""),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("nivel_permissao", usuario.nivel_permissao.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
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

        private static string GerarRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public bool VerificaSeEstaLogado(string idUsuario)
        {
            var tokens = _loginRepository.GetTokens(idUsuario);
            var token = tokens.OrderByDescending(obj => obj.dt_acesso).FirstOrDefault();
            if (token == null) return false;

            TimeSpan diferenca = DateTime.Now - token.dt_acesso;
            return diferenca <= TimeSpan.FromMinutes(15);
        }

        public (bool sucesso, string erro) CriarUsuario(CriarUsuarioDTO dto, int idUsuarioCriacao)
        {
            if (string.IsNullOrWhiteSpace(dto.login))
                return (false, "Login não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.senha) || dto.senha.Length < 6)
                return (false, "Senha deve ter pelo menos 6 caracteres.");

            if (_loginRepository.LoginExiste(dto.login))
                return (false, "Já existe um usuário com esse login.");

            var senhaHash = PasswordHelper.HashPassword(dto.senha);
            _loginRepository.CriarUsuario(dto.login, senhaHash, dto.nivel_permissao, idUsuarioCriacao);
            return (true, "");
        }

        public List<Usuarios> GetTodosUsuarios()
        {
            return _loginRepository.GetTodosUsuarios();
        }

        public (bool sucesso, string erro) AtualizarUsuario(AtualizarUsuarioDTO dto, int idEdicao)
        {
            if (string.IsNullOrWhiteSpace(dto.login))
                return (false, "Login não pode ser vazio.");

            if (!string.IsNullOrWhiteSpace(dto.nova_senha) && dto.nova_senha.Length < 6)
                return (false, "Nova senha deve ter pelo menos 6 caracteres.");

            string novaSenhaHash = null;
            if (!string.IsNullOrWhiteSpace(dto.nova_senha))
                novaSenhaHash = PasswordHelper.HashPassword(dto.nova_senha);

            _loginRepository.AtualizarUsuario(dto.id_usuario, dto.login, dto.nivel_permissao, novaSenhaHash, idEdicao);
            return (true, "");
        }

        public void DeletarUsuario(int id, int idDelecao)
        {
            _loginRepository.DeletarUsuario(id, idDelecao);
        }
    }
}
