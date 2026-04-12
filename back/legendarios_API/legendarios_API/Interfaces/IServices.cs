using System.Collections.Generic;
using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Models;

namespace legendarios_API.Interfaces
{
    public interface ILegendariosService
    {
        ResponseListDTO GetAllLegendarios(LegendariosParams param);
        ResponseListDTO GetAllLegendariosAll();
        ResponseOneDTO GetLegendarioById(string idLegendario);
        ResponseOneDTO SalvarLegendarioById(LegendariosDTO legendario);
        ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario);
        ResponseListDTO GetEstatisticasDashboard();
        ResponseOneDTO AtualizarStatusLegendario(int idLegendario, string status);
        ResponseListDTO GetLegendariosPorStatus(string status);
        PaginatedResponse<LegendariosDTO> GetLegendariosPaginado(PaginacaoParams param);
    }

    public interface ILoginService
    {
        AuthResponseDTO RealizarLogin(LoginDTO parans);
        AuthResponseDTO RefreshToken(RefreshTokenRequestDTO request);
        bool VerificaSeEstaLogado(string idUsuario);
        (bool sucesso, string erro) CriarUsuario(CriarUsuarioDTO dto, int idUsuarioCriacao);
        List<Usuarios> GetTodosUsuarios();
        (bool sucesso, string erro) AtualizarUsuario(AtualizarUsuarioDTO dto, int idEdicao);
        void DeletarUsuario(int id, int idDelecao);
        void RevogarToken(string refreshToken);
    }

    public interface IAnunciosService
    {
        AnuncioResponseListDTO GetTodos();
        AnuncioResponseListDTO GetTodosAdm();
        AnuncioResponseOneDTO Criar(AnuncioDTO dto);
        AnuncioResponseOneDTO Atualizar(AnuncioDTO dto);
        AnuncioResponseOneDTO Deletar(int id);
    }

    public interface IEventosService
    {
        List<EventoResponseDTO> GetTodos(string status = null);
        EventoResponseDTO GetById(int id);
        EventoResponseDTO Criar(EventoRequestDTO dto, int idUsuario);
        bool Atualizar(EventoRequestDTO dto);
        bool Deletar(int id);
        PaginatedResponse<EventoResponseDTO> GetPaginado(PaginacaoParams param, string status = null);
    }

    public interface IInscricoesService
    {
        List<InscricaoResponseDTO> GetByEvento(int idEvento);
        List<InscricaoResponseDTO> GetByLegendario(int idLegendario);
        ApiResponse<InscricaoResponseDTO> Inscrever(InscricaoRequestDTO dto, int? inscritoPor);
        ApiResponse<bool> AtualizarStatus(int id, string status);
        ApiResponse<bool> Cancelar(int id);
    }

    public interface ICheckinService
    {
        List<CheckinResponseDTO> GetByEvento(int idEvento);
        ApiResponse<CheckinResponseDTO> RegistrarCheckin(CheckinRequestDTO dto, int? registradoPor);
        ApiResponse<bool> RegistrarCheckout(int idCheckin);
    }

    public interface IVoluntariosService
    {
        List<VoluntarioResponseDTO> GetTodos();
        VoluntarioResponseDTO GetById(int id);
        ApiResponse<VoluntarioResponseDTO> Criar(VoluntarioRequestDTO dto);
        ApiResponse<bool> Atualizar(VoluntarioRequestDTO dto);
        ApiResponse<bool> Deletar(int id);
        PaginatedResponse<VoluntarioResponseDTO> GetPaginado(PaginacaoParams param);
    }

    public interface IAuditService
    {
        void Registrar(string tabela, int idRegistro, string acao, object dadosAnteriores, object dadosNovos, int? idUsuario, string nomeUsuario, string ipAddress);
        PaginatedResponse<AuditLog> GetPaginado(PaginacaoParams param, string tabela = null);
    }
}
