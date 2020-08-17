using System;
using System.Collections;
using System.Collections.Generic;
using ClockNet.GameClient;
using ClockNet.GameState.Commands;
using ClockNet.GameState.GameStateManagement;
using UnityEngine;

namespace ClockNet.Networking.Data
{

    public interface IDataDebugger
    {
        /*Debug obtained data */
        void DebugData(byte[] data, string metainfo);
    }
}