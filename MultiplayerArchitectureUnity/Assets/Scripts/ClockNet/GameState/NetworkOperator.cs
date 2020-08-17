using System.Threading;
using ClockNet.Networking;
using ClockNet.Networking.Data;
using UnityEngine;

namespace ClockNet.GameState
{
    /// <summary>
    /// Contains all data and methods needed to connect to a game or host a server.
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
        /// <param name="gs">GameState to be loaded via network.</param>
        /// <returns>Client class that attempted connection.</returns>
        public Client JoinGame(ref GameState gs)
        {
            Client client = new Client(ip, port);
            client.Connect();
            MultiplayerGameStateHandler multiplayerGameStateHandler = new MultiplayerGameStateHandler();
            client.InitializeDataEndPoint(multiplayerGameStateHandler, multiplayerGameStateHandler);

            //Wait up to 5 seconds or until you receive a GameState from the server.
            //IMPORTANT: Change it if your Game State is much bigger
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                if (multiplayerGameStateHandler.gameState != null)
                {
                    gs = multiplayerGameStateHandler.gameState;
                    Debug.Log("Received GameState");
                    return client;
                }
                else
                {
                    Debug.Log("Waiting for GameState...");
                }
            }
            Debug.Log("Couldn't receive GameState from the server. Check connection or try again.");
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
}
