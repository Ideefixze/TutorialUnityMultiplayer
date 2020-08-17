using System.Collections;
using System.Collections.Generic;
using ClockNet.GameState.Commands;
using UnityEngine;

namespace ClockNet.GameState.CommandExecutor
{
    /// <summary>
    /// Executes GameCommands on a GameState.
    /// </summary>
    public interface IGameExecutor : IExecutor<GameState, IGameCommand>
    {
        event System.Action<GameState> onStateUpdated; //GameExecutor should acknowledge all views with current changes.
    }
}