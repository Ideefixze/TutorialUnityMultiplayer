using ClockNet.GameClient;
using ClockNet.GameState.CommandExecutor;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateData.PlayerList;
using ClockNet.GameState.GameStateManagement;
using ClockNet.GameState.GameStateManagement.Simulation;
using ClockNet.Networking;
using UnityEngine;

namespace ClockNet.GameState
{
    /// <summary>
    /// Mediator between the Unity and gameExecutor. Initializes them and contains some other functionalities.
    /// </summary>
    public class GameOperator : MonoBehaviour
    {
        public IGameExecutor gameExecutor { get; private set; }

        public GameState gameState;

        [SerializeField]
        private NetworkOperator networkOperator;

        /// <summary>
        /// Creates one-player server for a singleplayer.
        /// </summary>
        public void InitSingleplayer()
        {
            InitCommon();
            networkOperator.maxPlayers = 0;
            Server s = networkOperator.RunServer();
            gameExecutor = new MultiplayerServerCommandsExecutor(gameState, s);
            gameExecutor.Execute(new JoinGameCommand(true, PlayerType.HUMAN, "LocalPlayer"));
            Debug.Log("Inited singleplayer");
        }

        /// <summary>
        /// Initializes client and automatically joins a multiplayer game.
        /// </summary>

        public void InitMultiplayerClient()
        {
            InitCommon();
            Client c = networkOperator.JoinGame(ref gameState);
            gameExecutor = new MultiplayerClientCommandsExecutor(gameState, c);
            gameExecutor.Execute(new JoinGameCommand(false, PlayerType.HUMAN, "Clockworker"));
            Debug.Log("Inited multiplayer client");
        }

        /// <summary>
        /// Creates server for a multiplayer game.
        /// </summary>
        public void InitMultiplayerServer()
        {
            InitCommon();
            Server s = networkOperator.RunServer();
            gameExecutor = new MultiplayerServerCommandsExecutor(gameState, s);
            gameExecutor.Execute(new JoinGameCommand(true, PlayerType.HUMAN, "ServerMaster"));
            Debug.Log("Inited multiplayer server");
            //StartSimulation();
        }

        /// <summary>
        /// Starts up GameState simulation that executes some commands in DefauldGameSimulation every one second.
        /// TODO: It is just an example. Remember to make it more general.
        /// </summary>
        public void StartSimulation()
        {
            DefaultGameSimulation simulation = new DefaultGameSimulation(gameExecutor, gameState);
            DefaultGameTimer timer = new DefaultGameTimer(1f);
            StartCoroutine(timer.WorldSimulation(simulation));
            Debug.Log("Started game simulation");
        }

        /// <summary>
        /// Simple GameState initialization with 8 players.
        /// </summary>
        public void InitCommon()
        {
            gameState = new GameState(8);
        }

        /// <summary>
        /// Executes all actions scheduled by ThreadManager (that were added from different threads)
        /// </summary>
        public void Update()
        {
            ThreadManager.UpdateMain();
        }

    }
}
