using System.ComponentModel.DataAnnotations.Schema;
using LiteDB;

namespace PlayerList.API.Interfaces;

[Table("players")]
public class Player
{
    [Column("port")]
    public int Port { get; set; }
    [Column("nickname")]
    public string Nickname { get; set; }
    [Column("userId"), BsonId]
    public string? UserId { get; set; }
}