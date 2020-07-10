using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandHolder<TCommand>
{
    TCommand GetNextCommand();
    void AddCommand(TCommand cmd);
}

public class QueueCommandHolder: ICommandHolder<IGameCommand>
{
    Queue<IGameCommand> queue;

    public IGameCommand GetNextCommand()
    {
        return queue.Dequeue();
    }

    public void AddCommand(IGameCommand cmd)
    {
        queue.Enqueue(cmd);
    }
}
