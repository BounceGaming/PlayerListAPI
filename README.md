# PlayerListAPI
Gives a new way to get your server's player list in real time, better in most cases than using Northwood's API.

# Use cases
You can use this solution for:
- Discord Bots
- Webs

# Installation (Self hosted)
- Download the API, Wrapper and Plugin [from here](https://github.com/BounceGaming/PlayerListAPI/releases/latest)
- Install the plugin inside ``.config/EXILED/Plugins`` and the dependency inside ``.config/EXILED/Plugins/dependencies``.
- Create a new folder and decompress the API.tar.gz inside of it, if you're running it on Linux use ``chmod +x ./PlayerList.API`` to give execution permissions, then run it with ./PlayerList.API, the default ports that the API will be using are: 5000 (non-HTTPS) and 5001 (HTTPS), you can change those using ``./PlayerList.API --urls="http:*:PORT_HERE"``. The API will use LiteDB to store the Player List by default, if you want to change this to use MySQL/MariaDB, please read [this](https://github.com/BounceGaming/PlayerListAPI/blob/main/README.md#Configuration).

# Installation (Cloud, not self-hosted)
_Coming soon_

# Configuration

### Plugin
  - Go to ``.config/EXILED/Configs/(YOUR_PORT)-config.yml`` and set the BaseUrl for the API (default is used) and the ApiKey, this last one is also needed to be changed in the API configuration, make sure they are both the same.

### API
  - Go to the folder containing the API executable and open ``appsettings.json``, there you can change the prefered Database to be used (default is LiteDB but I recommend using MySQL).
  
# For Developers
You can use the Wrapper to develop your own software with this API, the only thing you need to do is import PlayerList.Wrapper.dll and the Newtonsoft.Json.dll [from here](https://github.com/BounceGaming/PlayerListAPI/releases/latest).

Example use:
```cs
// Inicialize the Wrapper.
var api = new PlayerListWrapper(BASE_URL, API_KEY);

// Get the list of online players in the server with port 7777, do not return UserIds (they'll be null)
var players = api.GetPlayers(false, 7777);

// Get the list of online players in the server with port 7778, return UserIds.
var players = api.GetPlayers(true, 7778);
```
