using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;

namespace legendarios_API.Repository
{
    public class LegendariosRepositoryV2 : BaseRepository, ILegendariosRepository
    {
        public LegendariosRepositoryV2(IConfiguration configuration) : base(configuration) { }

        public ResponseListDTO GetAllLegendarios()
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM legendarios WHERE deletado = 0";
                var result = conn.Query<LegendariosDTO>(sql).ToList();
                return new ResponseListDTO { Sucesso = true, Data = result, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseListDTO GetAllLegendariosAll()
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM legendarios";
                var result = conn.Query<LegendariosDTO>(sql).ToList();
                return new ResponseListDTO { Sucesso = true, Data = result, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO GetLegendarioById(string idLegendario)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = "SELECT * FROM legendarios WHERE n_lgnd = @idLegendario";
                var result = conn.QueryFirstOrDefault<LegendariosDTO>(sql, new { idLegendario });
                return new ResponseOneDTO { Sucesso = true, Data = result, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO SalvarLegendarioById(LegendariosDTO legendario)
        {
            try
            {
                using var conn = CreateConnection();
                var sql = @"UPDATE legendarios SET
                    nome = @nome, celular = @celular, cadastro_pessoa = @cadastro_pessoa,
                    data_de_nascimento = @data_de_nascimento, estado_civil = @estado_civil,
                    profissao = @profissao, tipo_sanguineo = @tipo_sanguineo,
                    religiao = @religiao, igreja = @igreja, e_batizado = @e_batizado,
                    frequenta_celula = @frequenta_celula, rede = @rede,
                    e_lider_de_celula = @e_lider_de_celula, ativo = @ativo,
                    cnh = @cnh, categoria_cnh = @categoria_cnh,
                    endereco = @endereco, cidade = @cidade, estado = @estado,
                    cep = @cep, pais = @pais,
                    emergencia_nome = @emergencia_nome, emergencia_telefone = @emergencia_telefone,
                    tamanho_camiseta = @tamanho_camiseta, observacoes = @observacoes,
                    status_cadastro = @status_cadastro
                WHERE id_legendario = @id_legendario";

                conn.Execute(sql, legendario);
                return new ResponseOneDTO { Sucesso = true, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseOneDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario)
        {
            try
            {
                using var conn = CreateConnection();
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
                     @data_cadastro, 'pendente', 1, 0);
                    SELECT LAST_INSERT_ID();";

                var id = conn.ExecuteScalar<int>(sql, new
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
                using var conn = CreateConnection();
                var sqlTotal = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0";
                var sqlPendentes = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'pendente'";
                var sqlAprovados = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'aprovado'";
                var sqlReprovados = "SELECT COUNT(*) FROM legendarios WHERE deletado = 0 AND status_cadastro = 'reprovado'";

                var total = conn.ExecuteScalar<int>(sqlTotal);
                var pendentes = conn.ExecuteScalar<int>(sqlPendentes);
                var aprovados = conn.ExecuteScalar<int>(sqlAprovados);
                var reprovados = conn.ExecuteScalar<int>(sqlReprovados);

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
                using var conn = CreateConnection();
                var sql = "UPDATE legendarios SET status_cadastro = @status WHERE id_legendario = @id";
                conn.Execute(sql, new { status, id = idLegendario });
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
                using var conn = CreateConnection();
                var sql = "SELECT * FROM legendarios WHERE deletado = 0 AND status_cadastro = @status ORDER BY data_cadastro DESC";
                var result = conn.Query<LegendariosDTO>(sql, new { status }).ToList();
                return new ResponseListDTO { Sucesso = true, Data = result, Erro = "" };
            }
            catch (Exception ex)
            {
                return new ResponseListDTO { Sucesso = false, Erro = ex.Message };
            }
        }

        public PaginatedResponse<LegendariosDTO> GetLegendariosPaginado(PaginacaoParams param)
        {
            try
            {
                using var conn = CreateConnection();
                var where = "WHERE deletado = 0";
                var parameters = new Dapper.DynamicParameters();

                if (!string.IsNullOrWhiteSpace(param.Busca))
                {
                    where += " AND (nome LIKE @busca OR email LIKE @busca OR cadastro_pessoa LIKE @busca OR celular LIKE @busca)";
                    parameters.Add("busca", $"%{param.Busca}%");
                }

                var sqlCount = $"SELECT COUNT(*) FROM legendarios {where}";
                var total = conn.ExecuteScalar<int>(sqlCount, parameters);

                var orderBy = param.OrdenarPor switch
                {
                    "nome" => "nome",
                    "data_cadastro" => "data_cadastro",
                    "status" => "status_cadastro",
                    _ => "id_legendario"
                };
                if (param.Descendente) orderBy += " DESC";

                var offset = (param.Pagina - 1) * param.TamanhoPagina;
                var sql = $"SELECT * FROM legendarios {where} ORDER BY {orderBy} LIMIT @limit OFFSET @offset";
                parameters.Add("limit", param.TamanhoPagina);
                parameters.Add("offset", offset);

                var result = conn.Query<LegendariosDTO>(sql, parameters).ToList();

                return new PaginatedResponse<LegendariosDTO>
                {
                    Sucesso = true,
                    Data = result,
                    TotalRegistros = total,
                    Pagina = param.Pagina,
                    TamanhoPagina = param.TamanhoPagina,
                    TotalPaginas = (int)Math.Ceiling(total / (double)param.TamanhoPagina)
                };
            }
            catch (Exception)
            {
                return new PaginatedResponse<LegendariosDTO> { Sucesso = false, Data = new List<LegendariosDTO>() };
            }
        }
    }
}
