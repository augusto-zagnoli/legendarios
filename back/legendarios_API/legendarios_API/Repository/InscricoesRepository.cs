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
    public class InscricoesRepository : BaseRepository, IInscricoesRepository
    {
        public InscricoesRepository(IConfiguration configuration) : base(configuration) { }

        public List<Inscricao> GetByEvento(int idEvento)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT i.*, e.titulo AS titulo_evento, l.nome AS nome_legendario
                        FROM inscricoes i
                        JOIN eventos e ON i.id_evento = e.id_evento
                        JOIN legendarios l ON i.id_legendario = l.id_legendario
                        WHERE i.id_evento = @idEvento
                        ORDER BY i.data_inscricao DESC";
            return conn.Query<Inscricao>(sql, new { idEvento }).ToList();
        }

        public List<Inscricao> GetByLegendario(int idLegendario)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT i.*, e.titulo AS titulo_evento, l.nome AS nome_legendario
                        FROM inscricoes i
                        JOIN eventos e ON i.id_evento = e.id_evento
                        JOIN legendarios l ON i.id_legendario = l.id_legendario
                        WHERE i.id_legendario = @idLegendario
                        ORDER BY i.data_inscricao DESC";
            return conn.Query<Inscricao>(sql, new { idLegendario }).ToList();
        }

        public Inscricao GetById(int id)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT i.*, e.titulo AS titulo_evento, l.nome AS nome_legendario
                        FROM inscricoes i
                        JOIN eventos e ON i.id_evento = e.id_evento
                        JOIN legendarios l ON i.id_legendario = l.id_legendario
                        WHERE i.id_inscricao = @id";
            return conn.QueryFirstOrDefault<Inscricao>(sql, new { id });
        }

        public Inscricao Criar(Inscricao inscricao)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO inscricoes (id_evento, id_legendario, status, observacoes, inscrito_por)
                        VALUES (@id_evento, @id_legendario, @status, @observacoes, @inscrito_por);
                        SELECT LAST_INSERT_ID();";
            inscricao.id_inscricao = conn.ExecuteScalar<int>(sql, inscricao);

            // Atualizar vagas ocupadas
            conn.Execute("UPDATE eventos SET vagas_ocupadas = vagas_ocupadas + 1 WHERE id_evento = @id_evento",
                new { inscricao.id_evento });

            return GetById(inscricao.id_inscricao);
        }

        public bool AtualizarStatus(int id, string status)
        {
            using var conn = CreateConnection();
            var sql = "UPDATE inscricoes SET status = @status";
            if (status == "confirmado")
                sql += ", data_confirmacao = NOW()";
            if (status == "cancelado")
                sql += ", data_cancelamento = NOW()";
            sql += " WHERE id_inscricao = @id";
            return conn.Execute(sql, new { id, status }) > 0;
        }

        public bool Cancelar(int id)
        {
            using var conn = CreateConnection();
            var inscricao = GetById(id);
            if (inscricao == null) return false;

            var sql = "UPDATE inscricoes SET status = 'cancelado', data_cancelamento = NOW() WHERE id_inscricao = @id";
            var result = conn.Execute(sql, new { id }) > 0;

            if (result)
            {
                conn.Execute("UPDATE eventos SET vagas_ocupadas = GREATEST(vagas_ocupadas - 1, 0) WHERE id_evento = @id_evento",
                    new { inscricao.id_evento });
            }

            return result;
        }

        public bool ExisteInscricao(int idEvento, int idLegendario)
        {
            using var conn = CreateConnection();
            var sql = "SELECT COUNT(*) FROM inscricoes WHERE id_evento = @idEvento AND id_legendario = @idLegendario AND status != 'cancelado'";
            return conn.ExecuteScalar<int>(sql, new { idEvento, idLegendario }) > 0;
        }

        public int ContarInscritosPorEvento(int idEvento)
        {
            using var conn = CreateConnection();
            return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM inscricoes WHERE id_evento = @idEvento AND status != 'cancelado'",
                new { idEvento });
        }
    }
}
