using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadGameStateCommand : SerializableClass, IGameCommand
{

    public LoadGameStateCommand() : base()
    {
    }
    public bool Execute(GameState gameState)
    {
        GameStateLoader.Deserialize();
        return true;
    }

    public override string ToString()
    {
        return "";
    }
}
