-- =============================================
-- Tabela: anuncios
-- Banco: DBLEGENDARIOS
-- =============================================

CREATE TABLE IF NOT EXISTS anuncios (
    id_anuncio    INT           NOT NULL AUTO_INCREMENT,
    titulo        VARCHAR(200)  NOT NULL,
    imagem_url    VARCHAR(500)  NOT NULL,
    texto         TEXT          NOT NULL,
    link          VARCHAR(500)  NULL,
    ativo         TINYINT(1)    NOT NULL DEFAULT 1,
    ordem         INT           NOT NULL DEFAULT 0,
    criado_em     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (id_anuncio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Exemplo de insert
INSERT INTO anuncios (titulo, imagem_url, texto, link, ativo, ordem)
VALUES (
  'Ação Social - Distribuição de Cestas Básicas',
  'https://legendarios.cloud/assets/img/acao1.jpg',
  'No próximo sábado estaremos distribuindo cestas básicas para famílias carentes da região. Venha fazer parte dessa ação!',
  'https://wa.me/5531999999999',
  1,
  1
);
