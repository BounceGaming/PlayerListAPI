using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using PlayerList.API.Interfaces;

namespace PlayerList.API.Services.DatabaseControllers;

public class MySqlController
{
    public MySqlController(IConfiguration configuration)
    {
        if (configuration["Db:Method"] != "MySQL")
            return;
        DataConnection.DefaultSettings = new DbSettings(configuration);
        ConnectionString = DataConnection.DefaultSettings.ConnectionStrings.FirstOrDefault()!.ConnectionString;
    }

    public string ConnectionString { get; }
    
    public void CreateDatabase()
    {
        using var db = new DataConnection();
        try
        {
            _ = db.GetTable<Player>().Any();
        }
        catch
        {
            db.CreateTable<Player>();
        }
    }
}

public class DbSettings : ILinqToDBSettings
{
    private readonly IConfiguration _configuration;

    public DbSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<IDataProviderSettings> DataProviders
        => Enumerable.Empty<IDataProviderSettings>();

    public string DefaultConfiguration => "SqlServer";
    public string DefaultDataProvider => "SqlServer";

    public IEnumerable<IConnectionStringSettings> ConnectionStrings => new[]
    {
        new ConnectionStringSettings(_configuration["Db:MySql:Database"],
            $"Server={_configuration["Db:MySql:Host"]}; " +
            $"Database={_configuration["Db:MySql:Database"]}; " +
            $"User Id={_configuration["Db:MySql:Username"]}; " +
            $"Password={_configuration["Db:MySql:Password"]};",
            ProviderName.MySql)
    };
}