using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlayerList.Wrapper.Interfaces;

namespace PlayerList.Wrapper
{
    public class PlayerListWrapper
    {
        private readonly string _authToken;
        private readonly string _baseUrl;
        public PlayerListWrapper(string baseUrl, string authToken)
        {
            _authToken = authToken;
            _baseUrl = baseUrl;
        }

        public IReadOnlyCollection<Player> GetPlayers(bool getUserId = true, int port = 0)
            => CreateRequest<IReadOnlyCollection<Player>>($"playerlist{(getUserId ? "?getUserId=true" : "")}" + 
                                                          $"{(port != 0 ? getUserId ? $"&port={port}" : $"?port={port}" : "")}", HttpMethodType.Get).Response;

        public async Task<IReadOnlyCollection<Player>> GetPlayersAsync(bool getUserId = true, int port = 0)
            => (await CreateRequestAsync<IReadOnlyCollection<Player>>(
                $"playerlist{(getUserId ? "?getUserId=true" : "")}" +
                $"{(port != 0 ? getUserId ? $"&port={port}" : $"?port={port}" : "")}", HttpMethodType.Get)).Response;

        public bool DeletePlayer(Player player)
            => CreateRequest("playerlist", HttpMethodType.Delete, player).StatusCode == HttpStatusCode.OK;
        
        public async Task<bool> DeletePlayerAsync(Player player)
            => (await CreateRequestAsync("playerlist", HttpMethodType.Delete, player)).StatusCode == HttpStatusCode.OK;
        
        public bool CreatePlayer(Player player)
            => CreateRequest("playerlist", HttpMethodType.Put, player).StatusCode == HttpStatusCode.OK;
        
        public async Task<bool> CreatePlayerPlayerAsync(Player player)
            => (await CreateRequestAsync("playerlist", HttpMethodType.Put, player)).StatusCode == HttpStatusCode.OK;

        public bool DeleteAllPlayers()
        {
            var players = GetPlayers();
            if (players.Count == 0)
                return false;
            foreach (var player in players)
                DeletePlayer(player);
            return true;
        }
        
        public async Task<bool> DeleteAllPlayersAsync()
        {
            var players = await GetPlayersAsync();
            if (players.Count == 0)
                return false;
            foreach (var player in players)
                await DeletePlayerAsync(player);
            return true;
        }

        private (HttpStatusCode StatusCode, T? Response) CreateRequest<T>(string url, string method, object? input = null) where T : class
        {
            var request = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{url}");
            request.Headers["UserAgent"] = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36";
            request.Headers[HttpRequestHeader.Authorization] = _authToken;
            request.ContentType = "application/json";
            request.Method = method;
            if (input != null)
            {
                using var outgoingData = new StreamWriter(request.GetRequestStream());
                outgoingData.Write(JsonConvert.SerializeObject(input));   
            }
            
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
                return (response.StatusCode, null);
            var responseString = new StreamReader(responseStream).ReadToEnd();
            return (response.StatusCode, responseString.StartsWith("{") ? JsonConvert.DeserializeObject<T>(responseString) : responseString as T);
        }
        
        private async Task<(HttpStatusCode StatusCode, T? Response)> CreateRequestAsync<T>(string url, string method, object? input = null) where T : class
        {
            var request = (HttpWebRequest)WebRequest.Create($"{_baseUrl}/{url}");
            request.Headers["UserAgent"] = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36";
            request.Headers[HttpRequestHeader.Authorization] = _authToken;
            request.ContentType = "application/json";
            request.Method = method;
            if (input != null)
            {
                using var outgoingData = new StreamWriter(await request.GetRequestStreamAsync());
                await outgoingData.WriteAsync(JsonConvert.SerializeObject(input));
            }
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();
            }
            catch(WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
                return (response.StatusCode, null);
            var responseString = await new StreamReader(responseStream).ReadToEndAsync();
            return (response.StatusCode, responseString.StartsWith("{") ? JsonConvert.DeserializeObject<T>(responseString) : responseString as T);
        }

        private (HttpStatusCode StatusCode, object? Response) CreateRequest(string url, string method, object? input = null)
        {
            var request = CreateRequest<object>(url, method, input);
            return (request.StatusCode, request.Response);
        }

        private async Task<(HttpStatusCode StatusCode, object? Response)> CreateRequestAsync(string url, string method, object? input = null)
        {
            var request = await CreateRequestAsync<object>(url, method, input);
            return (request.StatusCode, request.Response);
        }
    }
}