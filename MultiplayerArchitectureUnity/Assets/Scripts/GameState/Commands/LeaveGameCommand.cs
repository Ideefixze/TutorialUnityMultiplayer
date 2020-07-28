using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///
/// </summary>
[System.Serializable]
public class LeaveGameCommand : SerializableClass, IGameCommand
{
    [SerializeField]
    private int netsocketid = 0;



    public LeaveGameCommand(int netsocketid) : base()
    {
        this.netsocketid = netsocketid;
    }
    public bool Execute(GameState gameState)
    {
        gameState.playerList.RemovePlayerByNetID(netsocketid);
        return true;
    }

    public override string ToString()
    {
        return netsocketid.ToString();
    }

}
