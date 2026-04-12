using System.Collections.Generic;
using System.Linq;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Service
{
    public class InscricoesServiceV2 : IInscricoesService
    {
        private readonly IInscricoesRepository _repo;
        private readonly IEventosRepository _eventosRepo;

        public InscricoesServiceV2(IInscricoesRepository repo, IEventosRepository eventosRepo)
        {
            _repo = repo;
            _eventosRepo = eventosRepo;
        }

        public List<InscricaoResponseDTO> GetByEvento(int idEvento)
        {
            return _repo.GetByEvento(idEvento).Select(MapToDTO).ToList();
        }

        public List<InscricaoResponseDTO> GetByLegendario(int idLegendario)
        {
            return _repo.GetByLegendario(idLegendario).Select(MapToDTO).ToList();
        }

        public ApiResponse<InscricaoResponseDTO> Inscrever(InscricaoRequestDTO dto, int? inscritoPor)
        {
            if (_repo.ExisteInscricao(dto.IdEvento, dto.IdLegendario))
                return ApiResponse<InscricaoResponseDTO>.Erro("Participante já inscrito neste evento.");

            var evento = _eventosRepo.GetById(dto.IdEvento);
            if (evento == null)
                return ApiResponse<InscricaoResponseDTO>.Erro("Evento não encontrado.");

            if (evento.status != "aberto")
                return ApiResponse<InscricaoResponseDTO>.Erro("Evento não está aberto para inscrições.");

            if (evento.max_vagas > 0 && evento.vagas_ocupadas >= evento.max_vagas)
            {
                var inscricaoEspera = new Inscricao
                {
                    id_evento = dto.IdEvento,
                    id_legendario = dto.IdLegendario,
                    status = "lista_espera",
                    observacoes = dto.Observacoes,
                    inscrito_por = inscritoPor
                };
                var waitResult = _repo.Criar(inscricaoEspera);
                return ApiResponse<InscricaoResponseDTO>.Ok(MapToDTO(waitResult), "Adicionado à lista de espera.");
            }

            var status = evento.requer_aprovacao ? "pendente" : "confirmado";
            var inscricao = new Inscricao
            {
                id_evento = dto.IdEvento,
                id_legendario = dto.IdLegendario,
                status = status,
                observacoes = dto.Observacoes,
                inscrito_por = inscritoPor
            };

            var result = _repo.Criar(inscricao);
            return ApiResponse<InscricaoResponseDTO>.Ok(MapToDTO(result));
        }

        public ApiResponse<bool> AtualizarStatus(int id, string status)
        {
            var validos = new[] { "pendente", "confirmado", "cancelado", "lista_espera" };
            if (!validos.Contains(status))
                return ApiResponse<bool>.Erro("Status inválido.");

            var result = _repo.AtualizarStatus(id, status);
            return ApiResponse<bool>.Ok(result);
        }

        public ApiResponse<bool> Cancelar(int id)
        {
            var result = _repo.Cancelar(id);
            return result ? ApiResponse<bool>.Ok(true) : ApiResponse<bool>.Erro("Inscrição não encontrada.");
        }

        private static InscricaoResponseDTO MapToDTO(Inscricao i) => new InscricaoResponseDTO
        {
            IdInscricao = i.id_inscricao,
            IdEvento = i.id_evento,
            TituloEvento = i.titulo_evento,
            IdLegendario = i.id_legendario,
            NomeLegendario = i.nome_legendario,
            Status = i.status,
            DataInscricao = i.data_inscricao,
            DataConfirmacao = i.data_confirmacao,
            Observacoes = i.observacoes
        };
    }
}
