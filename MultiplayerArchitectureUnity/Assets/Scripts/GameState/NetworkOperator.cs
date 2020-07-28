using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
/// <summary>
/// Contains all data needed to connect to a game or host a server.
/// </summary>
public class NetworkOperator : MonoBehaviour
{

    [SerializeField]
    public string ip { get; set; } = "127.0.0.1";
    [SerializeField]
    public int port { get; set; } = 12345;
    public int maxPlayers { get; set; } = 8;

    /// <summary>
    /// Joins a multiplayer server. Parameters about server are in NetworkOperator members.
    /// </summary>
    /// <returns>Client class that attempted connection.</returns>
    public Client JoinGame(ref GameState gs)
    {
        Client client = new Client(ip, port);
        client.Connect();
        MultiplayerGameStateHandler multiplayerGameStateHandler = new MultiplayerGameStateHandler();
        client.InitializeDataEndPoint(multiplayerGameStateHandler, multiplayerGameStateHandler);

        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(500);
            if (multiplayerGameStateHandler.gameState != null)
            {
                gs = multiplayerGameStateHandler.gameState;
                Debug.Log("Received GameState");
                break;
            }
            else
            {
                Debug.Log("Waiting for GameState...");
            }
        }
        return client;
    }


    /// <summary>
    /// Runs a server on this machine. Parameters about server are in NetworkOperator members.
    /// </summary>
    /// <returns>Server class that have been created</returns>

    public Server RunServer()
    {
        Server server = new Server(port, maxPlayers);
        return server;
    }

}
