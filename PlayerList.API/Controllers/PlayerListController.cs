using Microsoft.AspNetCore.Mvc;
using PlayerList.API.Interfaces;
using PlayerList.API.Services;

namespace PlayerList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerListController : ControllerBase
{
    private readonly GenericDatabaseManager _databaseManager;
    private readonly IConfiguration _configuration;

    public PlayerListController(GenericDatabaseManager databaseManager, IConfiguration configuration)
    {
        _databaseManager = databaseManager;
        _configuration = configuration;
    }

    [HttpPut]
    public ActionResult PlayerConnected([FromBody] Player? player, [FromHeader] string? authorization)
    {
        if(_configuration["ApiKey"] == "CHANGE ME")
            return Unauthorized("Please change the API key in the appsettings.json file");
        if(authorization == null || authorization != _configuration["ApiKey"])
            return Unauthorized();
        if(player == null)
            return BadRequest();
        var result = _databaseManager.AddPlayer(player);
        if(result)
            return Ok();
        return Conflict();
    }
    
    [HttpDelete]
    public ActionResult PlayerDisconnected([FromBody] Player? player, [FromHeader] string? authorization)
    {
        if(_configuration["ApiKey"] == "CHANGE ME")
            return Unauthorized("Please change the API key in the appsettings.json file");
        if(authorization == null || authorization != _configuration["ApiKey"])
            return Unauthorized();
        if(player == null)
            return BadRequest();
        var result = _databaseManager.DeletePlayer(player);
        if(result)
            return Ok();
        return NotFound();
    }
    
    [HttpGet]
    public ActionResult GetPlayers([FromQuery] bool? getUserIds, [FromQuery] int? port, [FromHeader] string? authorization)
    {
        if(_configuration["ApiKey"] == "CHANGE ME")
            return Unauthorized("Please change the API key in the appsettings.json file");
        if(authorization == null || authorization != _configuration["ApiKey"])
            return Unauthorized();
        return Ok(_databaseManager.GetPlayers(getUserIds.HasValue && getUserIds.Value));
    }
}