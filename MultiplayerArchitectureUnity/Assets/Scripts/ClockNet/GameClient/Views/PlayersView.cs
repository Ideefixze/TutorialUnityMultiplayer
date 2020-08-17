using ClockNet.GameState;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateData.PlayerList;
using UnityEngine;
using UnityEngine.UI;

namespace ClockNet.GameClient.Views
{
    /// <summary>
    /// Shows a text with current players connected to the server.
    /// </summary>
    public class PlayersView : MonoBehaviour
    {
        public Text playerInfo;
        public GameOperator gameOperator;
        public GameState.GameState gameState;
        public bool updated = false;

        public void Awake()
        {
            Init();
        }
        public void Init()
        {
            gameOperator.gameExecutor.onStateUpdated += UpdateView;
        }
        public void UpdateView(GameState.GameState gameState)
        {
        
            playerInfo.text = " ";
            for(int i = 0; i<gameState.playerList.players.Length;i++)
            {
                PlayerData pd = gameState.playerList.players[i];
                playerInfo.text += "ID: " + pd.ID.ToString() +" "+pd.nickname+ " S_ID:" + pd.networkID + " SLOT: " + (pd.slotTaken ? "TAKEN " : "EMPTY ") + "LOAD: "+(pd.loadState.ToString("g")) +" TYPE: " + (pd.type.ToString("g"))+"\n";
            }

            playerInfo.text += "\nMY ID: " + gameState.playerList.GetClientData().ID +"  " +gameState.playerList.GetClientData().nickname;

            this.gameState = gameState;
        }

        public void Update()
        {
            if(Input.GetKeyUp(KeyCode.S))
            {
                gameOperator.gameExecutor.Execute(new SwapPlayerSlotCommand(gameState.playerList.GetClientData().ID, gameState.playerList.GetClientData().ID + 1));
            }
            updated = false;
        }
        private void OnDestroy()
        {
            gameOperator.gameExecutor.onStateUpdated -= UpdateView;
        }
    }
}
