using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionEndPoint 
{
    public IConnectionHandler connectionHandler { get; private set; }

    public void InitializeConnectionEndPoint(IConnectionHandler handler)
    {
        connectionHandler = handler;
    }
}
