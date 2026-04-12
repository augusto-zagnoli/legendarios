using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Repository
{
    public class VoluntariosRepository : BaseRepository, IVoluntariosRepository
    {
        public VoluntariosRepository(IConfiguration configuration) : base(configuration) { }

        public List<Voluntario> GetTodos()
        {
            using var conn = CreateConnection();
            var sql = @"SELECT v.*, l.nome AS nome_legendario, l.email AS email_legendario, l.celular AS celular_legendario
                        FROM voluntarios v
                        JOIN legendarios l ON v.id_legendario = l.id_legendario
                        WHERE v.deletado = 0
                        ORDER BY v.data_cadastro DESC";
            return conn.Query<Voluntario>(sql).ToList();
        }

        public Voluntario GetById(int id)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT v.*, l.nome AS nome_legendario, l.email AS email_legendario, l.celular AS celular_legendario
                        FROM voluntarios v
                        JOIN legendarios l ON v.id_legendario = l.id_legendario
                        WHERE v.id_voluntario = @id AND v.deletado = 0";
            return conn.QueryFirstOrDefault<Voluntario>(sql, new { id });
        }

        public Voluntario Criar(Voluntario voluntario)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO voluntarios (id_legendario, habilidades, disponibilidade, area_atuacao)
                        VALUES (@id_legendario, @habilidades, @disponibilidade, @area_atuacao);
                        SELECT LAST_INSERT_ID();";
            voluntario.id_voluntario = conn.ExecuteScalar<int>(sql, voluntario);
            return GetById(voluntario.id_voluntario);
        }

        public bool Atualizar(Voluntario voluntario)
        {
            using var conn = CreateConnection();
            var sql = @"UPDATE voluntarios SET habilidades = @habilidades, disponibilidade = @disponibilidade,
                        area_atuacao = @area_atuacao, ativo = @ativo
                        WHERE id_voluntario = @id_voluntario AND deletado = 0";
            return conn.Execute(sql, voluntario) > 0;
        }

        public bool Deletar(int id)
        {
            using var conn = CreateConnection();
            return conn.Execute("UPDATE voluntarios SET deletado = 1 WHERE id_voluntario = @id", new { id }) > 0;
        }

        public PaginatedResponse<Voluntario> GetPaginado(PaginacaoParams param)
        {
            using var conn = CreateConnection();
            var where = "WHERE v.deletado = 0";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(param.Busca))
            {
                where += " AND (l.nome LIKE @busca OR v.habilidades LIKE @busca OR v.area_atuacao LIKE @busca)";
                parameters.Add("busca", $"%{param.Busca}%");
            }

            var sqlCount = $"SELECT COUNT(*) FROM voluntarios v JOIN legendarios l ON v.id_legendario = l.id_legendario {where}";
            var total = conn.ExecuteScalar<int>(sqlCount, parameters);

            var offset = (param.Pagina - 1) * param.TamanhoPagina;
            var sql = $@"SELECT v.*, l.nome AS nome_legendario, l.email AS email_legendario, l.celular AS celular_legendario
                        FROM voluntarios v
                        JOIN legendarios l ON v.id_legendario = l.id_legendario
                        {where} ORDER BY v.data_cadastro DESC LIMIT @limit OFFSET @offset";
            parameters.Add("limit", param.TamanhoPagina);
            parameters.Add("offset", offset);

            var result = conn.Query<Voluntario>(sql, parameters).ToList();

            return new PaginatedResponse<Voluntario>
            {
                Sucesso = true,
                Data = result,
                TotalRegistros = total,
                Pagina = param.Pagina,
                TamanhoPagina = param.TamanhoPagina,
                TotalPaginas = (int)Math.Ceiling(total / (double)param.TamanhoPagina)
            };
        }
    }
}
