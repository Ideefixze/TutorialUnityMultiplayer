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

        private Coroutine gameSimulationCoroutine;

        /// <summary>
        /// Creates one-player server for a singleplayer.
        /// </summary>
        public void InitSingleplayer()
        {
            gameState = new GameState();
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
            gameExecutor.Execute(new JoinGameCommand(false, PlayerType.HUMAN, "GuestPlayer"));
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
        }

        /// <summary>
        /// Starts up GameState simulation that executes some commands in DefauldGameSimulation every one second.
        /// TODO: It is just an example. Remember to make it more general.
        /// </summary>
        public void StartSimulation()
        {
            if(gameSimulationCoroutine!=null) return;
            
            DefaultGameSimulation simulation = new DefaultGameSimulation(gameExecutor, gameState);
            DefaultGameTimer timer = new DefaultGameTimer(0.05f);
            gameSimulationCoroutine = StartCoroutine(timer.WorldSimulation(simulation));
            Debug.Log("Game Simulation: Start");
        }

        public void StopSimulation()
        {
            if (gameSimulationCoroutine != null)
            {
                StopCoroutine(gameSimulationCoroutine);
                gameSimulationCoroutine = null;
            }
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
