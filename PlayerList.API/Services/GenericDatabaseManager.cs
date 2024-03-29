﻿using LinqToDB;
using LinqToDB.Data;
using PlayerList.API.Interfaces;
using PlayerList.API.Services.DatabaseControllers;

namespace PlayerList.API.Services;

public class GenericDatabaseManager
{
    private readonly LiteDbController _liteDbController;
    public readonly bool UseLiteDb;

    public GenericDatabaseManager(LiteDbController liteDbController, IConfiguration configuration)
    {
        _liteDbController = liteDbController;
        UseLiteDb = configuration["Db:Method"] == "LiteDB";
    }

    public bool AddPlayer(Player player)
    {
        if (UseLiteDb)
        {
            if (_liteDbController.Database.GetCollection<Player>().Exists(x => player == x))
                return false;
            _liteDbController.Database.GetCollection<Player>().Insert(player);
            _liteDbController.Database.Checkpoint();
            return true;
        }

        using var db = new DataConnection();
        return db.Insert(player) > 0;
    }

    public bool DeletePlayer(Player player)
    {
        if (UseLiteDb)
        {
            var result = _liteDbController.Database.GetCollection<Player>()
                .DeleteMany(x => x.Port == player.Port && x.UserId == player.UserId);
            _liteDbController.Database.Checkpoint();
            return result > 0;
        }

        using var db = new DataConnection();
        return db.Delete(player) > 0;
    }

    public IEnumerable<Player> GetPlayers(int port, bool getUserIds = true)
    {
        IEnumerable<Player> players;
        if (UseLiteDb)
            players = _liteDbController.Database.GetCollection<Player>().FindAll();
        else
        {
            using var db = new DataConnection();
            players = db.GetTable<Player>().AsEnumerable();
        }

        foreach (var player in players)
        {
            if (!getUserIds)
                player.UserId = null;
            if (port == 0)
                yield return player;
            if (player.Port == port)
                yield return player;
        }
    }
}