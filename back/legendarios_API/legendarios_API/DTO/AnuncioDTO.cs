namespace legendarios_API.DTO
{
    public class AnuncioDTO
    {
        public int? id_anuncio { get; set; }
        public string titulo { get; set; }
        public string imagem_url { get; set; }
        public string texto { get; set; }
        public string link { get; set; }
        public bool ativo { get; set; } = true;
        public int ordem { get; set; } = 0;
    }
}
