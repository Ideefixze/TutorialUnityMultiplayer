using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDataHandler
{
    /*Handle data and do something with it */
    void HandleData(byte[] data);

}

public interface IDataDebugger
{
    /*Debug obtained data */
    void DebugData(byte[] data, string metainfo);
}

public class MultiplayerDataHandler: IDataHandler, IDataDebugger
{
    protected Action<IGameCommand> onGameData;

    public MultiplayerDataHandler(Action<IGameCommand> onGameData)
    {
        this.onGameData += onGameData;
    }
    public void HandleData(byte[] data) 
    {
        string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
        char[] sep = { '$' };
        string[] commands = datastr.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in commands)
        {
            ThreadManager.ExecuteOnMainThread(() => { onGameData.Invoke(GameCommandTranslator.StringToCommand(s)); });
        }
    }

    public void DebugData(byte[] data, string metainfo)
    {
        string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
        char[] sep = { '$' };
        string[] commands = datastr.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in commands)
        {
            Debug.Log($"{s}  — {metainfo}");
        }
    }
}

public class MultiplayerGameStateHandler : IDataHandler, IDataDebugger
{
    private string serializedGameState="";
    public GameState gameState;

    public MultiplayerGameStateHandler()
    {
        gameState = null;
    }
    public void HandleData(byte[] data)
    {
        string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
        serializedGameState += datastr;
        if(serializedGameState.EndsWith("$"))
        {

            gameState = GameStateLoader.Deserialize(serializedGameState.Replace("$",""));
        }
    }

    public void DebugData(byte[] data, string metainfo)
    {
        string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
        char[] sep = { '$' };
        string[] commands = datastr.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in commands)
        {
            Debug.Log($"{s}  — {metainfo}");
        }
    }
}


