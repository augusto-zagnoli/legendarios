using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Security.Permissions;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using legendarios_API.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using legendarios_API.Service;
using legendarios_API.Repository;
using legendarios_API.Entity;
using legendarios_API.Models;

namespace legendarios_API.Service
{
    public class LegendariosService
    {

        static string _connectionString = "Server=89.117.32.251;port=33;Database=DBLEGENDARIOS;Uid=root;Pwd=97514815";
        LegendariosRepository legendariosRepository = new LegendariosRepository();

        public IEnumerable<LegendariosDTO> GetDept()
        {
            var result = legendariosRepository.GetDept();

            return result;
        }

        public ResponseListDTO GetDeptByDeptNo(int deptNo)
        {
            var result = legendariosRepository.GetDeptByDeptNo(deptNo);
            return result;
        }

        public ResponseListDTO GetAllLegendarios(LegendariosParams param)
        {
            try
            {

                var result = legendariosRepository.GetAllLegendarios();

                var response = result.Data.ToList();

                if (!string.IsNullOrEmpty(param.NOMELEGENDARIO))
                {
                    response = result.Data.Where(x => x.nome.ToUpper().Contains(param.NOMELEGENDARIO.ToUpper())).ToList();
                }

                if (param.CODIGOLEGENDARIO != null && param.CODIGOLEGENDARIO != 0)
                {
                    response = result.Data.Where(x => x.n_lgnd.ToString().Contains(param.CODIGOLEGENDARIO.ToString())).ToList();
                }

                var results = new ResponseListDTO()
                {
                    Sucesso = true,
                    Data = response,
                    Erro = ""
                };

                return results;
            }
            catch (Exception ex)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public async Task<ResponseListDTO> GetAllLegendariosAll()
        {
            try
            {

                var result = legendariosRepository.GetAllLegendariosAll();

                var response = result;

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public async Task<ResponseOneDTO> GetLegendarioById(string IdLegendario)
        {
            try
            {
                var result = legendariosRepository.GetLegendarioById(IdLegendario);

                return result;
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public async Task<ResponseOneDTO> SalvarLegendarioById(LegendariosDTO Legendario)
        {
            try
            {
                var result = legendariosRepository.SalvarLegendarioById(Legendario);

                return result;
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario)
        {
            return legendariosRepository.CadastrarLegendario(legendario);
        }

        public ResponseListDTO GetEstatisticasDashboard()
        {
            return legendariosRepository.GetEstatisticasDashboard();
        }

        public ResponseOneDTO AtualizarStatusLegendario(int idLegendario, string status)
        {
            var statusValidos = new[] { "pendente", "aprovado", "reprovado" };
            if (!Array.Exists(statusValidos, s => s == status))
                return new ResponseOneDTO { Sucesso = false, Erro = "Status inválido." };

            return legendariosRepository.AtualizarStatusLegendario(idLegendario, status);
        }

        public ResponseListDTO GetLegendariosPorStatus(string status)
        {
            return legendariosRepository.GetLegendariosPorStatus(status);
        }
    }
}
