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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using legendarios_API.Service;
using legendarios_API.Entity;
using System.Configuration;

namespace legendarios_API.Repository
{
    public class LegendariosRepository
    {

        static string _connectionString = "Server=187.77.245.217;port=3306;Database=DBLEGENDARIOS;Uid=root;Pwd=legendarioSenhaBanco";
        MySqlConnection _conn = new MySqlConnection(_connectionString);


        public List<LegendariosDTO> GetDept()
        {
            try
            {
                var sql = "SELECT DeptNo,DName,Location FROM dept";
                var result = this._conn.Query<LegendariosDTO>(sql).ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<LegendariosDTO>();
            }
        }

        public ResponseListDTO GetDeptByDeptNo(int deptNo)
        {
            try
            {
                var sql = "SELECT DeptNo,DName,Location FROM dept WHERE DeptNo = @DeptNo";
                var result = this._conn.Query<LegendariosDTO>(sql, new { DeptNo = deptNo }).FirstOrDefault();

                var retorno = new ResponseListDTO()
                {
                    Sucesso = true,
                   // Data = result,
                    Erro = "",
                };

                return retorno;
            }
            catch (Exception ex)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetAllLegendarios()
        {
            try
            {
                var sql = "SELECT * FROM legendarios";

                var result = this._conn.QueryAsync<LegendariosDTO>(sql).Result;

                var response = new ResponseListDTO()
                {
                    Sucesso = true,
                    Data = result.ToList(),
                    Erro = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetAllLegendariosAll()
        {
            try
            {
                var sql = "SELECT * FROM legendarios";


                var result = this._conn.QueryAsync<LegendariosDTO>(sql).Result;

                var response = new ResponseListDTO()
                {
                    Sucesso = true,
                    Data = result.ToList(),
                    Erro = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseListDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO GetLegendarioById(string IdLegendario)
        {
            try
            {
                var sql = $"SELECT * FROM legendarios where n_lgnd = {IdLegendario}";

                var result = this._conn.QueryFirstAsync<LegendariosDTO>(sql).Result;


                var response = new ResponseOneDTO
                {
                    Sucesso = true,
                    Data = result,
                    Erro = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO() { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO SalvarLegendarioById(LegendariosDTO legendario)
        {
            try
            {
                var sql = @$"UPDATE legendarios
                                                SET 
                                                id_legendario = {legendario.id_legendario}, 
                                                n_lgnd = {legendario.n_lgnd},
                                                nome = {legendario.nome},
                                                rec = {legendario.rec},
                                                celular = {legendario.celular},
                                                cadastro_pessoa = {legendario.cadastro_pessoa},
                                                data_de_nascimento = {legendario.data_de_nascimento},
                                                estado_civil = {legendario.estado_civil},
                                                profissao = {legendario.profissao},
                                                tipo_sanguineo = {legendario.tipo_sanguineo},
                                                religiao = {legendario.religiao},
                                                igreja = {legendario.igreja},
                                                e_batizado = {legendario.e_batizado},
                                                frequanta_celula = {legendario.frequanta_celula},
                                                rede = {legendario.rede},
                                                e_lider_de_celula = {legendario.e_lider_de_celula},
                                                ativo = {legendario.ativo},
                                                deletado = {legendario.deletado},
                                                
                                                WHERE id_legendario = {legendario.id_legendario} ;";

                var result = this._conn.ExecuteAsync(sql).Result;




                var response = new ResponseOneDTO()
                {
                    Sucesso = true,
                    Erro = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO() { Sucesso = false, Erro = ex.Message };
            }
        }
    }
}
