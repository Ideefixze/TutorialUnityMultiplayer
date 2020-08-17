using System;
using System.Collections;
using System.Collections.Generic;
using ClockNet.GameClient;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateManagement;
using UnityEngine;

namespace ClockNet.Networking.Data
{
public class MultiplayerGameStateHandler : IDataHandler, IDataDebugger
    {
        private string serializedGameState="";
        public GameState.GameState gameState;

        public MultiplayerGameStateHandler()
        {
            gameState = null;
        }
        public void HandleData(byte[] data)
        {
            string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
            serializedGameState += datastr;
            if(serializedGameState.EndsWith(MultiplayerDataSettings.endChar.ToString()))
            {
                gameState = GameStateLoader.Deserialize(serializedGameState.Replace("$",""));
            }
        }

        public void DebugData(byte[] data, string metainfo)
        {
            string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
            char[] sep = { MultiplayerDataSettings.endChar };
            string[] commands = datastr.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in commands)
            {
                Debug.Log($"{s}  — {metainfo}");
            }
        }
    }
}