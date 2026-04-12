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
    public class EventosRepository : BaseRepository, IEventosRepository
    {
        public EventosRepository(IConfiguration configuration) : base(configuration) { }

        public List<Evento> GetTodos(string status = null)
        {
            using var conn = CreateConnection();
            var sql = "SELECT e.*, u.n_lgnd AS nome_lider FROM eventos e LEFT JOIN usuarios u ON e.id_lider = u.id_usuario";
            if (!string.IsNullOrWhiteSpace(status))
            {
                sql += " WHERE e.status = @status";
                return conn.Query<Evento>(sql, new { status }).ToList();
            }
            return conn.Query<Evento>(sql).ToList();
        }

        public Evento GetById(int id)
        {
            using var conn = CreateConnection();
            var sql = "SELECT e.*, u.n_lgnd AS nome_lider FROM eventos e LEFT JOIN usuarios u ON e.id_lider = u.id_usuario WHERE e.id_evento = @id";
            return conn.QueryFirstOrDefault<Evento>(sql, new { id });
        }

        public Evento Criar(Evento evento)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO eventos (titulo, descricao, data_inicio, data_fim, local_evento, max_vagas, 
                        status, id_lider, imagem_url, requer_aprovacao, criado_por)
                        VALUES (@titulo, @descricao, @data_inicio, @data_fim, @local_evento, @max_vagas,
                        @status, @id_lider, @imagem_url, @requer_aprovacao, @criado_por);
                        SELECT LAST_INSERT_ID();";
            evento.id_evento = conn.ExecuteScalar<int>(sql, evento);
            return GetById(evento.id_evento);
        }

        public bool Atualizar(Evento evento)
        {
            using var conn = CreateConnection();
            var sql = @"UPDATE eventos SET titulo = @titulo, descricao = @descricao,
                        data_inicio = @data_inicio, data_fim = @data_fim, local_evento = @local_evento,
                        max_vagas = @max_vagas, status = @status, id_lider = @id_lider,
                        imagem_url = @imagem_url, requer_aprovacao = @requer_aprovacao,
                        atualizado_em = NOW()
                        WHERE id_evento = @id_evento";
            return conn.Execute(sql, evento) > 0;
        }

        public bool Deletar(int id)
        {
            using var conn = CreateConnection();
            return conn.Execute("DELETE FROM eventos WHERE id_evento = @id", new { id }) > 0;
        }

        public PaginatedResponse<Evento> GetPaginado(PaginacaoParams param, string status = null)
        {
            using var conn = CreateConnection();
            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(status))
            {
                where += " AND e.status = @status";
                parameters.Add("status", status);
            }

            if (!string.IsNullOrWhiteSpace(param.Busca))
            {
                where += " AND (e.titulo LIKE @busca OR e.descricao LIKE @busca OR e.local_evento LIKE @busca)";
                parameters.Add("busca", $"%{param.Busca}%");
            }

            var sqlCount = $"SELECT COUNT(*) FROM eventos e {where}";
            var total = conn.ExecuteScalar<int>(sqlCount, parameters);

            var orderBy = param.OrdenarPor switch
            {
                "titulo" => "e.titulo",
                "data" => "e.data_inicio",
                "status" => "e.status",
                _ => "e.data_inicio DESC"
            };

            var offset = (param.Pagina - 1) * param.TamanhoPagina;
            var sql = $@"SELECT e.*, u.n_lgnd AS nome_lider FROM eventos e 
                        LEFT JOIN usuarios u ON e.id_lider = u.id_usuario
                        {where} ORDER BY {orderBy} LIMIT @limit OFFSET @offset";
            parameters.Add("limit", param.TamanhoPagina);
            parameters.Add("offset", offset);

            var result = conn.Query<Evento>(sql, parameters).ToList();

            return new PaginatedResponse<Evento>
            {
                Sucesso = true,
                Data = result,
                TotalRegistros = total,
                Pagina = param.Pagina,
                TamanhoPagina = param.TamanhoPagina,
                TotalPaginas = (int)Math.Ceiling(total / (double)param.TamanhoPagina)
            };
        }

        public DashboardDTO GetDashboardStats()
        {
            using var conn = CreateConnection();
            var stats = new DashboardDTO();

            stats.TotalLegendarios = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM legendarios WHERE deletado = 0");
            stats.LegendariosPendentes = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'pendente'");
            stats.LegendariosAprovados = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'aprovado'");
            stats.LegendariosReprovados = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'reprovado'");
            stats.TotalEventos = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM eventos");
            stats.EventosAtivos = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM eventos WHERE status = 'aberto'");
            stats.TotalVoluntarios = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM voluntarios WHERE deletado = 0 AND ativo = 1");
            stats.TotalInscricoes = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM inscricoes WHERE status != 'cancelado'");
            stats.CheckinsHoje = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM checkins WHERE DATE(data_checkin) = CURDATE()");

            return stats;
        }
    }
}
