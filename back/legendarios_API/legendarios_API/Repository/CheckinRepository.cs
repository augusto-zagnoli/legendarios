using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Repository
{
    public class CheckinRepository : BaseRepository, ICheckinRepository
    {
        public CheckinRepository(IConfiguration configuration) : base(configuration) { }

        public List<Checkin> GetByEvento(int idEvento)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT c.*, e.titulo AS titulo_evento, l.nome AS nome_legendario
                        FROM checkins c
                        JOIN eventos e ON c.id_evento = e.id_evento
                        JOIN legendarios l ON c.id_legendario = l.id_legendario
                        WHERE c.id_evento = @idEvento
                        ORDER BY c.data_checkin DESC";
            return conn.Query<Checkin>(sql, new { idEvento }).ToList();
        }

        public Checkin GetById(int id)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT c.*, e.titulo AS titulo_evento, l.nome AS nome_legendario
                        FROM checkins c
                        JOIN eventos e ON c.id_evento = e.id_evento
                        JOIN legendarios l ON c.id_legendario = l.id_legendario
                        WHERE c.id_checkin = @id";
            return conn.QueryFirstOrDefault<Checkin>(sql, new { id });
        }

        public Checkin Criar(Checkin checkin)
        {
            using var conn = CreateConnection();
            var sql = @"INSERT INTO checkins (id_inscricao, id_evento, id_legendario, registrado_por, observacoes)
                        VALUES (@id_inscricao, @id_evento, @id_legendario, @registrado_por, @observacoes);
                        SELECT LAST_INSERT_ID();";
            checkin.id_checkin = conn.ExecuteScalar<int>(sql, checkin);
            return GetById(checkin.id_checkin);
        }

        public bool RegistrarCheckout(int idCheckin)
        {
            using var conn = CreateConnection();
            return conn.Execute("UPDATE checkins SET data_checkout = NOW() WHERE id_checkin = @idCheckin", new { idCheckin }) > 0;
        }

        public bool ExisteCheckin(int idInscricao)
        {
            using var conn = CreateConnection();
            return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM checkins WHERE id_inscricao = @idInscricao", new { idInscricao }) > 0;
        }
    }
}
