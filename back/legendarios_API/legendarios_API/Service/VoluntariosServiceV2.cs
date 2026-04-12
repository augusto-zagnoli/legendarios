using System.Collections.Generic;
using System.Linq;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Service
{
    public class VoluntariosServiceV2 : IVoluntariosService
    {
        private readonly IVoluntariosRepository _repo;

        public VoluntariosServiceV2(IVoluntariosRepository repo)
        {
            _repo = repo;
        }

        public List<VoluntarioResponseDTO> GetTodos()
        {
            return _repo.GetTodos().Select(MapToDTO).ToList();
        }

        public VoluntarioResponseDTO GetById(int id)
        {
            var v = _repo.GetById(id);
            return v != null ? MapToDTO(v) : null;
        }

        public ApiResponse<VoluntarioResponseDTO> Criar(VoluntarioRequestDTO dto)
        {
            var voluntario = new Voluntario
            {
                id_legendario = dto.IdLegendario,
                habilidades = dto.Habilidades,
                disponibilidade = dto.Disponibilidade,
                area_atuacao = dto.AreaAtuacao
            };

            var result = _repo.Criar(voluntario);
            return ApiResponse<VoluntarioResponseDTO>.Ok(MapToDTO(result));
        }

        public ApiResponse<bool> Atualizar(VoluntarioRequestDTO dto)
        {
            var voluntario = new Voluntario
            {
                id_voluntario = dto.IdVoluntario ?? 0,
                id_legendario = dto.IdLegendario,
                habilidades = dto.Habilidades,
                disponibilidade = dto.Disponibilidade,
                area_atuacao = dto.AreaAtuacao
            };

            var result = _repo.Atualizar(voluntario);
            return result ? ApiResponse<bool>.Ok(true) : ApiResponse<bool>.Erro("Voluntário não encontrado.");
        }

        public ApiResponse<bool> Deletar(int id)
        {
            var result = _repo.Deletar(id);
            return result ? ApiResponse<bool>.Ok(true) : ApiResponse<bool>.Erro("Voluntário não encontrado.");
        }

        public PaginatedResponse<VoluntarioResponseDTO> GetPaginado(PaginacaoParams param)
        {
            var result = _repo.GetPaginado(param);
            return new PaginatedResponse<VoluntarioResponseDTO>
            {
                Sucesso = result.Sucesso,
                Data = result.Data?.Select(MapToDTO).ToList(),
                TotalRegistros = result.TotalRegistros,
                Pagina = result.Pagina,
                TamanhoPagina = result.TamanhoPagina,
                TotalPaginas = result.TotalPaginas
            };
        }

        private static VoluntarioResponseDTO MapToDTO(Voluntario v) => new VoluntarioResponseDTO
        {
            IdVoluntario = v.id_voluntario,
            IdLegendario = v.id_legendario,
            NomeLegendario = v.nome_legendario,
            EmailLegendario = v.email_legendario,
            CelularLegendario = v.celular_legendario,
            Habilidades = v.habilidades,
            Disponibilidade = v.disponibilidade,
            AreaAtuacao = v.area_atuacao,
            DataCadastro = v.data_cadastro,
            Ativo = v.ativo
        };
    }
}
