using PlayerList.API.Services;
using PlayerList.API.Services.DatabaseControllers;
using Serilog;

Console.Title = "PlayerList.API v1.0.0";
const string splash = "  ____                              _____                 _             \n" +
                      " |  _ \\                            / ____|               (_)            \n" +
                      " | |_) | ___  _   _ _ __   ___ ___| |  __  __ _ _ __ ___  _ _ __   __ _ \n" +
                      " |  _ < / _ \\| | | | '_ \\ / __/ _ \\ | |_ |/ _` | '_ ` _ \\| | '_ \\ / _` |\n" +
                      " | |_) | (_) | |_| | | | | (_|  __/ |__| | (_| | | | | | | | | | | (_| | \n" +
                      " |____/ \\___/ \\__,_|_| |_|\\___\\___|\\_____|\\__,_|_| |_| |_|_|_| |_|\\__, | \n" +
                      "                                                                   __/ |\n" +
                      " Developed by [Bujapetas, xRoier]                                 |___/ \n";
Console.ForegroundColor = ConsoleColor.DarkBlue;
Console.WriteLine(splash);
Console.ResetColor();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
var serilog = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Logging.AddSerilog(serilog);
builder.Services.AddSingleton(serilog);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<LiteDbController>();
builder.Services.AddSingleton<MySqlController>();
builder.Services.AddSingleton<GenericDatabaseManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Services.GetService<GenericDatabaseManager>()!.UseLiteDb)
    app.Services.GetService<MySqlController>()!.CreateDatabase();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();