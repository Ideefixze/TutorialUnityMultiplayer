using ClockNet.GameState.Commands;
using ClockNet.Networking;
using ClockNet.Networking.Data;

namespace ClockNet.GameState.CommandExecutor
{
    /// <summary>
    /// Executor for a client. Instead of executing them locally, send data to the server.
    /// </summary>
    public class MultiplayerClientCommandsExecutor : IGameExecutor
    {
        private GameState gameState;
        private System.Action<GameState> stateUpdated;
        private Client client;

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
        public MultiplayerClientCommandsExecutor(GameState gameState, Client client)
        {
            this.gameState = gameState;
            this.client = client;
            MultiplayerDataHandler handler = new MultiplayerDataHandler(UpdateState);
            this.client.InitializeDataEndPoint(handler, handler);
        }
        public void Execute(IGameCommand cmd)
        {
            this.client.Send(GameCommandTranslator.CommandToString(cmd));
        }

        /// <summary>
        /// Executes Command on local copy of GameState. Use it from DataHandler when JSON command arrives from server.
        /// </summary>
        /// <param name="cmd">Command to be executed</param>
        public void UpdateState(IGameCommand cmd)
        {
            if (cmd == null) return;

            cmd.Execute(gameState);
            stateUpdated?.Invoke(gameState);

        }
    }
}
