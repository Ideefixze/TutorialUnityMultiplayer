using System;
using System.Collections;
using System.Collections.Generic;
using ClockNet.GameClient;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateManagement;
using UnityEngine;

namespace ClockNet.Networking.Data
{
public class MultiplayerDataHandler: IDataHandler, IDataDebugger
    {
        private string currentBuffer = "";
        protected Action<IGameCommand> onGameData;

        public MultiplayerDataHandler(Action<IGameCommand> onGameData, string startBuffer="")
        {
            currentBuffer = startBuffer;
            this.onGameData += onGameData;
        }
        public void HandleData(byte[] data) 
        {
            string datastr = System.Text.Encoding.ASCII.GetString(data).Trim('\0');
            currentBuffer += datastr;
            char[] sep = { MultiplayerDataSettings.endChar };

            //If current buffer has no ending char, don't invoke incomplete commands.
            if (!currentBuffer.Contains(MultiplayerDataSettings.endChar.ToString()))
                return;

            string[] commands = currentBuffer.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in commands)
            {
                ThreadManager.ExecuteOnMainThread(() => { onGameData.Invoke(GameCommandTranslator.StringToCommand(s)); });
            }

            currentBuffer = "";
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
            return currentBuffer;
        }
    }
}