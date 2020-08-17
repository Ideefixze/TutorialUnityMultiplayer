using ClockNet.GameState.Commands;
using ClockNet.Networking;
using ClockNet.Networking.Connection;
using ClockNet.Networking.Data;

namespace ClockNet.GameState.CommandExecutor
{
    /// <summary>
    /// Server-side executor. Broadcasts Commands to all clients after an errorless execution.
    /// </summary>
    public class MultiplayerServerCommandsExecutor : IGameExecutor
    {
        private GameState gameState;
        private System.Action<GameState> stateUpdated;
        private Server server;

        public event System.Action<GameState> onStateUpdated
        {
            add
            {
                stateUpdated += value;
                if (value != null)
                {
                    value(gameState);
                }
            }
            remove
            {
                stateUpdated -= value;
            }
        }


        public MultiplayerServerCommandsExecutor(GameState gameState, Server server)
        {
            this.gameState = gameState;
            this.server = server;

            MultiplayerDataHandler dataHandler = new MultiplayerDataHandler(Execute);
            this.server.InitializeDataEndPoint(dataHandler, dataHandler);

            MultiplayerConnectionHandler connectionHandler = new MultiplayerConnectionHandler(Execute, this.gameState);
            this.server.InitializeConnectionEndPoint(connectionHandler);
        
        }

        /// <summary>
        /// Executes a Command and broadcasts it to all clients.
        /// </summary>
        /// <param name="cmd">Command to be executed and broadcasted</param>
        public void Execute(IGameCommand cmd)
        {
            if (cmd == null) return;

            cmd.Execute(gameState);
            this.server.SendToAll(cmd);

            stateUpdated?.Invoke(gameState);
        }

    }
}
