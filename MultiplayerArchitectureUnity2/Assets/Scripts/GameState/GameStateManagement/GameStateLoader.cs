using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateLoader
{
    static string data = "";
    static GameState loadedGameState;
    public static string Serialize(GameState gameState)
    {
        return JsonUtility.ToJson(gameState);
    }

    public static GameState Deserialize(string serializedGameState)
    {
        return loadedGameState = JsonUtility.FromJson<GameState>(serializedGameState);
    }

    public static GameState Deserialize()
    {
        return loadedGameState = JsonUtility.FromJson<GameState>(data);
    }

    public static void AppendStringToSerialize(string data)
    {
        GameStateLoader.data += data;
        Debug.Log(GameStateLoader.data);
    }

    public static void ClearStringToSerialize()
    {
        data = "";
    }
}
