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

        public ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario)
        {
            try
            {
                var sql = @"INSERT INTO legendarios
                    (nome, email, celular, cadastro_pessoa, data_de_nascimento, estado_civil,
                     profissao, tipo_sanguineo, religiao, cnh, categoria_cnh,
                     endereco, cidade, estado, cep, pais,
                     emergencia_nome, emergencia_telefone, tamanho_camiseta, observacoes,
                     data_cadastro, status_cadastro, ativo, deletado)
                    VALUES
                    (@nome, @email, @celular, @cadastro_pessoa, @data_de_nascimento, @estado_civil,
                     @profissao, @tipo_sanguineo, @religiao, @cnh, @categoria_cnh,
                     @endereco, @cidade, @estado, @cep, @pais,
                     @emergencia_nome, @emergencia_telefone, @tamanho_camiseta, @observacoes,
                     @data_cadastro, 'pendente', 1, 0)";

                this._conn.Execute(sql, new
                {
                    legendario.nome,
                    legendario.email,
                    legendario.celular,
                    legendario.cadastro_pessoa,
                    legendario.data_de_nascimento,
                    legendario.estado_civil,
                    legendario.profissao,
                    legendario.tipo_sanguineo,
                    legendario.religiao,
                    legendario.cnh,
                    legendario.categoria_cnh,
                    legendario.endereco,
                    legendario.cidade,
                    legendario.estado,
                    legendario.cep,
                    legendario.pais,
                    legendario.emergencia_nome,
                    legendario.emergencia_telefone,
                    legendario.tamanho_camiseta,
                    legendario.observacoes,
                    data_cadastro = DateTime.Now
                });

                return new ResponseOneDTO { Sucesso = true, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetEstatisticasDashboard()
        {
            try
            {
                var sqlTotal = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0";
                var sqlPendentes = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'pendente'";
                var sqlAprovados = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'aprovado'";
                var sqlReprovados = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'reprovado'";

                var total = this._conn.ExecuteScalar<int>(sqlTotal);
                var pendentes = this._conn.ExecuteScalar<int>(sqlPendentes);
                var aprovados = this._conn.ExecuteScalar<int>(sqlAprovados);
                var reprovados = this._conn.ExecuteScalar<int>(sqlReprovados);

                return new ResponseListDTO
                {
                    Sucesso = true,
                    Erro = $"{total}|{pendentes}|{aprovados}|{reprovados}"
                };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO AtualizarStatusLegendario(int idLegendario, string status)
        {
            try
            {
                var sql = "UPDATE legendarios SET status_cadastro = @status WHERE id_legendario = @id";
                this._conn.Execute(sql, new { status, id = idLegendario });
                return new ResponseOneDTO { Sucesso = true, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetLegendariosPorStatus(string status)
        {
            try
            {
                var sql = "SELECT * FROM legendarios WHERE deletado = 0 AND status_cadastro = @status ORDER BY data_cadastro DESC";
                var result = this._conn.Query<LegendariosDTO>(sql, new { status }).ToList();
                return new ResponseListDTO { Sucesso = true, Data = result, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO GetLegendarioById(string IdLegendario)
        {
            try
            {
                var sql = "SELECT * FROM legendarios WHERE n_lgnd = @idLegendario";
                var result = this._conn.QueryFirst<LegendariosDTO>(sql, new { idLegendario = IdLegendario });
                return new ResponseOneDTO { Sucesso = true, Data = result, Erro = "" };
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
                var sql = @"UPDATE legendarios SET
                                nome = @nome,
                                celular = @celular,
                                cadastro_pessoa = @cadastro_pessoa,
                                data_de_nascimento = @data_de_nascimento,
                                estado_civil = @estado_civil,
                                profissao = @profissao,
                                tipo_sanguineo = @tipo_sanguineo,
                                religiao = @religiao,
                                igreja = @igreja,
                                e_batizado = @e_batizado,
                                frequenta_celula = @frequenta_celula,
                                rede = @rede,
                                e_lider_de_celula = @e_lider_de_celula,
                                ativo = @ativo,
                                cnh = @cnh,
                                categoria_cnh = @categoria_cnh,
                                endereco = @endereco,
                                cidade = @cidade,
                                estado = @estado,
                                cep = @cep,
                                pais = @pais,
                                emergencia_nome = @emergencia_nome,
                                emergencia_telefone = @emergencia_telefone,
                                tamanho_camiseta = @tamanho_camiseta,
                                observacoes = @observacoes,
                                status_cadastro = @status_cadastro
                            WHERE id_legendario = @id_legendario";

                this._conn.Execute(sql, legendario);
                return new ResponseOneDTO { Sucesso = true, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO() { Sucesso = false, Erro = ex.Message };
            }
        }
    }
}
