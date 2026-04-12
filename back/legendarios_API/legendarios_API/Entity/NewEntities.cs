using System;

namespace legendarios_API.Entity
{
    public class Evento
    {
        public int id_evento { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime? data_fim { get; set; }
        public string local_evento { get; set; }
        public int max_vagas { get; set; }
        public int vagas_ocupadas { get; set; }
        public string status { get; set; }
        public int? id_lider { get; set; }
        public string imagem_url { get; set; }
        public bool requer_aprovacao { get; set; }
        public DateTime criado_em { get; set; }
        public DateTime atualizado_em { get; set; }
        public int? criado_por { get; set; }

        // Join fields
        public string nome_lider { get; set; }
    }

    public class Inscricao
    {
        public int id_inscricao { get; set; }
        public int id_evento { get; set; }
        public int id_legendario { get; set; }
        public string status { get; set; }
        public DateTime data_inscricao { get; set; }
        public DateTime? data_confirmacao { get; set; }
        public DateTime? data_cancelamento { get; set; }
        public string observacoes { get; set; }
        public int? inscrito_por { get; set; }

        // Join fields
        public string titulo_evento { get; set; }
        public string nome_legendario { get; set; }
    }

    public class Checkin
    {
        public int id_checkin { get; set; }
        public int id_inscricao { get; set; }
        public int id_evento { get; set; }
        public int id_legendario { get; set; }
        public DateTime data_checkin { get; set; }
        public DateTime? data_checkout { get; set; }
        public int? registrado_por { get; set; }
        public string observacoes { get; set; }

        // Join fields
        public string titulo_evento { get; set; }
        public string nome_legendario { get; set; }
    }

    public class Voluntario
    {
        public int id_voluntario { get; set; }
        public int id_legendario { get; set; }
        public string habilidades { get; set; }
        public string disponibilidade { get; set; }
        public string area_atuacao { get; set; }
        public DateTime data_cadastro { get; set; }
        public bool ativo { get; set; }
        public bool deletado { get; set; }

        // Join fields
        public string nome_legendario { get; set; }
        public string email_legendario { get; set; }
        public string celular_legendario { get; set; }
    }

    public class RefreshToken
    {
        public int id { get; set; }
        public string token { get; set; }
        public int id_usuario { get; set; }
        public DateTime expires_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? revoked_at { get; set; }
        public string replaced_by { get; set; }
        public bool IsExpired => DateTime.UtcNow >= expires_at;
        public bool IsRevoked => revoked_at != null;
        public bool IsActive => !IsRevoked && !IsExpired;
    }

    public class AuditLog
    {
        public long id { get; set; }
        public string tabela { get; set; }
        public int id_registro { get; set; }
        public string acao { get; set; }
        public string dados_anteriores { get; set; }
        public string dados_novos { get; set; }
        public int? id_usuario { get; set; }
        public string nome_usuario { get; set; }
        public string ip_address { get; set; }
        public DateTime data_acao { get; set; }
    }

    public class Notificacao
    {
        public int id { get; set; }
        public int? id_usuario { get; set; }
        public int? id_legendario { get; set; }
        public string titulo { get; set; }
        public string mensagem { get; set; }
        public string tipo { get; set; }
        public bool lida { get; set; }
        public DateTime data_criacao { get; set; }
        public DateTime? data_leitura { get; set; }
    }

    public class Documento
    {
        public int id { get; set; }
        public int? id_legendario { get; set; }
        public int? id_evento { get; set; }
        public string nome_arquivo { get; set; }
        public string tipo_documento { get; set; }
        public string caminho_arquivo { get; set; }
        public long tamanho_bytes { get; set; }
        public int? uploaded_por { get; set; }
        public DateTime data_upload { get; set; }
    }
}
