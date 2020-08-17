using ClockNet.GameState.GameStateData.PlayerList;

namespace ClockNet.GameState
{
    /// <summary>
    /// Contains all game data that is shared among all players.
    /// </summary>
    [System.Serializable]
    public class GameState
    {
        public int points;
        public PlayerList playerList;

        public GameState(int maxPlayer=1)
        {
            playerList = new PlayerList(maxPlayer);
        }
    }
}
