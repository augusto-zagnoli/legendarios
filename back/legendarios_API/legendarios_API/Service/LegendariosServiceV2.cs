using System;
using System.Collections.Generic;
using System.Linq;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using legendarios_API.Models;

namespace legendarios_API.Service
{
    public class LegendariosServiceV2 : ILegendariosService
    {
        private readonly ILegendariosRepository _repository;

        public LegendariosServiceV2(ILegendariosRepository repository)
        {
            _repository = repository;
        }

        public ResponseListDTO GetAllLegendarios(LegendariosParams param)
        {
            try
            {
                var result = _repository.GetAllLegendarios();
                var response = result.Data?.ToList() ?? new List<LegendariosDTO>();

                if (!string.IsNullOrEmpty(param.NOMELEGENDARIO))
                {
                    response = response
                        .Where(x => x.nome != null && x.nome.ToUpper().Contains(param.NOMELEGENDARIO.ToUpper()))
                        .ToList();
                }

                if (param.CODIGOLEGENDARIO != null && param.CODIGOLEGENDARIO != 0)
                {
                    response = response
                        .Where(x => x.n_lgnd != null && x.n_lgnd.Contains(param.CODIGOLEGENDARIO.ToString()))
                        .ToList();
                }

                return new ResponseListDTO { Sucesso = true, Data = response, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetAllLegendariosAll()
        {
            return _repository.GetAllLegendariosAll();
        }

        public ResponseOneDTO GetLegendarioById(string idLegendario)
        {
            return _repository.GetLegendarioById(idLegendario);
        }

        public ResponseOneDTO SalvarLegendarioById(LegendariosDTO legendario)
        {
            return _repository.SalvarLegendarioById(legendario);
        }

        public ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario)
        {
            return _repository.CadastrarLegendario(legendario);
        }

        public ResponseListDTO GetEstatisticasDashboard()
        {
            return _repository.GetEstatisticasDashboard();
        }

        public ResponseOneDTO AtualizarStatusLegendario(int idLegendario, string status)
        {
            var statusValidos = new[] { "pendente", "aprovado", "reprovado" };
            if (!Array.Exists(statusValidos, s => s == status))
                return new ResponseOneDTO { Sucesso = false, Erro = "Status inválido." };

            return _repository.AtualizarStatusLegendario(idLegendario, status);
        }

        public ResponseListDTO GetLegendariosPorStatus(string status)
        {
            return _repository.GetLegendariosPorStatus(status);
        }

        public PaginatedResponse<LegendariosDTO> GetLegendariosPaginado(PaginacaoParams param)
        {
            return _repository.GetLegendariosPaginado(param);
        }
    }
}
