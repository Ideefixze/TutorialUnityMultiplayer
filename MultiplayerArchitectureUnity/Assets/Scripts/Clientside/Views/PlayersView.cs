using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersView : MonoBehaviour
{
    public Text playerInfo;
    public GameOperator gameOperator;
    public GameState gameState;
    public bool updated = false;

    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        gameOperator.gameExecutor.onStateUpdated += UpdateView;
    }
    public void UpdateView(GameState gameState)
    {
        
        playerInfo.text = " ";
        for(int i = 0; i<gameState.playerList.players.Length;i++)
        {
            PlayerData pd = gameState.playerList.players[i];
            playerInfo.text += "ID: " + pd.ID.ToString() +" "+pd.nickname+ " S_ID:" + pd.networkID + " SLOT: " + (pd.slotTaken ? "TAKEN " : "EMPTY ") + "LOAD: "+(pd.loadState.ToString("g")) +" TYPE: " + (pd.type.ToString("g"))+"\n";
        }

        playerInfo.text += "MY ID: " + gameState.playerList.GetClientData().ID +"  " +gameState.playerList.GetClientData().nickname;

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
