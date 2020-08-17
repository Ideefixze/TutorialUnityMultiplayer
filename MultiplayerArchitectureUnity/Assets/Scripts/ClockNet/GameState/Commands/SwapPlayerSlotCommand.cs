using UnityEngine;

namespace ClockNet.GameState.Commands
{
    [System.Serializable]
    public class SwapPlayerSlotCommand : SerializableClass, IGameCommand
    {
        [SerializeField]
        private int from = 0;
        [SerializeField]
        private int to = 0;
        public SwapPlayerSlotCommand(int from, int to) : base()
        {
            this.from = from;
            this.to = to;
        }
        public bool Execute(GameState gameState)
        {
            from = from% (gameState.playerList.players.Length+1);
            to = to% (gameState.playerList.players.Length+1);
            if (to == 0)
                to = 1;

            gameState.playerList.SwapPlaces(from, to);
            return true;
        }

        public override string ToString()
        {
            return "";
        }

    }
}
