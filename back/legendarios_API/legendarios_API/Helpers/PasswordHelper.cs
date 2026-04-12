namespace legendarios_API.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(11));
        }

        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash) || !hash.StartsWith("$2"))
            {
                return password == hash;
            }
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }

    public static class RoleHelper
    {
        public const int Participante = 0;
        public const int Admin = 1;
        public const int Lider = 2;
        public const int Voluntario = 3;

        public static string GetRoleName(int nivel)
        {
            return nivel switch
            {
                Admin => "Admin",
                Lider => "Lider",
                Voluntario => "Voluntario",
                _ => "Participante"
            };
        }
    }
}
