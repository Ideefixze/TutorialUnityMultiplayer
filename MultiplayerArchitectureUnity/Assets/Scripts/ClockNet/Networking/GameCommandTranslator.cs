using System;
using ClockNet.GameState.Commands;
using UnityEngine;

namespace ClockNet.Networking
{
    /// <summary>
    /// Contains all static functions that helps translate between JSON and IGameCommands.
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
}
