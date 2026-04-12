using System.Text.Json;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Service
{
    public class AuditServiceV2 : IAuditService
    {
        private readonly IAuditRepository _repo;

        public AuditServiceV2(IAuditRepository repo)
        {
            _repo = repo;
        }

        public void Registrar(string tabela, int idRegistro, string acao, object dadosAnteriores, object dadosNovos, int? idUsuario, string nomeUsuario, string ipAddress)
        {
            var anterior = dadosAnteriores != null ? JsonSerializer.Serialize(dadosAnteriores) : null;
            var novo = dadosNovos != null ? JsonSerializer.Serialize(dadosNovos) : null;

            _repo.Registrar(tabela, idRegistro, acao, anterior, novo, idUsuario, nomeUsuario, ipAddress);
        }

        public PaginatedResponse<AuditLog> GetPaginado(PaginacaoParams param, string tabela = null)
        {
            return _repo.GetPaginado(param, tabela);
        }
    }
}
