using System.ComponentModel;
using Exiled.API.Interfaces;

namespace PlayerList.Plugin
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; }
        
        [Description("BaseUrl where the API is running")]
        public string BaseUrl { get; set; } = "http://localhost:5000";
        
        [Description("Specify the ApiKey to use with the API")]
        public string ApiKey { get; set; } = "CHANGE ME";
        
    }
}