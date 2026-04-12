using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using legendarios_API.Helpers;

namespace legendarios_API.Repository
{
    public class LoginRepositoryV2 : BaseRepository, ILoginRepository
    {
        public LoginRepositoryV2(IConfiguration configuration) : base(configuration) { }

        public Usuarios GetUsuariosIdChave(string usuario, string senha)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM usuarios WHERE deletado = 0 AND n_lgnd = @usuario";
                var user = conn.QueryFirstOrDefault<Usuarios>(sql, new { usuario });

                if (user == null) return null;

                if (!PasswordHelper.VerifyPassword(senha, user.chave))
                    return null;

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Usuarios GetUsuarioById(int id)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM usuarios WHERE id_usuario = @id AND deletado = 0";
                return conn.QueryFirstOrDefault<Usuarios>(sql, new { id });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int AtualizaToken(string idUsuario, string token)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO tokens (token, id_usuario, dt_acesso, deletado)
                        VALUES (@token, @idUsuario, @dtAcesso, 0)";
            return conn.Execute(sql, new
            {
                token,
                idUsuario = int.Parse(idUsuario),
                dtAcesso = DateTime.Now
            });
        }

        public List<Tokens> GetTokens(string usuario)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM tokens WHERE deletado = 0 AND id_usuario = @idUsuario";
                return conn.Query<Tokens>(sql, new { idUsuario = int.Parse(usuario) }).ToList();
            }
            catch (Exception)
            {
                return new List<Tokens>();
            }
        }

        public bool LoginExiste(string login)
        {
            using var conn = CreateConnection();
            var sql = "SELECT COUNT(*) FROM usuarios WHERE n_lgnd = @login AND deletado = 0";
            return conn.ExecuteScalar<int>(sql, new { login }) > 0;
        }

        public void CriarUsuario(string login, string senhaHash, int nivelPermissao, int idUsuarioCriacao)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO usuarios (n_lgnd, chave, nivel_permissao, id_usuario_criacao, deletado)
                        VALUES (@login, @senhaHash, @nivelPermissao, @idUsuarioCriacao, 0)";
            conn.Execute(sql, new { login, senhaHash, nivelPermissao, idUsuarioCriacao });
        }

        public List<Usuarios> GetTodosUsuarios()
        {
            try
            {
                using var conn = CreateConnection();
                var sql = @"SELECT id_usuario, n_lgnd, nivel_permissao, id_usuario_criacao, 
                           data_edicao, id_usuario_edicao 
                           FROM usuarios WHERE deletado = 0 ORDER BY id_usuario ASC";
                return conn.Query<Usuarios>(sql).ToList();
            }
            catch (Exception)
            {
                return new List<Usuarios>();
            }
        }

        public void AtualizarUsuario(int id, string login, int nivelPermissao, string novaSenhaHash, int idEdicao)
        {
            using var conn = CreateConnection();
            if (!string.IsNullOrWhiteSpace(novaSenhaHash))
            {
                var sql = @"UPDATE usuarios SET n_lgnd = @login, nivel_permissao = @nivelPermissao,
                            chave = @novaSenhaHash, data_edicao = NOW(), id_usuario_edicao = @idEdicao
                            WHERE id_usuario = @id AND deletado = 0";
                conn.Execute(sql, new { id, login, nivelPermissao, novaSenhaHash, idEdicao });
            }
            else
            {
                var sql = @"UPDATE usuarios SET n_lgnd = @login, nivel_permissao = @nivelPermissao,
                            data_edicao = NOW(), id_usuario_edicao = @idEdicao
                            WHERE id_usuario = @id AND deletado = 0";
                conn.Execute(sql, new { id, login, nivelPermissao, idEdicao });
            }
        }

        public void DeletarUsuario(int id, int idDelecao)
        {
            using var conn = CreateConnection();
            var sql = @"UPDATE usuarios SET deletado = 1, data_delecao = NOW(), id_usuario_delecao = @idDelecao
                        WHERE id_usuario = @id";
            conn.Execute(sql, new { id, idDelecao });
        }

        public void SalvarRefreshToken(int idUsuario, string token, DateTime expiresAt)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO refresh_tokens (token, id_usuario, expires_at, created_at)
                        VALUES (@token, @idUsuario, @expiresAt, NOW())";
            conn.Execute(sql, new { token, idUsuario, expiresAt });
        }

        public RefreshToken GetRefreshToken(string token)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM refresh_tokens WHERE token = @token";
                return conn.QueryFirstOrDefault<RefreshToken>(sql, new { token });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RevogarRefreshToken(string token, string replacedBy)
        {
            using var conn = CreateConnection();
            var sql = @"UPDATE refresh_tokens SET revoked_at = NOW(), replaced_by = @replacedBy
                        WHERE token = @token";
            conn.Execute(sql, new { token, replacedBy });
        }
    }
}
