using System.Collections;
using System.Collections.Generic;
using ClockNet.GameState.Commands;
using UnityEngine;

namespace ClockNet.GameState.CommandExecutor
{
    /// <summary>
    /// Interface for all Command executors.
    /// </summary>
    /// <typeparam name="TState">State type on which changes will be made</typeparam>
    /// <typeparam name="TCommand">Type of commands</typeparam>
    public interface IExecutor<TState, TCommand>
    {
        void Execute(TCommand command);
    }
}