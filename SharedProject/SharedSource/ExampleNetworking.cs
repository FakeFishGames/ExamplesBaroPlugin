using Barotrauma;
using Barotrauma.Networking;
using Microsoft.Xna.Framework;
using Barotrauma.Plugins;
using System.Collections.Immutable;

namespace Examples;

enum NetworkHeader
{
    ClientRequest,
    ServerResponse
}

[NetworkSerialize]
internal readonly record struct ClientRequestData(string SomeData, int Number) : INetSerializableStruct;
[NetworkSerialize]
internal readonly record struct ServerResponseData(bool IsCorrect) : INetSerializableStruct;


public static class ExampleNetworking
{
    public static void Register()
    {
        Plugin.GameNetwork.RegisterNetworkHeaders<NetworkHeader>();

#if SERVER
        Plugin.GameNetwork.RegisterHandler<NetworkHeader, ClientRequestData>(NetworkHeader.ClientRequest, ClientRequestReceived);
#elif CLIENT
        Plugin.GameNetwork.RegisterHandler<NetworkHeader, ServerResponseData>(NetworkHeader.ServerResponse, ServerResponseReceived);

        Plugin.DebugConsole.RegisterCommand("sendexamplemessage", "sendexamplemessage [number]", CommandFlags.DoNotRelayToServer, args =>
        {
            if (args.Length == 0)
            {
                Plugin.DebugConsole.NewMessage("Not enough arguments", Color.Red);
                return;
            }

            if (!int.TryParse(args[0], out int result))
            {
                Plugin.DebugConsole.NewMessage("Invalid number", Color.Red);
                return;
            }

            if (GameMain.Client is null)
            {
                Plugin.DebugConsole.NewMessage("Not connected to a server", Color.Red);
                return;
            }

            Plugin.GameNetwork.Send(NetworkHeader.ClientRequest, new ClientRequestData("something", result));
            Plugin.DebugConsole.NewMessage("Sent request to server", Color.Orange);
        });
#endif
    }

#if SERVER
    private static void ClientRequestReceived(ClientRequestData data, Client client)
    {
        if (!client.HasPermission(ClientPermissions.ManageCampaign))
        {
            return;
        }

        if (data.Number == 42)
        {
            Plugin.GameNetwork.SendToClient(client, NetworkHeader.ServerResponse, new ServerResponseData(true));
            Plugin.DebugConsole.NewMessage("Client was correct", Color.Blue);
        }
        else
        {
            Plugin.GameNetwork.SendToClient(client, NetworkHeader.ServerResponse, new ServerResponseData(false));
            Plugin.DebugConsole.NewMessage("Client was incorrect", Color.Blue);
        }
    }
#elif CLIENT
    private static void ServerResponseReceived(ServerResponseData data)
    {
        if (data.IsCorrect)
        {
            Plugin.DebugConsole.NewMessage("We were correct", Color.Green);
        }
        else
        {
            Plugin.DebugConsole.NewMessage("We were incorrect", Color.Red);
        }
    }
#endif
}
