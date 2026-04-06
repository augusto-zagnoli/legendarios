using System;

namespace legendarios_API.Entity
{
    public class Anuncio
    {
        public int id_anuncio { get; set; }
        public string titulo { get; set; }
        public string imagem_url { get; set; }
        public string texto { get; set; }
        public string link { get; set; }
        public bool ativo { get; set; }
        public int ordem { get; set; }
        public DateTime criado_em { get; set; }
        public DateTime atualizado_em { get; set; }
    }

    public class AnuncioResponseListDTO
    {
        public bool Sucesso { get; set; }
        public string Erro { get; set; }
        public System.Collections.Generic.List<Anuncio> Data { get; set; }
    }

    public class AnuncioResponseOneDTO
    {
        public bool Sucesso { get; set; }
        public string Erro { get; set; }
        public Anuncio Data { get; set; }
    }
}
