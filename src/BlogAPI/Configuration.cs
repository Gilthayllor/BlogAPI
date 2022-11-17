namespace BlogAPI
{
    public class Configuration
    {
        public static string JwtKey = "OTllZmNjYTctZWEwZS00MGUxLWFiNDAtY2IxNWNmNDlkOTll";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "blog_api_OWY2ZDA4ODgtNmJkMi00NDk2LThiNWQtMjQyMjAxNmM2ZDgx";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
