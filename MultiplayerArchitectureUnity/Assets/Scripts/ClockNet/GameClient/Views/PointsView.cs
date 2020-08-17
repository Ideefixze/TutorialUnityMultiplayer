using ClockNet.GameState;
using ClockNet.GameState.Commands;
using UnityEngine;
using UnityEngine.UI;

namespace ClockNet.GameClient.Views
{
    /// <summary>
    /// Views for the points in the GUI.
    /// Handles showing data and creating commands to be executed.
    /// </summary>
    public class PointsView : MonoBehaviour
    {
        public Text currencyText;
        public GameOperator gameOperator;
        public bool updated = false;

        public void Awake()
        {
            Init();
        }
        public void Init()
        {
            gameOperator.gameExecutor.onStateUpdated += UpdateView;
        }
        public void AddCoins()
        {
            var cmd = new AddPointsCommand(Random.Range(1, 5));
            gameOperator.gameExecutor.Execute(cmd);
        }
        public void RemoveCoins()
        {
            var cmd = new AddPointsCommand(-Random.Range(1, 5));
            gameOperator.gameExecutor.Execute(cmd);
        }
        public void UpdateView(GameState.GameState gameState)
        {
            currencyText.text = "Coins: " + gameState.points;
        }

        public void Update()
        {
            updated = false;
        }
        private void OnDestroy()
        {
            gameOperator.gameExecutor.onStateUpdated -= UpdateView;
        }
    
    }
}
