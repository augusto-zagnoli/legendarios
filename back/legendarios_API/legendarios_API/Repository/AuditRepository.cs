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
    public class AuditRepository : BaseRepository, IAuditRepository
    {
        public AuditRepository(IConfiguration configuration) : base(configuration) { }

        public void Registrar(string tabela, int idRegistro, string acao, string dadosAnteriores, string dadosNovos, int? idUsuario, string nomeUsuario, string ipAddress)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = @"INSERT INTO audit_log (tabela, id_registro, acao, dados_anteriores, dados_novos, id_usuario, nome_usuario, ip_address)
                            VALUES (@tabela, @idRegistro, @acao, @dadosAnteriores, @dadosNovos, @idUsuario, @nomeUsuario, @ipAddress)";
                conn.Execute(sql, new { tabela, idRegistro, acao, dadosAnteriores, dadosNovos, idUsuario, nomeUsuario, ipAddress });
            }
            catch
            {
                // Audit failures should not break the main operation
            }
        }

        public List<AuditLog> GetByTabela(string tabela, int? idRegistro = null)
        {
            using var conn = CreateConnection();
            var sql = "SELECT * FROM audit_log WHERE tabela = @tabela";
            if (idRegistro.HasValue)
                sql += " AND id_registro = @idRegistro";
            sql += " ORDER BY data_acao DESC LIMIT 100";
            return conn.Query<AuditLog>(sql, new { tabela, idRegistro }).ToList();
        }

        public PaginatedResponse<AuditLog> GetPaginado(PaginacaoParams param, string tabela = null)
        {
            using var conn = CreateConnection();
            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(tabela))
            {
                where += " AND tabela = @tabela";
                parameters.Add("tabela", tabela);
            }

            if (!string.IsNullOrWhiteSpace(param.Busca))
            {
                where += " AND (tabela LIKE @busca OR acao LIKE @busca OR nome_usuario LIKE @busca)";
                parameters.Add("busca", $"%{param.Busca}%");
            }

            var sqlCount = $"SELECT COUNT(*) FROM audit_log {where}";
            var total = conn.ExecuteScalar<int>(sqlCount, parameters);

            var offset = (param.Pagina - 1) * param.TamanhoPagina;
            var sql = $"SELECT * FROM audit_log {where} ORDER BY data_acao DESC LIMIT @limit OFFSET @offset";
            parameters.Add("limit", param.TamanhoPagina);
            parameters.Add("offset", offset);

            var result = conn.Query<AuditLog>(sql, parameters).ToList();

            return new PaginatedResponse<AuditLog>
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
