using System.Collections.Generic;
using legendarios_API.DTO;
using legendarios_API.Entity;

namespace legendarios_API.Interfaces
{
    public interface IBaseRepository
    {
        MySqlConnector.MySqlConnection CreateConnection();
    }

    public interface ILegendariosRepository
    {
        ResponseListDTO GetAllLegendarios();
        ResponseListDTO GetAllLegendariosAll();
        ResponseOneDTO GetLegendarioById(string idLegendario);
        ResponseOneDTO SalvarLegendarioById(LegendariosDTO legendario);
        ResponseOneDTO CadastrarLegendario(LegendariosDTO legendario);
        ResponseListDTO GetEstatisticasDashboard();
        ResponseOneDTO AtualizarStatusLegendario(int idLegendario, string status);
        ResponseListDTO GetLegendariosPorStatus(string status);
        PaginatedResponse<LegendariosDTO> GetLegendariosPaginado(PaginacaoParams param);
    }

    public interface ILoginRepository
    {
        Usuarios GetUsuariosIdChave(string usuario, string senha);
        Usuarios GetUsuarioById(int id);
        int AtualizaToken(string idUsuario, string token);
        List<Tokens> GetTokens(string usuario);
        bool LoginExiste(string login);
        void CriarUsuario(string login, string senhaHash, int nivelPermissao, int idUsuarioCriacao);
        List<Usuarios> GetTodosUsuarios();
        void AtualizarUsuario(int id, string login, int nivelPermissao, string novaSenhaHash, int idEdicao);
        void DeletarUsuario(int id, int idDelecao);
        void SalvarRefreshToken(int idUsuario, string token, System.DateTime expiresAt);
        RefreshToken GetRefreshToken(string token);
        void RevogarRefreshToken(string token, string replacedBy);
    }

    public interface IAnunciosRepository
    {
        AnuncioResponseListDTO GetTodos();
        AnuncioResponseListDTO GetTodosAdm();
        AnuncioResponseOneDTO Criar(AnuncioDTO dto);
        AnuncioResponseOneDTO Atualizar(AnuncioDTO dto);
        AnuncioResponseOneDTO Deletar(int id);
    }

    public interface IEventosRepository
    {
        List<Evento> GetTodos(string status = null);
        Evento GetById(int id);
        Evento Criar(Evento evento);
        bool Atualizar(Evento evento);
        bool Deletar(int id);
        PaginatedResponse<Evento> GetPaginado(PaginacaoParams param, string status = null);
        DashboardDTO GetDashboardStats();
    }

    public interface IInscricoesRepository
    {
        List<Inscricao> GetByEvento(int idEvento);
        List<Inscricao> GetByLegendario(int idLegendario);
        Inscricao GetById(int id);
        Inscricao Criar(Inscricao inscricao);
        bool AtualizarStatus(int id, string status);
        bool Cancelar(int id);
        bool ExisteInscricao(int idEvento, int idLegendario);
        int ContarInscritosPorEvento(int idEvento);
    }

    public interface ICheckinRepository
    {
        List<Checkin> GetByEvento(int idEvento);
        Checkin GetById(int id);
        Checkin Criar(Checkin checkin);
        bool RegistrarCheckout(int idCheckin);
        bool ExisteCheckin(int idInscricao);
    }

    public interface IVoluntariosRepository
    {
        List<Voluntario> GetTodos();
        Voluntario GetById(int id);
        Voluntario Criar(Voluntario voluntario);
        bool Atualizar(Voluntario voluntario);
        bool Deletar(int id);
        PaginatedResponse<Voluntario> GetPaginado(PaginacaoParams param);
    }

    public interface IAuditRepository
    {
        void Registrar(string tabela, int idRegistro, string acao, string dadosAnteriores, string dadosNovos, int? idUsuario, string nomeUsuario, string ipAddress);
        List<AuditLog> GetByTabela(string tabela, int? idRegistro = null);
        PaginatedResponse<AuditLog> GetPaginado(PaginacaoParams param, string tabela = null);
    }
}
