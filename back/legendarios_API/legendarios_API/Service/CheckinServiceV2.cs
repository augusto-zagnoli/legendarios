using System.Collections.Generic;
using System.Linq;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Service
{
    public class CheckinServiceV2 : ICheckinService
    {
        private readonly ICheckinRepository _repo;
        private readonly IInscricoesRepository _inscricoesRepo;

        public CheckinServiceV2(ICheckinRepository repo, IInscricoesRepository inscricoesRepo)
        {
            _repo = repo;
            _inscricoesRepo = inscricoesRepo;
        }

        public List<CheckinResponseDTO> GetByEvento(int idEvento)
        {
            return _repo.GetByEvento(idEvento).Select(MapToDTO).ToList();
        }

        public ApiResponse<CheckinResponseDTO> RegistrarCheckin(CheckinRequestDTO dto, int? registradoPor)
        {
            var inscricao = _inscricoesRepo.GetById(dto.IdInscricao);
            if (inscricao == null)
                return ApiResponse<CheckinResponseDTO>.Erro("Inscrição não encontrada.");

            if (inscricao.status != "confirmado")
                return ApiResponse<CheckinResponseDTO>.Erro("Inscrição não está confirmada.");

            if (_repo.ExisteCheckin(dto.IdInscricao))
                return ApiResponse<CheckinResponseDTO>.Erro("Check-in já realizado para esta inscrição.");

            var checkin = new Checkin
            {
                id_inscricao = dto.IdInscricao,
                id_evento = inscricao.id_evento,
                id_legendario = inscricao.id_legendario,
                registrado_por = registradoPor,
                observacoes = dto.Observacoes
            };

            var result = _repo.Criar(checkin);
            return ApiResponse<CheckinResponseDTO>.Ok(MapToDTO(result));
        }

        public ApiResponse<bool> RegistrarCheckout(int idCheckin)
        {
            var result = _repo.RegistrarCheckout(idCheckin);
            return result ? ApiResponse<bool>.Ok(true) : ApiResponse<bool>.Erro("Check-in não encontrado.");
        }

        private static CheckinResponseDTO MapToDTO(Checkin c) => new CheckinResponseDTO
        {
            IdCheckin = c.id_checkin,
            IdInscricao = c.id_inscricao,
            IdEvento = c.id_evento,
            TituloEvento = c.titulo_evento,
            IdLegendario = c.id_legendario,
            NomeLegendario = c.nome_legendario,
            DataCheckin = c.data_checkin,
            DataCheckout = c.data_checkout,
            Observacoes = c.observacoes
        };
    }
}
