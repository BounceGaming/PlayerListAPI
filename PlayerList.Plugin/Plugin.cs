using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using PlayerList.Wrapper;

namespace PlayerList.Plugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "BounceGaming-Team";
        public override string Name { get; } = typeof(Plugin).Namespace;
        public override string Prefix => Name.ToLower();
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 3, 0);
        
        public PlayerListWrapper PlayerListWrapper { get; private set; }

        public override void OnEnabled()
        {
            PlayerListWrapper = new PlayerListWrapper(Config.BaseUrl, Config.ApiKey);
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            PlayerListWrapper = null;
            base.OnDisabled();
        }
        
        private void OnVerified(VerifiedEventArgs ev)
            => PlayerListWrapper.CreatePlayer(new Wrapper.Interfaces.Player(Server.Port,ev.Player.Nickname,ev.Player.UserId));
        
        private void OnLeft(LeftEventArgs ev)
            => PlayerListWrapper.DeletePlayer(new Wrapper.Interfaces.Player(Server.Port,ev.Player.Nickname,ev.Player.UserId));
        
        private void OnWaitingForPlayers() 
            => PlayerListWrapper.DeleteAllPlayers();
    }
}