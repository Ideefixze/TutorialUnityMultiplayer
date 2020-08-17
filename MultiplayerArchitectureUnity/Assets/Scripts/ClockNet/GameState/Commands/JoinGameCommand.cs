using ClockNet.GameState.GameStateData.PlayerList;
using UnityEngine;

namespace ClockNet.GameState.Commands
{
    /// <summary>
    /// Adds a information about a new player in the local gameManager playerList.
    /// If this is a server, send an information to an connected player that they got accepted.
    /// </summary>
    [System.Serializable]
    public class JoinGameCommand : SerializableClass, IGameCommand
    {
        [SerializeField]
        private bool local;
        [SerializeField]
        private PlayerType type;
        [SerializeField]
        private string nickname;

        /// <param name="netsocketid">(Socket)ClientHandle ID given from Connection Handler.</param>
        /// <param name="type">Type of Player: AI? Human?</param>
        public JoinGameCommand(bool local, PlayerType type, string nickname) : base()
        {
            this.local = local;
            this.type = type;
            this.nickname = nickname;
        }
        public bool Execute(GameState gameState)
        {
            gameState.playerList.JoinPlayer(local, type, nickname);
            return true;
        }

    }
}
