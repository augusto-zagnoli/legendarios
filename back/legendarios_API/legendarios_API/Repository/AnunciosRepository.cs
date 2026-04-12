using System;
using System.Collections.Generic;
using Dapper;
using MySqlConnector;
using legendarios_API.Entity;
using legendarios_API.DTO;
using legendarios_API.Interfaces;
using Microsoft.Extensions.Configuration;

namespace legendarios_API.Repository
{
    public class AnunciosRepository : BaseRepository, IAnunciosRepository
    {
        public AnunciosRepository(IConfiguration configuration) : base(configuration) { }

        private MySqlConnection Conn() => CreateConnection();

        public AnuncioResponseListDTO GetTodos()
        {
            try
            {
                using var conn = Conn();
                var sql = "SELECT * FROM anuncios WHERE ativo = 1 ORDER BY ordem ASC, criado_em DESC";
                var data = conn.Query<Anuncio>(sql).AsList();
                return new AnuncioResponseListDTO { Sucesso = true, Data = data };
            }
            catch (Exception ex)
            {
                return new AnuncioResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public AnuncioResponseListDTO GetTodosAdm()
        {
            try
            {
                using var conn = Conn();
                var sql = "SELECT * FROM anuncios ORDER BY ordem ASC, criado_em DESC";
                var data = conn.Query<Anuncio>(sql).AsList();
                return new AnuncioResponseListDTO { Sucesso = true, Data = data };
            }
            catch (Exception ex)
            {
                return new AnuncioResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public AnuncioResponseOneDTO Criar(AnuncioDTO dto)
        {
            try
            {
                using var conn = Conn();
                var sql = @"INSERT INTO anuncios (titulo, imagem_url, texto, link, ativo, ordem, criado_por)
                            VALUES (@titulo, @imagem_url, @texto, @link, @ativo, @ordem, @criado_por);
                            SELECT LAST_INSERT_ID();";
                var id = conn.ExecuteScalar<int>(sql, dto);
                var anuncio = conn.QueryFirstOrDefault<Anuncio>("SELECT * FROM anuncios WHERE id_anuncio = @id", new { id });
                return new AnuncioResponseOneDTO { Sucesso = true, Data = anuncio };
            }
            catch (Exception ex)
            {
                return new AnuncioResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public AnuncioResponseOneDTO Atualizar(AnuncioDTO dto)
        {
            try
            {
                using var conn = Conn();
                var sql = @"UPDATE anuncios SET titulo = @titulo, imagem_url = @imagem_url,
                            texto = @texto, link = @link, ativo = @ativo, ordem = @ordem,
                            modificado_por = @modificado_por, atualizado_em = NOW()
                            WHERE id_anuncio = @id_anuncio";
                conn.Execute(sql, dto);
                var anuncio = conn.QueryFirstOrDefault<Anuncio>("SELECT * FROM anuncios WHERE id_anuncio = @id_anuncio", dto);
                return new AnuncioResponseOneDTO { Sucesso = true, Data = anuncio };
            }
            catch (Exception ex)
            {
                return new AnuncioResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public AnuncioResponseOneDTO Deletar(int id)
        {
            try
            {
                using var conn = Conn();
                conn.Execute("DELETE FROM anuncios WHERE id_anuncio = @id", new { id });
                return new AnuncioResponseOneDTO { Sucesso = true };
            }
            catch (Exception ex)
            {
                return new AnuncioResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }
    }
}
