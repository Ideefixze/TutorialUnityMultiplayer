using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mediator between the Unity and both: gameManager and gameExecutor. Initializes them and contains some other functionalities.
/// </summary>
public class GameOperator : MonoBehaviour
{
    public IGameExecutor gameExecutor { get; private set; }
    public GameManager gameManager { get; private set; }

    public GameState gameState;

    [SerializeField]
    private NetworkOperator networkOperator;

    /// <summary>
    /// Creates one-player server for a singleplayer.
    /// </summary>
    public void InitSingleplayer()
    {
        InitCommon();
        networkOperator.maxPlayers = 1;
        Server s = networkOperator.RunServer();
        gameExecutor = new MultiplayerServerCommandsExecutor(gameState, s);
        gameExecutor.Execute(new JoinGameCommand(true, PlayerType.HUMAN, "LocalPlayer"));
        gameExecutor.Execute(new AcknowledgeLoadCommand(0));
        Debug.Log("Inited singleplayer");
    }

    /// <summary>
    /// Initializes client and automatically joins a multiplayer game.
    /// </summary>

    public void InitMultiplayerClient()
    {
        InitCommon();
        Client c = networkOperator.JoinGame(ref gameState);
        gameExecutor = new MultiplayerClientCommandsExecutor(gameState, c);
        gameExecutor.Execute(new JoinGameCommand(false, PlayerType.HUMAN, "Clockworker"));
        Debug.Log("Inited multiplayer client");
    }

    /// <summary>
    /// Creates server for a multiplayer game.
    /// </summary>
    public void InitMultiplayerServer()
    {
        InitCommon();
        Server s = networkOperator.RunServer();
        gameExecutor = new MultiplayerServerCommandsExecutor(gameState, s);
        gameExecutor.Execute(new JoinGameCommand(true, PlayerType.HUMAN, "ServerMaster"));
        gameExecutor.Execute(new AcknowledgeLoadCommand(0));
        Debug.Log("Inited multiplayer server");
    }

    public void InitCommon()
    {
        gameState = new GameState(8);
    }

    public void LoadGameState()
    {
        Debug.Log("Loading started");
        gameManager.LoadFromFile("gamestate.json");
        gameExecutor.Execute(new DebugLogCommand("Updating views."));
        Debug.Log("Loading finished");
    }

    public void SaveGameState()
    {
        Debug.Log("Saving started");
        gameManager.SaveToFile("gamestate.json");
        Debug.Log("Saving finished");
    }

    public void Update()
    {
        ThreadManager.UpdateMain();
    }



}
