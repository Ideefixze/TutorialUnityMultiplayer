using System;
using System.Collections;
using System.Collections.Generic;
using ClockNet.GameClient;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateManagement;
using UnityEngine;

namespace ClockNet.Networking.Data
{
    public class MultiplayerDataSettings
    {
        public static char endChar = '$';
    }

    public interface IDataHandler
    {
        /// <summary>
        /// Basic method for this interface, that handles any data received by the socket. For example: translates gathered data into command and executes it.
        /// </summary>
        /// <param name="data">Data received by the NetworkStream</param>
        void HandleData(byte[] data);

        /// <summary>
        /// Returns an overheard data during a IDataHandler change, that wasn't used by this IDataHandler but may be still useful.
        /// </summary>
        /// <returns>Data that were to be handled, but got outdated for this handler.</returns>
        string GetOverheardData();

    }
}