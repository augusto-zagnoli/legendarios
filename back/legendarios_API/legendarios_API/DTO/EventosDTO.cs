using System;

namespace legendarios_API.DTO
{
    public class EventoRequestDTO
    {
        public int? IdEvento { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string LocalEvento { get; set; }
        public int MaxVagas { get; set; }
        public string Status { get; set; }
        public int? IdLider { get; set; }
        public string ImagemUrl { get; set; }
        public bool RequerAprovacao { get; set; }
    }

    public class EventoResponseDTO
    {
        public int IdEvento { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string LocalEvento { get; set; }
        public int MaxVagas { get; set; }
        public int VagasOcupadas { get; set; }
        public int VagasDisponiveis => MaxVagas == 0 ? int.MaxValue : MaxVagas - VagasOcupadas;
        public string Status { get; set; }
        public int? IdLider { get; set; }
        public string NomeLider { get; set; }
        public string ImagemUrl { get; set; }
        public bool RequerAprovacao { get; set; }
        public DateTime CriadoEm { get; set; }
    }

    public class InscricaoRequestDTO
    {
        public int IdEvento { get; set; }
        public int IdLegendario { get; set; }
        public string Observacoes { get; set; }
    }

    public class InscricaoResponseDTO
    {
        public int IdInscricao { get; set; }
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; }
        public int IdLegendario { get; set; }
        public string NomeLegendario { get; set; }
        public string Status { get; set; }
        public DateTime DataInscricao { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public string Observacoes { get; set; }
    }

    public class CheckinRequestDTO
    {
        public int IdInscricao { get; set; }
        public int IdEvento { get; set; }
        public int IdLegendario { get; set; }
        public string Observacoes { get; set; }
    }

    public class CheckinResponseDTO
    {
        public int IdCheckin { get; set; }
        public int IdInscricao { get; set; }
        public int IdEvento { get; set; }
        public string TituloEvento { get; set; }
        public int IdLegendario { get; set; }
        public string NomeLegendario { get; set; }
        public DateTime DataCheckin { get; set; }
        public DateTime? DataCheckout { get; set; }
        public string Observacoes { get; set; }
    }

    public class VoluntarioRequestDTO
    {
        public int? IdVoluntario { get; set; }
        public int IdLegendario { get; set; }
        public string Habilidades { get; set; }
        public string Disponibilidade { get; set; }
        public string AreaAtuacao { get; set; }
    }

    public class VoluntarioResponseDTO
    {
        public int IdVoluntario { get; set; }
        public int IdLegendario { get; set; }
        public string NomeLegendario { get; set; }
        public string EmailLegendario { get; set; }
        public string CelularLegendario { get; set; }
        public string Habilidades { get; set; }
        public string Disponibilidade { get; set; }
        public string AreaAtuacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }

    public class DashboardDTO
    {
        public int TotalLegendarios { get; set; }
        public int LegendariosPendentes { get; set; }
        public int LegendariosAprovados { get; set; }
        public int LegendariosReprovados { get; set; }
        public int TotalEventos { get; set; }
        public int EventosAtivos { get; set; }
        public int TotalVoluntarios { get; set; }
        public int TotalInscricoes { get; set; }
        public int CheckinsHoje { get; set; }
    }

    public class RefreshTokenRequestDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int NivelPermissao { get; set; }
        public string Role { get; set; }
    }
}
