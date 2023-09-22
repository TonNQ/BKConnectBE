namespace BKConnectBE;

public class Settings
{
    public SqlServer sqlServer { get; set; }   
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

    public int RefreshTokenExpireMonths { get; set; }
}