-- =============================================
-- Tabelas: usuarios e tokens
-- Banco: DBLEGENDARIOS
-- =============================================

CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario         INT          NOT NULL AUTO_INCREMENT,
    n_lgnd             VARCHAR(100) NOT NULL,
    chave              VARCHAR(255) NOT NULL,
    nivel_permissao    INT          NOT NULL DEFAULT 0,
    id_usuario_criacao VARCHAR(20)  NULL,
    data_edicao        DATETIME     NULL,
    id_usuario_edicao  VARCHAR(20)  NULL,
    data_delecao       DATETIME     NULL,
    id_usuario_delecao VARCHAR(20)  NULL,
    deletado           TINYINT(1)   NOT NULL DEFAULT 0,
    PRIMARY KEY (id_usuario),
    UNIQUE KEY uq_n_lgnd (n_lgnd)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS tokens (
    id_tokens  INT          NOT NULL AUTO_INCREMENT,
    token      TEXT         NOT NULL,
    id_usuario INT          NOT NULL,
    dt_acesso  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deletado   TINYINT(1)   NOT NULL DEFAULT 0,
    PRIMARY KEY (id_tokens),
    KEY idx_tokens_usuario (id_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ─────────────────────────────────────────────
-- Se a tabela já existir, execute os ALTERs abaixo manualmente:
ALTER TABLE usuarios ADD COLUMN nivel_permissao    INT         NOT NULL DEFAULT 0;
ALTER TABLE usuarios ADD COLUMN id_usuario_criacao VARCHAR(20) NULL;
ALTER TABLE usuarios ADD COLUMN data_edicao        DATETIME    NULL;
ALTER TABLE usuarios ADD COLUMN id_usuario_edicao  VARCHAR(20) NULL;
ALTER TABLE usuarios ADD COLUMN data_delecao       DATETIME    NULL;
ALTER TABLE usuarios ADD COLUMN id_usuario_delecao VARCHAR(20) NULL;
ALTER TABLE usuarios ADD COLUMN deletado           TINYINT(1)  NOT NULL DEFAULT 0;

-- ─────────────────────────────────────────────
-- Usuário administrador padrão (senha: trocar123)
-- ATENÇÃO: troque a senha antes de usar em produção!
-- ─────────────────────────────────────────────
INSERT INTO usuarios (n_lgnd, chave, nivel_permissao, id_usuario_criacao, deletado)
SELECT 'admin', 'trocar123', 1, '0', 0
WHERE NOT EXISTS (
    SELECT 1 FROM usuarios WHERE n_lgnd = 'admin'
);
