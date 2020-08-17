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
        /*Handle data and do something with it */
        void HandleData(byte[] data);

    }
}