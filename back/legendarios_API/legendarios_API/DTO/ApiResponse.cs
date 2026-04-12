using System.Collections.Generic;

namespace legendarios_API.DTO
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T Data { get; set; }
        public List<string> Erros { get; set; }

        public static ApiResponse<T> Ok(T data, string mensagem = null)
        {
            return new ApiResponse<T> { Sucesso = true, Data = data, Mensagem = mensagem };
        }

        public static ApiResponse<T> Erro(string mensagem, List<string> erros = null)
        {
            return new ApiResponse<T> { Sucesso = false, Mensagem = mensagem, Erros = erros };
        }
    }

    public class PaginatedResponse<T>
    {
        public bool Sucesso { get; set; }
        public List<T> Data { get; set; }
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class PaginacaoParams
    {
        private const int MaxTamanhoPagina = 100;
        private int _tamanhoPagina = 20;

        public int Pagina { get; set; } = 1;
        public int TamanhoPagina
        {
            get => _tamanhoPagina;
            set => _tamanhoPagina = value > MaxTamanhoPagina ? MaxTamanhoPagina : value;
        }
        public string Busca { get; set; }
        public string OrdenarPor { get; set; }
        public bool Descendente { get; set; }
    }
}
