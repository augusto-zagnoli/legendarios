using System.Collections.Generic;
using System.Linq;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Service
{
    public class EventosServiceV2 : IEventosService
    {
        private readonly IEventosRepository _repo;

        public EventosServiceV2(IEventosRepository repo)
        {
            _repo = repo;
        }

        public List<EventoResponseDTO> GetTodos(string status = null)
        {
            return _repo.GetTodos(status).Select(MapToDTO).ToList();
        }

        public EventoResponseDTO GetById(int id)
        {
            var evento = _repo.GetById(id);
            return evento != null ? MapToDTO(evento) : null;
        }

        public EventoResponseDTO Criar(EventoRequestDTO dto, int idUsuario)
        {
            var evento = new Evento
            {
                titulo = dto.Titulo,
                descricao = dto.Descricao,
                data_inicio = dto.DataInicio,
                data_fim = dto.DataFim,
                local_evento = dto.LocalEvento,
                max_vagas = dto.MaxVagas,
                status = string.IsNullOrWhiteSpace(dto.Status) ? "rascunho" : dto.Status,
                id_lider = dto.IdLider,
                imagem_url = dto.ImagemUrl,
                requer_aprovacao = dto.RequerAprovacao,
                criado_por = idUsuario
            };

            var created = _repo.Criar(evento);
            return MapToDTO(created);
        }

        public bool Atualizar(EventoRequestDTO dto)
        {
            var evento = new Evento
            {
                id_evento = dto.IdEvento ?? 0,
                titulo = dto.Titulo,
                descricao = dto.Descricao,
                data_inicio = dto.DataInicio,
                data_fim = dto.DataFim,
                local_evento = dto.LocalEvento,
                max_vagas = dto.MaxVagas,
                status = dto.Status,
                id_lider = dto.IdLider,
                imagem_url = dto.ImagemUrl,
                requer_aprovacao = dto.RequerAprovacao
            };

            return _repo.Atualizar(evento);
        }

        public bool Deletar(int id) => _repo.Deletar(id);

        public PaginatedResponse<EventoResponseDTO> GetPaginado(PaginacaoParams param, string status = null)
        {
            var result = _repo.GetPaginado(param, status);
            return new PaginatedResponse<EventoResponseDTO>
            {
                Sucesso = result.Sucesso,
                Data = result.Data?.Select(MapToDTO).ToList(),
                TotalRegistros = result.TotalRegistros,
                Pagina = result.Pagina,
                TamanhoPagina = result.TamanhoPagina,
                TotalPaginas = result.TotalPaginas
            };
        }

        private static EventoResponseDTO MapToDTO(Evento e) => new EventoResponseDTO
        {
            IdEvento = e.id_evento,
            Titulo = e.titulo,
            Descricao = e.descricao,
            DataInicio = e.data_inicio,
            DataFim = e.data_fim,
            LocalEvento = e.local_evento,
            MaxVagas = e.max_vagas,
            VagasOcupadas = e.vagas_ocupadas,
            Status = e.status,
            IdLider = e.id_lider,
            NomeLider = e.nome_lider,
            ImagemUrl = e.imagem_url,
            RequerAprovacao = e.requer_aprovacao,
            CriadoEm = e.criado_em
        };
    }
}
