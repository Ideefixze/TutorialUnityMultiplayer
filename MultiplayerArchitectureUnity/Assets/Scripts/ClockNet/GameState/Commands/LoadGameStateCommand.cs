using ClockNet.GameState.GameStateManagement;

namespace ClockNet.GameState.Commands
{
    [System.Serializable]
    public class LoadGameStateCommand : SerializableClass, IGameCommand
    {

        public LoadGameStateCommand() : base()
        {
        }
        public bool Execute(GameState gameState)
        {
            GameStateLoader.Deserialize();
            return true;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
