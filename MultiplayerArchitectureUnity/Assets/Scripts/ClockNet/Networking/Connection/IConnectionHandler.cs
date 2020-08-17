using System;
using System.Collections;
using System.Collections.Generic;
using ClockNet.GameClient;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateManagement;
using UnityEngine;

namespace ClockNet.Networking.Connection
{
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
        private GameState.GameState gameState;
        protected Action<IGameCommand> onConnection;

        public MultiplayerConnectionHandler(Action<IGameCommand> onConnection, GameState.GameState gameState)
        {
            this.onConnection += onConnection;
            this.gameState = gameState;
        }
        public void HandleConnection(int id, ClientHandle clientHandle)
        {
            string serializedGameState = GameStateLoader.Serialize(gameState);
            clientHandle.Send(serializedGameState);

        }

        public void HandleDisconnection(int id, ClientHandle clientHandle)
        {
            ThreadManager.ExecuteOnMainThread(() => { onConnection.Invoke(new LeaveGameCommand(id)); });
        }
    }
}