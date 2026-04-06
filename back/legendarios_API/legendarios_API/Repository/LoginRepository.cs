using Dapper;
using legendarios_API.Entity;
using MySqlConnector;
using System.Collections.Generic;
using System;
using System.Linq;

namespace legendarios_API.Repository
{
    public class LoginRepository
    {
        static string _connectionString = "Server=187.77.245.217;port=3306;Database=DBLEGENDARIOS;Uid=root;Pwd=legendarioSenhaBanco";
        MySqlConnection _conn = new MySqlConnection(_connectionString);

        public List<Usuarios> GetUsuarios()
        {
            try
            {
                var sql = "SELECT * FROM usuarios WHERE deletado = 0";

                var result = this._conn.Query<Usuarios>(sql).ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<Usuarios>();
            }
        }

        public Usuarios GetUsuariosIdChave(string usuario, string senha)
        {
            try
            {
                var sql = @"SELECT * FROM usuarios WHERE
                                deletado = 0
                            AND n_lgnd = @usuario
                            AND chave = @senha";

                var result = this._conn.Query<Usuarios>(sql, new { usuario, senha }).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int AtualizaToken(string idUsuario, string token)
        {
            var sql = @"INSERT INTO tokens (token, id_usuario, dt_acesso, deletado)
                        VALUES (@token, @idUsuario, @dtAcesso, 0)";

            return this._conn.Execute(sql, new
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
                var sql = @"SELECT * FROM tokens WHERE deletado = 0 AND id_usuario = @idUsuario";

                var result = this._conn.Query<Tokens>(sql, new { idUsuario = int.Parse(usuario) }).ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<Tokens>();
            }
        }

        public bool LoginExiste(string login)
        {
            var sql = "SELECT COUNT(*) FROM usuarios WHERE n_lgnd = @login AND deletado = 0";
            var count = this._conn.ExecuteScalar<int>(sql, new { login });
            return count > 0;
        }

        public void CriarUsuario(string login, string senha, int nivelPermissao, int idUsuarioCriacao)
        {
            var sql = @"INSERT INTO usuarios (n_lgnd, chave, nivel_permissao, id_usuario_criacao, deletado)
                        VALUES (@login, @senha, @nivelPermissao, @idUsuarioCriacao, 0)";

            this._conn.Execute(sql, new { login, senha, nivelPermissao, idUsuarioCriacao });
        }

        public List<Usuarios> GetTodosUsuarios()
        {
            try
            {
                var sql = "SELECT id_usuario, n_lgnd, nivel_permissao, id_usuario_criacao, data_edicao, id_usuario_edicao FROM usuarios WHERE deletado = 0 ORDER BY id_usuario ASC";
                return this._conn.Query<Usuarios>(sql).ToList();
            }
            catch (Exception)
            {
                return new List<Usuarios>();
            }
        }

        public void AtualizarUsuario(int id, string login, int nivelPermissao, string novaSenha, int idEdicao)
        {
            if (!string.IsNullOrWhiteSpace(novaSenha))
            {
                var sql = @"UPDATE usuarios SET n_lgnd = @login, nivel_permissao = @nivelPermissao,
                            chave = @novaSenha, data_edicao = NOW(), id_usuario_edicao = @idEdicao
                            WHERE id_usuario = @id AND deletado = 0";
                this._conn.Execute(sql, new { id, login, nivelPermissao, novaSenha, idEdicao });
            }
            else
            {
                var sql = @"UPDATE usuarios SET n_lgnd = @login, nivel_permissao = @nivelPermissao,
                            data_edicao = NOW(), id_usuario_edicao = @idEdicao
                            WHERE id_usuario = @id AND deletado = 0";
                this._conn.Execute(sql, new { id, login, nivelPermissao, idEdicao });
            }
        }

        public void DeletarUsuario(int id, int idDelecao)
        {
            var sql = @"UPDATE usuarios SET deletado = 1, data_delecao = NOW(), id_usuario_delecao = @idDelecao
                        WHERE id_usuario = @id";
            this._conn.Execute(sql, new { id, idDelecao });
        }

    }
}
