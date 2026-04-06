namespace legendarios_API.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int NivelPermissao { get; set; }
    }
}
