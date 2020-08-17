using UnityEngine;

namespace ClockNet.GameState.Commands
{
    /// <summary>
    /// Command that executes Debug.Log(). Used in testing.
    /// </summary>
    [System.Serializable]
    public class DebugLogCommand :SerializableClass, IGameCommand
    {
        [SerializeField]
        private string Message;


        /// <param name="msg">Message that should be logged in Debug.Log.</param>
        public DebugLogCommand(string msg) : base()
        {
            Message = msg;
        }
        public bool Execute(GameState gameState)
        {
            Debug.Log(Message);
            return true;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
