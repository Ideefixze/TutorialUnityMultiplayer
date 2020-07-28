using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Confirms GameState being loaded by an client with given ID.
/// </summary>
[System.Serializable]
public class AcknowledgeLoadCommand : SerializableClass, IGameCommand
{

    [SerializeField]
    private int id;
    /// <param name="id">ID of client that confirms being loaded.</param>
    public AcknowledgeLoadCommand(int id) : base()
    {
        this.id = id;
    }
    public bool Execute(GameState gameState)
    {
        gameState.playerList.players[id].loadState = PlayerLoadState.READY; //Change it... method?
        return true;
    }

    public override string ToString()
    {
        return "";
    }
}
