using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AddPointsCommand :  SerializableClass, IGameCommand
{
    [SerializeField]
    private int val = 0;
    public AddPointsCommand(int val) : base()
    {
        this.val = val;
    }
    public bool Execute(GameState gameState)
    {
        gameState.points += val;
        gameState.testdata = new string[2];
        for (int i = 0; i < 2; i++)
            gameState.testdata[i] = "xTESTxABCDx1234x";
        return true;
    }

    public override string ToString()
    {
        return val.ToString();
    }

}
