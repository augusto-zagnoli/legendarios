namespace legendarios_API.DTO
{
    public class LoginDTO
    {
        public string login { get; set; }
        public string password { get; set; }
        public bool rememberUser { get; set; }
    }

    public class CriarUsuarioDTO
    {
        public string login { get; set; }
        public string senha { get; set; }
        public int nivel_permissao { get; set; }
    }

    public class AtualizarUsuarioDTO
    {
        public int id_usuario { get; set; }
        public string login { get; set; }
        public int nivel_permissao { get; set; }
        public string nova_senha { get; set; }
    }
}
