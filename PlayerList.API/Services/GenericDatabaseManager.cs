using LinqToDB;
using PlayerList.API.Interfaces;
using PlayerList.API.Services.DatabaseControllers;

namespace PlayerList.API.Services;

public class GenericDatabaseManager
{
    private readonly LiteDbController _liteDbController;
    private readonly MySqlController _mySqlController;
    private readonly bool _useLiteDb;

    public GenericDatabaseManager(LiteDbController liteDbController, MySqlController mySqlController, IConfiguration configuration)
    {
        _liteDbController = liteDbController;
        _mySqlController = mySqlController;
        _useLiteDb = configuration["Db:Method"] == "LiteDB";
    }
    
    public bool AddPlayer(Player player)
    {
        if (_useLiteDb)
        {
            if (_liteDbController.Database.GetCollection<Player>().Exists(x => player == x))
                return false;
            _liteDbController.Database.GetCollection<Player>().Insert(player);
            _liteDbController.Database.Checkpoint();
            return true;
        }
        using var db = new MySqlDataConnection(_mySqlController.ConnectionString);
        return db.Insert(player) > 0;
    }

    public bool DeletePlayer(Player player)
    {
        if (_useLiteDb)
        {
            var result = _liteDbController.Database.GetCollection<Player>().DeleteMany(x => x.Port == player.Port && x.UserId == player.UserId);
            _liteDbController.Database.Checkpoint();
            return result > 0;
        }
        using var db = new MySqlDataConnection(_mySqlController.ConnectionString);
        return db.Delete(player) > 0;
    }

    public IEnumerable<Player> GetPlayers(bool getUserIds = true)
    {
        IEnumerable<Player> players;
        if(_useLiteDb)
            players = _liteDbController.Database.GetCollection<Player>().FindAll();
        else
        {
            using var db = new MySqlDataConnection(_mySqlController.ConnectionString);
            players = db.GetTable<Player>().AsEnumerable();
        }
        foreach (var player in players)
        {
            if(!getUserIds)
                player.UserId = null;
            yield return player;
        }
    }
}