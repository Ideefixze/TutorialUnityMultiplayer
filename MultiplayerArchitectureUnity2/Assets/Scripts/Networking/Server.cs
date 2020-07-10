using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;

public class Server
{
    List<ClientHandle> clientList;
    TcpListener serverSocket;
    public int maxPlayers { get; private set; }

    public Server(int port, int maxPlayers)
    {

        this.maxPlayers = maxPlayers;
        InitializeServer();

        IPAddress addr = IPAddress.Parse("127.0.0.1");
        serverSocket = new TcpListener(addr,port);
        serverSocket.Start();

        serverSocket.BeginAcceptSocket(AcceptClients,null);
    }


    private void AcceptClients(IAsyncResult AR)
    {
        TcpClient clientSocket = serverSocket.EndAcceptTcpClient(AR);
        serverSocket.BeginAcceptTcpClient(AcceptClients, null);
        clientSocket.ReceiveBufferSize = 2048;
        clientSocket.SendBufferSize = 2048;
        Debug.Log($"Connection from {clientSocket.Client.RemoteEndPoint}...");
        
        //Find next empty slot for incoming connection...
        for(int i = 0; i<maxPlayers;i++)
        {
            if(clientList[i].socket==null)
            {
                clientList[i].Connect(clientSocket);
                return;
            }
        }

        Debug.Log($"Failed connection: {clientSocket.Client.RemoteEndPoint} - server is full!");
    }

    public void SendToAll(string msg)
    {
        for (int i = 0; i <maxPlayers; i++)
        {
            if (clientList[i].socket != null)
            {
                clientList[i].Send(msg);
            }
        }
    }

    public void SendToAll(IGameCommand cmd)
    {
        SendToAll(GameCommandTranslator.CommandToString(cmd));
    }

    public void SendToClient(int id, string msg)
    {
        if (clientList[id].socket != null)
        {
            clientList[id].Send(msg);
        }
    }

    public void SendToClient(int id, IGameCommand cmd)
    {
        SendToClient(id, GameCommandTranslator.CommandToString(cmd));
    }

    private void InitializeServer()
    {
        clientList = new List<ClientHandle>();

        for(int i = 0; i<maxPlayers;i++)
        {
            ClientHandle ch = new ClientHandle(i);
            clientList.Add(ch);
        }
            
    }

    public void InitializeDataEndPoint(IDataHandler handler, IDataDebugger debugger)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            clientList[i].InitializeDataEndPoint(handler, debugger);
        }
    }

    public void InitializeConnectionEndPoint(IConnectionHandler handler)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            clientList[i].InitializeConnectionEndPoint(handler);
        }
    }
}

public class ClientHandle
{
    public static int bufferSize = 2048;

    public TcpClient socket;

    private DataEndPoint dataEndPoint;
    private ConnectionEndPoint connectionEndPoint;

    private int id;
    private NetworkStream stream;
    private byte[] buffer;

    public ClientHandle(int id)
    {
        dataEndPoint = new DataEndPoint();
        connectionEndPoint = new ConnectionEndPoint();

        this.id = id;
        socket = null;
    }

    public void Connect(TcpClient socket)
    {
        this.socket = socket;
        this.socket.ReceiveBufferSize = bufferSize;
        this.socket.SendBufferSize = bufferSize;

        buffer = new byte[bufferSize];

        stream = this.socket.GetStream();

        stream.BeginRead(buffer, 0, bufferSize, Receive, null);

        Debug.Log($"{id} connected");

        connectionEndPoint.connectionHandler.HandleConnection(id, this);

    }

    public void Disconnect()
    {
        connectionEndPoint.connectionHandler.HandleDisconnection(id, this);
        stream.Close();
        socket.Close();
        socket = null;

    }

    private void Receive(IAsyncResult AR)
    {
        try
        {
            int byteLength = stream.EndRead(AR);
            if (byteLength <= 0)
            {
                Disconnect();
                Debug.Log($"{id} has disconnected.");
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(buffer, data, byteLength);

            dataEndPoint.dataDebugger.DebugData(data, $"from {id}");
            dataEndPoint.dataHandler.HandleData(data);

            stream.BeginRead(buffer, 0, bufferSize, Receive, null);
        }
        catch (Exception _ex)
        {
            Debug.LogError($"Error receiving TCP data: {_ex}");
        }
    }

    public void Send(String msg)
    {
        msg = msg + "$";
        try
        {
            if (socket != null)
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                stream.BeginWrite(data, 0, data.Length, null, null);
            }
        }
        catch (Exception _ex)
        {
            Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
        }
    }

    public void Send(IGameCommand cmd)
    {
        Send(GameCommandTranslator.CommandToString(cmd));
    }

    public void InitializeDataEndPoint(IDataHandler handler, IDataDebugger debugger)
    {
        dataEndPoint.InitializeDataEndPoint(handler, debugger);
    }

    public void InitializeConnectionEndPoint(IConnectionHandler handler)
    {
        connectionEndPoint.InitializeConnectionEndPoint(handler);
    }
}

