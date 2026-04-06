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

    }
}
