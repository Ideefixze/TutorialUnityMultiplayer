using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTimer
{
    IEnumerator WorldSimulation(IGameSimulation simulationRules);
}
