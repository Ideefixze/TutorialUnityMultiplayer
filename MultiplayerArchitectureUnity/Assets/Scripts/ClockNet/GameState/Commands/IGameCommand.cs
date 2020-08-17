using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockNet.GameState.Commands
{
/// <summary>
    /// Interface for all Commands that execute with given gameState and have to be sent via network.
    /// 
    /// <para>To prevent desync problems:</para>
    /// <para>All client-side commands that <b>modify GameState</b> should be sent via network.</para>
    /// <para>All server-side commands that <b>modify GameState</b> should be sent via network.</para>
    /// <para>Don't use IGameCommand if Command executes locally and changes should not be sent via network. Example: local settings changes. </para>
    /// </summary>
    public interface IGameCommand : ICommand
    {
        /// <summary>
        /// Executes a piece of game code in given gameState.
        /// </summary>
        /// <param name="gameState"> GameState that cointains local client-side data and network-side GameState.</param>
        /// <returns></returns>
        bool Execute(GameState gameState);
    }
}