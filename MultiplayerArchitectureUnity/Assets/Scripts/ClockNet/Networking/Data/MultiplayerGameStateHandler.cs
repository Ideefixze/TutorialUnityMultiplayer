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
        //If during loading a serialized GameState we get any command, we should execute them on the start.
        //We could also stop the server until player is loaded.
        private string overheardData = ""; 
        public GameState.GameState gameState;

        public MultiplayerGameStateHandler()
        {
            gameState = null;
        }
        public void HandleData(byte[] data)
        {
            string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
            serializedGameState += datastr;
            if (gameState == null)
            {
                if (serializedGameState.EndsWith(MultiplayerDataSettings.endChar.ToString()))
                {

                    gameState = GameStateLoader.Deserialize(serializedGameState.Replace("$", ""));

                }
            }
            else
            {
                overheardData += datastr;
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

        public string GetOverheardData()
        {
            return overheardData;
        }
    }
}