using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SendGameStateCommand : SerializableClass, IGameCommand
{

    [SerializeField]
    private string data;
    public SendGameStateCommand(string data) : base()
    {
        this.data = data;
    }
    public bool Execute(GameState gameState)
    {
        GameStateLoader.AppendStringToSerialize(data);
        return true;
    }

    public override string ToString()
    {
        return "";
    }
}
