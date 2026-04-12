using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text;

namespace legendarios_API.Controllers
{
    [ApiController]
    [Route("api/relatorios")]
    [Authorize(Roles = "Admin,Lider")]
    public class RelatoriosController : ControllerBase
    {
        private readonly ILegendariosRepository _legendarioRepo;
        private readonly IEventosRepository _eventosRepo;
        private readonly IInscricoesRepository _inscricoesRepo;
        private readonly ICheckinRepository _checkinRepo;
        private readonly IAuditRepository _auditRepo;

        public RelatoriosController(
            ILegendariosRepository legendarioRepo,
            IEventosRepository eventosRepo,
            IInscricoesRepository inscricoesRepo,
            ICheckinRepository checkinRepo,
            IAuditRepository auditRepo)
        {
            _legendarioRepo = legendarioRepo;
            _eventosRepo = eventosRepo;
            _inscricoesRepo = inscricoesRepo;
            _checkinRepo = checkinRepo;
            _auditRepo = auditRepo;
        }

        [HttpGet("legendarios/csv")]
        public IActionResult ExportarLegendariosCsv()
        {
            var result = _legendarioRepo.GetAllLegendarios();
            if (result.Data == null || !result.Data.Any())
                return NotFound(ApiResponse<object>.Erro("Nenhum registro encontrado."));

            var csv = new StringBuilder();
            csv.AppendLine("ID,Nome,Email,Celular,CPF,Data Nascimento,Status,Cidade,Estado");

            foreach (var l in result.Data)
            {
                csv.AppendLine($"{l.id_legendario},\"{l.nome}\",\"{l.email}\",\"{l.celular}\",\"{l.cadastro_pessoa}\",{l.data_de_nascimento:dd/MM/yyyy},\"{l.status_cadastro}\",\"{l.cidade}\",\"{l.estado}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "legendarios.csv");
        }

        [HttpGet("evento/{idEvento}/inscritos/csv")]
        public IActionResult ExportarInscritosCsv(int idEvento)
        {
            var inscritos = _inscricoesRepo.GetByEvento(idEvento);
            if (!inscritos.Any())
                return NotFound(ApiResponse<object>.Erro("Nenhum inscrito encontrado."));

            var csv = new StringBuilder();
            csv.AppendLine("ID Inscrição,Nome,Status,Data Inscrição,Data Confirmação");

            foreach (var i in inscritos)
            {
                csv.AppendLine($"{i.id_inscricao},\"{i.nome_legendario}\",\"{i.status}\",{i.data_inscricao:dd/MM/yyyy HH:mm},{i.data_confirmacao?.ToString("dd/MM/yyyy HH:mm") ?? ""}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"inscritos_evento_{idEvento}.csv");
        }

        [HttpGet("evento/{idEvento}/presenca/csv")]
        public IActionResult ExportarPresencaCsv(int idEvento)
        {
            var checkins = _checkinRepo.GetByEvento(idEvento);
            if (!checkins.Any())
                return NotFound(ApiResponse<object>.Erro("Nenhum check-in encontrado."));

            var csv = new StringBuilder();
            csv.AppendLine("ID,Nome,Check-in,Check-out,Observações");

            foreach (var c in checkins)
            {
                csv.AppendLine($"{c.id_checkin},\"{c.nome_legendario}\",{c.data_checkin:dd/MM/yyyy HH:mm},{c.data_checkout?.ToString("dd/MM/yyyy HH:mm") ?? ""},\"{c.observacoes}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"presenca_evento_{idEvento}.csv");
        }

        [HttpGet("auditoria")]
        public IActionResult GetAuditoria([FromQuery] PaginacaoParams param, [FromQuery] string tabela = null)
        {
            var result = _auditRepo.GetPaginado(param, tabela);
            return Ok(result);
        }
    }
}
