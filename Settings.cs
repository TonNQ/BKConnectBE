namespace BKConnectBE
{
    public class Settings
    {
        public SqlServer SqlServer { get; set; }

        public JwtConfig JwtConfig { get; set; }

        public Emailing Emailing { get; set; }
    }

    public class SqlServer
    {
        public string User { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string DbName { get; set; }

        public string ConnectionString { get; set; }
    }

    public class JwtConfig
    {
        public string AccessTokenKey { get; set; }

        public string RefreshTokenKey { get; set; }

        public int AccessTokenExpireDays { get; set; }

        public int TemporaryCodeExpireDays { get; set; }

        public int RefreshTokenExpireMonths { get; set; }
    }

    public class Emailing
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}