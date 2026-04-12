-- =============================================
-- Migration V1 - Schema Completo Legendários
-- Banco: DBLEGENDARIOS (MySQL)
-- =============================================

-- ─────────────────────────────────────────────
-- TABELA: usuarios
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario         INT           NOT NULL AUTO_INCREMENT,
    n_lgnd             VARCHAR(100)  NOT NULL,
    chave              VARCHAR(255)  NOT NULL,
    email              VARCHAR(200)  NULL,
    nivel_permissao    INT           NOT NULL DEFAULT 0 COMMENT '0=Participante,1=Admin,2=Lider,3=Voluntario',
    ativo              TINYINT(1)    NOT NULL DEFAULT 1,
    id_usuario_criacao INT           NULL,
    data_criacao       DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_edicao        DATETIME      NULL,
    id_usuario_edicao  INT           NULL,
    data_delecao       DATETIME      NULL,
    id_usuario_delecao INT           NULL,
    deletado           TINYINT(1)    NOT NULL DEFAULT 0,
    PRIMARY KEY (id_usuario),
    UNIQUE KEY uq_n_lgnd (n_lgnd),
    INDEX idx_usuarios_nivel (nivel_permissao),
    INDEX idx_usuarios_ativo (ativo, deletado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: tokens (JWT tracking)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS tokens (
    id_tokens   INT          NOT NULL AUTO_INCREMENT,
    token       TEXT         NOT NULL,
    id_usuario  INT          NOT NULL,
    dt_acesso   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deletado    TINYINT(1)   NOT NULL DEFAULT 0,
    PRIMARY KEY (id_tokens),
    INDEX idx_tokens_usuario (id_usuario),
    CONSTRAINT fk_tokens_usuario FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: refresh_tokens
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS refresh_tokens (
    id              INT          NOT NULL AUTO_INCREMENT,
    token           VARCHAR(500) NOT NULL,
    id_usuario      INT          NOT NULL,
    expires_at      DATETIME     NOT NULL,
    created_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    revoked_at      DATETIME     NULL,
    replaced_by     VARCHAR(500) NULL,
    PRIMARY KEY (id),
    INDEX idx_refresh_token (token(255)),
    INDEX idx_refresh_usuario (id_usuario),
    CONSTRAINT fk_refresh_usuario FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: legendarios (participantes)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS legendarios (
    id_legendario       INT           NOT NULL AUTO_INCREMENT,
    n_lgnd              VARCHAR(50)   NULL,
    nome                VARCHAR(200)  NOT NULL,
    rec                 VARCHAR(100)  NULL,
    email               VARCHAR(200)  NULL,
    celular             VARCHAR(20)   NULL,
    cadastro_pessoa     VARCHAR(20)   NULL COMMENT 'CPF ou documento estrangeiro',
    data_de_nascimento  DATE          NULL,
    estado_civil        VARCHAR(50)   NULL,
    profissao           VARCHAR(100)  NULL,
    tipo_sanguineo      VARCHAR(10)   NULL,
    religiao            VARCHAR(100)  NULL,
    igreja              VARCHAR(200)  NULL,
    e_batizado          TINYINT(1)    NOT NULL DEFAULT 0,
    frequenta_celula    TINYINT(1)    NOT NULL DEFAULT 0,
    rede                VARCHAR(100)  NULL,
    e_lider_de_celula   TINYINT(1)    NOT NULL DEFAULT 0,
    cnh                 VARCHAR(20)   NULL,
    categoria_cnh       VARCHAR(10)   NULL,
    endereco            VARCHAR(300)  NULL,
    cidade              VARCHAR(100)  NULL,
    estado              VARCHAR(50)   NULL,
    cep                 VARCHAR(15)   NULL,
    pais                VARCHAR(50)   NULL,
    emergencia_nome     VARCHAR(200)  NULL,
    emergencia_telefone VARCHAR(20)   NULL,
    tamanho_camiseta    VARCHAR(10)   NULL,
    observacoes         TEXT          NULL,
    nivel_participacao  INT           NOT NULL DEFAULT 0 COMMENT 'Sistema de níveis/progresso',
    pontos              INT           NOT NULL DEFAULT 0,
    data_cadastro       DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    status_cadastro     VARCHAR(20)   NOT NULL DEFAULT 'pendente',
    ativo               TINYINT(1)    NOT NULL DEFAULT 1,
    deletado            TINYINT(1)    NOT NULL DEFAULT 0,
    PRIMARY KEY (id_legendario),
    INDEX idx_legendarios_status (status_cadastro),
    INDEX idx_legendarios_nome (nome),
    INDEX idx_legendarios_cpf (cadastro_pessoa),
    INDEX idx_legendarios_ativo (ativo, deletado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: anuncios
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS anuncios (
    id_anuncio      INT           NOT NULL AUTO_INCREMENT,
    titulo          VARCHAR(200)  NOT NULL,
    imagem_url      VARCHAR(500)  NOT NULL,
    texto           TEXT          NOT NULL,
    link            VARCHAR(500)  NULL,
    ativo           TINYINT(1)    NOT NULL DEFAULT 1,
    ordem           INT           NOT NULL DEFAULT 0,
    criado_em       DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em   DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    criado_por      VARCHAR(100)  NULL,
    modificado_por  VARCHAR(100)  NULL,
    PRIMARY KEY (id_anuncio),
    INDEX idx_anuncios_ativo (ativo, ordem)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: eventos
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS eventos (
    id_evento       INT           NOT NULL AUTO_INCREMENT,
    titulo          VARCHAR(200)  NOT NULL,
    descricao       TEXT          NULL,
    data_inicio     DATETIME      NOT NULL,
    data_fim        DATETIME      NULL,
    local_evento    VARCHAR(300)  NULL,
    max_vagas       INT           NOT NULL DEFAULT 0 COMMENT '0 = sem limite',
    vagas_ocupadas  INT           NOT NULL DEFAULT 0,
    status          VARCHAR(20)   NOT NULL DEFAULT 'rascunho' COMMENT 'rascunho,aberto,encerrado,cancelado',
    id_lider        INT           NULL,
    imagem_url      VARCHAR(500)  NULL,
    requer_aprovacao TINYINT(1)   NOT NULL DEFAULT 0,
    criado_em       DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em   DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    criado_por      INT           NULL,
    PRIMARY KEY (id_evento),
    INDEX idx_eventos_status (status),
    INDEX idx_eventos_data (data_inicio),
    INDEX idx_eventos_lider (id_lider),
    CONSTRAINT fk_evento_lider FOREIGN KEY (id_lider) REFERENCES usuarios(id_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: inscricoes (inscrições em eventos)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS inscricoes (
    id_inscricao      INT          NOT NULL AUTO_INCREMENT,
    id_evento         INT          NOT NULL,
    id_legendario     INT          NOT NULL,
    status            VARCHAR(20)  NOT NULL DEFAULT 'pendente' COMMENT 'pendente,confirmado,cancelado,lista_espera',
    data_inscricao    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_confirmacao  DATETIME     NULL,
    data_cancelamento DATETIME     NULL,
    observacoes       TEXT         NULL,
    inscrito_por      INT          NULL,
    PRIMARY KEY (id_inscricao),
    UNIQUE KEY uq_inscricao_evento_legendario (id_evento, id_legendario),
    INDEX idx_inscricoes_evento (id_evento),
    INDEX idx_inscricoes_legendario (id_legendario),
    INDEX idx_inscricoes_status (status),
    CONSTRAINT fk_inscricao_evento FOREIGN KEY (id_evento) REFERENCES eventos(id_evento),
    CONSTRAINT fk_inscricao_legendario FOREIGN KEY (id_legendario) REFERENCES legendarios(id_legendario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: checkins (presença em eventos)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS checkins (
    id_checkin      INT          NOT NULL AUTO_INCREMENT,
    id_inscricao    INT          NOT NULL,
    id_evento       INT          NOT NULL,
    id_legendario   INT          NOT NULL,
    data_checkin    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_checkout   DATETIME     NULL,
    registrado_por  INT          NULL,
    observacoes     VARCHAR(500) NULL,
    PRIMARY KEY (id_checkin),
    UNIQUE KEY uq_checkin_inscricao (id_inscricao),
    INDEX idx_checkin_evento (id_evento),
    INDEX idx_checkin_legendario (id_legendario),
    CONSTRAINT fk_checkin_inscricao FOREIGN KEY (id_inscricao) REFERENCES inscricoes(id_inscricao),
    CONSTRAINT fk_checkin_evento FOREIGN KEY (id_evento) REFERENCES eventos(id_evento),
    CONSTRAINT fk_checkin_legendario FOREIGN KEY (id_legendario) REFERENCES legendarios(id_legendario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: voluntarios
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS voluntarios (
    id_voluntario     INT          NOT NULL AUTO_INCREMENT,
    id_legendario     INT          NOT NULL,
    habilidades       TEXT         NULL,
    disponibilidade   VARCHAR(200) NULL,
    area_atuacao      VARCHAR(100) NULL,
    data_cadastro     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ativo             TINYINT(1)   NOT NULL DEFAULT 1,
    deletado          TINYINT(1)   NOT NULL DEFAULT 0,
    PRIMARY KEY (id_voluntario),
    INDEX idx_voluntarios_legendario (id_legendario),
    INDEX idx_voluntarios_ativo (ativo, deletado),
    CONSTRAINT fk_voluntario_legendario FOREIGN KEY (id_legendario) REFERENCES legendarios(id_legendario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: evento_voluntarios (ligação evento-voluntário)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS evento_voluntarios (
    id               INT          NOT NULL AUTO_INCREMENT,
    id_evento        INT          NOT NULL,
    id_voluntario    INT          NOT NULL,
    funcao           VARCHAR(100) NULL,
    data_atribuicao  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    UNIQUE KEY uq_evento_voluntario (id_evento, id_voluntario),
    CONSTRAINT fk_ev_evento FOREIGN KEY (id_evento) REFERENCES eventos(id_evento),
    CONSTRAINT fk_ev_voluntario FOREIGN KEY (id_voluntario) REFERENCES voluntarios(id_voluntario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: audit_log (auditoria)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS audit_log (
    id              BIGINT       NOT NULL AUTO_INCREMENT,
    tabela          VARCHAR(100) NOT NULL,
    id_registro     INT          NOT NULL,
    acao            VARCHAR(20)  NOT NULL COMMENT 'INSERT,UPDATE,DELETE',
    dados_anteriores JSON        NULL,
    dados_novos     JSON         NULL,
    id_usuario      INT          NULL,
    nome_usuario    VARCHAR(100) NULL,
    ip_address      VARCHAR(50)  NULL,
    data_acao       DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    INDEX idx_audit_tabela (tabela, id_registro),
    INDEX idx_audit_usuario (id_usuario),
    INDEX idx_audit_data (data_acao)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: notificacoes
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS notificacoes (
    id              INT          NOT NULL AUTO_INCREMENT,
    id_usuario      INT          NULL,
    id_legendario   INT          NULL,
    titulo          VARCHAR(200) NOT NULL,
    mensagem        TEXT         NOT NULL,
    tipo            VARCHAR(50)  NOT NULL DEFAULT 'info' COMMENT 'info,sucesso,alerta,erro',
    lida            TINYINT(1)   NOT NULL DEFAULT 0,
    data_criacao    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_leitura    DATETIME     NULL,
    PRIMARY KEY (id),
    INDEX idx_notif_usuario (id_usuario, lida),
    INDEX idx_notif_legendario (id_legendario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- TABELA: documentos (upload de termos, etc.)
-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS documentos (
    id              INT          NOT NULL AUTO_INCREMENT,
    id_legendario   INT          NULL,
    id_evento       INT          NULL,
    nome_arquivo    VARCHAR(300) NOT NULL,
    tipo_documento  VARCHAR(100) NOT NULL,
    caminho_arquivo VARCHAR(500) NOT NULL,
    tamanho_bytes   BIGINT       NOT NULL DEFAULT 0,
    uploaded_por    INT          NULL,
    data_upload     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    INDEX idx_doc_legendario (id_legendario),
    INDEX idx_doc_evento (id_evento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- Usuário administrador padrão
-- Senha: Admin@123 (hash BCrypt)
-- ─────────────────────────────────────────────
INSERT INTO usuarios (n_lgnd, chave, nivel_permissao, id_usuario_criacao, deletado)
SELECT 'admin', '$2a$11$K3GRGx1V0VBxjSvSzGzLHePFRx0Vg7o3rPfs.MdWJHG7SDRAZ3TGi', 1, NULL, 0
WHERE NOT EXISTS (
    SELECT 1 FROM usuarios WHERE n_lgnd = 'admin'
);
