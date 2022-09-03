using LiteDB;
using PlayerList.API.Interfaces;

namespace PlayerList.API.Services.DatabaseControllers;

public class LiteDbController
{
    public LiteDatabase Database { get; }
    
    public LiteDbController(IConfiguration configuration)
    {
        if(configuration["Db:Method"] != "LiteDB")
            return;
        Database = new LiteDatabase(configuration["Db:LiteDb:File"]);
        Database.GetCollection<Player>().EnsureIndex(x => x.UserId);
    }
}