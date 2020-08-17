using System.Collections;
using UnityEngine;

namespace ClockNet.GameState.GameStateManagement.Simulation
{
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
}
