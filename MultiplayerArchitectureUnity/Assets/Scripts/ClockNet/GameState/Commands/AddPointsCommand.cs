using UnityEngine;

namespace ClockNet.GameState.Commands
{

    [System.Serializable]
    public class AddPointsCommand :  SerializableClass, IGameCommand
    {
        [SerializeField]
        private int val = 0;
        public AddPointsCommand(int val) : base()
        {
            this.val = val;
        }
        public bool Execute(GameState gameState)
        {
            gameState.points += val;
            return true;
        }

        public override string ToString()
        {
            return val.ToString();
        }

    }
}
