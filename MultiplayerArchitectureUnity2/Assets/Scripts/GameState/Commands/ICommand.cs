using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Interface for all Commands.
/// </summary>
public interface ICommand 
{
}

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


/// <summary>
/// All classes that will be serialized and unserialized should have their type contained in string so it helps unserializing them by their type.
/// </summary>
public class SerializableClass
{
    public string serializedClassName;

    public SerializableClass()
    {
        serializedClassName = this.GetType().ToString();
    }
}
