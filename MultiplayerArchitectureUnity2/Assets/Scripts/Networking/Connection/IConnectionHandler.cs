using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Handles a new connection.
 * For example: sends a bunch of commands to a new connected player that will load his GameState to the one used by server.
 */

public interface IConnectionHandler
{
    void HandleConnection(int id, ClientHandle clientHandle);
    void HandleDisconnection(int id, ClientHandle clientHandle);
}
public class MultiplayerConnectionHandler: IConnectionHandler
{
    private GameState gameState;
    protected Action<IGameCommand> onConnection;

    public MultiplayerConnectionHandler(Action<IGameCommand> onConnection, GameState gameState)
    {
        this.onConnection += onConnection;
        this.gameState = gameState;
    }
    public void HandleConnection(int id, ClientHandle clientHandle)
    {
        string serializedGameState = GameStateLoader.Serialize(gameState);
        /*int lenserial = serializedGameState.Length;
        int lenstream = ClientHandle.bufferSize/2;

        for(int i = 0; i <lenserial/lenstream;i++)
        {
            clientHandle.Send(serializedGameState.Substring(i * lenstream, lenstream));
        }

        clientHandle.Send(new SendGameStateCommand(serializedGameState.Substring(lenserial / lenstream * lenstream, lenserial - lenserial / lenstream * lenstream)));
        clientHandle.Send(new LoadGameStateCommand());*/
        clientHandle.Send(serializedGameState);

        //ThreadManager.ExecuteOnMainThread(() => { onConnection.Invoke(new JoinGameCommand(id, PlayerType.HUMAN)); });
    }

    public void HandleDisconnection(int id, ClientHandle clientHandle)
    {
        ThreadManager.ExecuteOnMainThread(() => { onConnection.Invoke(new LeaveGameCommand(id)); });
    }
}



