// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using KadmiumRtpMidi;
namespace rtpmidi_session_repeater;
class Program
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task Main()
    {
        var configText = await File.ReadAllTextAsync("config.json");
        if (configText == null)
        {
            await Console.Error.WriteLineAsync("Error loading config.json");
            return;
        }
        var config = JsonSerializer.Deserialize<Config>(configText, JsonOptions);
        if (config == null)
        {
            await Console.Error.WriteLineAsync("Error parsing config.json");
            return;
        }

        var localHostname = Dns.GetHostName();
        await Console.Out.WriteLineAsync("Using hostname " + localHostname);
        var hostEntry = await Dns.GetHostEntryAsync(localHostname);
        var localIp = hostEntry.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        await Console.Out.WriteLineAsync("Using IP " + localIp);

        var session = new Session(localIp, config.Port, config.SessionName);
        var peerTasks = config.Peers.Select(peer => session.Invite(new IPEndPoint(IPAddress.Parse(peer.Host), peer.Port)));
        await Task.WhenAll(peerTasks);

        CancellationTokenSource Cts = new();
        await Task.Delay(Timeout.Infinite, Cts.Token);
    }
}