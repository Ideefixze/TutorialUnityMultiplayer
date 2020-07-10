using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/// <summary>
/// Helper struct. Common field with SerializableClass.
/// </summary>
struct CommandType
{
    public string serializedClassName;
}

/// <summary>
/// Contains all functions that helps translate between JSON and Commands.
/// </summary>
public static class GameCommandTranslator 
{
    public static string CommandToString(IGameCommand cmd)
    {
        Debug.Log("CommandToString: " + JsonUtility.ToJson(cmd));
        return JsonUtility.ToJson(cmd);
    }

    public static IGameCommand StringToCommand(string msg)
    {
        Debug.Log("StringToCommand: "+msg);
        CommandType ctype = JsonUtility.FromJson<CommandType>(msg);
        Type t = Type.GetType(ctype.serializedClassName);
        IGameCommand gc = (IGameCommand)JsonUtility.FromJson(msg, t);
        return gc;
    }
}
