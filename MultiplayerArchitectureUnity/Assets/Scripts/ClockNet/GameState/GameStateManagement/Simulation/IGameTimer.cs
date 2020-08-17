using System.Collections;

namespace ClockNet.GameState.GameStateManagement.Simulation
{
    public interface IGameTimer
    {
        IEnumerator WorldSimulation(IGameSimulation simulationRules);
    }
}
