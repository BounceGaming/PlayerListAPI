namespace PlayerList.Wrapper.Interfaces
{
    public class Player
    {
        public Player(int port, string nickname, string? userId)
        {
            Port = port;
            Nickname = nickname;
            UserId = userId;
        }
        
        public int Port { get; set; }
        public string Nickname { get; set; }
        public string? UserId { get; set; }
    }
}