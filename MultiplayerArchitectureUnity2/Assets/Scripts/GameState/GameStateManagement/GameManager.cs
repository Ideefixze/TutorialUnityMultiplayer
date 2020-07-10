using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Manages all game-related data such as current GameState.
/// <para>Modifiable by Commands data should be here. </para>
/// </summary>
public class GameManager
{
    public GameState gameState { get; set; }
    public SimpleStringBuffer networkBuffer { get; set; }
    public FileReaderBuffer fileBuffer { get; set; }
    public Client client { get; set; }
    public int thisClientID { get; set; } = -1;

    private Server _server;
    public Server server 
    { 
        get
        {
            return _server;
        }
        set 
        { 
            _server = value; 
            gameState.playerList = new PlayerList(_server.maxPlayers); 
        } 
    }

    public GameManager()
    {
        gameState = new GameState();
        networkBuffer = new SimpleStringBuffer();
        fileBuffer = new FileReaderBuffer();
        gameState.playerList = new PlayerList(32);
    }

    //Put all Load/Save methods to some static class

    public void Load()
    {
        Load(networkBuffer.GetBuffer());
    }

    public void Load(string serializedGameState)
    {
        Debug.Log("Attempting to load: " + serializedGameState);
        gameState = JsonUtility.FromJson<GameState>(serializedGameState);
        Debug.Log("GameState loaded!");
    }

    public void Save()
    {
        Debug.Log("Saving state to a buffer");
        networkBuffer.SetBuffer(JsonUtility.ToJson(gameState));
        Debug.Log("GameState saved!");
    }

    public void LoadFromFile(string filename)
    {
        fileBuffer.SetBuffer("");
        fileBuffer.AddToBuffer(filename);
        if(fileBuffer.GetBuffer()!="")
            Load(fileBuffer.GetBuffer());
        gameState.playerList = new PlayerList(gameState.playerList.players.Length);
    }

    public void SaveToFile(string filename)
    {
        fileBuffer.SetBuffer(JsonUtility.ToJson(gameState));
        fileBuffer.SaveBuffer(filename);
    }
}
