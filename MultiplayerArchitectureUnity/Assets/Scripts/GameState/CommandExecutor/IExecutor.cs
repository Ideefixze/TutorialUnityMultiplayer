using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all Command executors.
/// </summary>
/// <typeparam name="TState">State type on which changes will be made</typeparam>
/// <typeparam name="TCommand">Type of commands</typeparam>
public interface IExecutor<TState, TCommand>
{
    void Execute(TCommand command);
}
/// <summary>
/// Executes GameCommands on a GameState.
/// </summary>
public interface IGameExecutor : IExecutor<GameState, IGameCommand>
{
    event System.Action<GameState> onStateUpdated;
}
