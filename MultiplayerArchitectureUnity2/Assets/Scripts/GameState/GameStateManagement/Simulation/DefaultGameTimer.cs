using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGameTimer : IGameTimer
{
    private float worldTick;
    public DefaultGameTimer(float worldTick)
    {
        this.worldTick = worldTick;
    }
    public IEnumerator WorldSimulation(IGameSimulation simulationRules)
    {
        while(true)
        {
            simulationRules.WorldTick();
            yield return new WaitForSeconds(worldTick);
        }
    }
}
