using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


/// <summary>
/// Contains all functions that helps translate between JSON and Commands.
/// </summary>
public static class GameCommandTranslator 
{
    public static string CommandToString(IGameCommand cmd)
    {
        return JsonUtility.ToJson(cmd);
    }

    public static IGameCommand StringToCommand(string msg)
    {
        SerializableClass ctype = JsonUtility.FromJson<SerializableClass>(msg);
        Type t = Type.GetType(ctype.GetClassName());
        IGameCommand gc = (IGameCommand)JsonUtility.FromJson(msg, t);
        return gc;
    }
}
