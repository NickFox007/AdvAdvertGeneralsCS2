using AdvAdvert;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Cvars;
using System.Xml.Linq;

public class AdvertGenerals : BasePlugin
{
    public override string ModuleName => "[Adverts] Generals";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "Nick Fox";
    public override string ModuleDescription => "General placeholders for AdvAdverts";

    private IAdvertApi? _api;
    private readonly PluginCapability<IAdvertApi> _plugin = new("adverts:nfcore");

    ConVar CVarName;
    ConVar CVarPort;

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        _api = _plugin.Get();

        if (_api == null)
            Console.WriteLine("Adverts core not found");
        else
        {
            CVarName = ConVar.Find("hostname");
            CVarPort = ConVar.Find("hostport");

            _api.AddHook(HookAction);
        }
    }

    private string GetPlayersCount(int withBots = 0)
    {
        List<CCSPlayerController> players;
        switch (withBots)
        {
            case 0: players = Utilities.GetPlayers(); break;
            case 1: players = Utilities.GetPlayers().FindAll(x => !x.IsBot); break;
            case 2: players = Utilities.GetPlayers().FindAll(x => x.IsBot); break;
            default: players = new List<CCSPlayerController>();break;
        }

        return players.Count.ToString();
    }

    private void HookAction(MessageData data)
    {
        data.Replace("{IpPort}", _api.GetServerIp());        
        data.Replace("{Port}", CVarPort.GetPrimitiveValue<int>().ToString());        
        data.Replace("{Servername}", CVarName.StringValue);
        data.Replace("{Players}", GetPlayersCount(1));
        data.Replace("{PlayersWithBots}", GetPlayersCount());
        data.Replace("{Bots}", GetPlayersCount(2));
        data.Replace("{MaxPlayers}", Server.MaxPlayers.ToString());
        data.Replace("{Map}", Server.MapName);
        data.Replace("{TimeHMS}", DateTime.Now.ToString("HH:mm:ss"));
        data.Replace("{TimeHM}", DateTime.Now.ToString("HH:mm"));
        data.Replace("{Date}", DateTime.Now.ToString("dd.MM.yyyy"));
        data.Replace("{Weekday}", DateTime.Now.ToString("dddd"));
    }
}