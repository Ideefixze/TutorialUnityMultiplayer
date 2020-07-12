using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;


public class Client : DataEndPoint
{
    string host;
    int port;
    TcpClient clientSocket;
    NetworkStream stream;
    int bufferSize = 16384;
    byte[] buffer;

    public Client(string host, int port)
    {
        this.host = host;
        this.port = port;
        clientSocket = new TcpClient();
        buffer = new byte[bufferSize];
    }

    public void Connect()
    {
        try
        {
            clientSocket.Connect(host, port);
            clientSocket.ReceiveBufferSize = bufferSize;
            clientSocket.SendBufferSize = bufferSize;
            stream = clientSocket.GetStream();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        stream.BeginRead(buffer, 0, bufferSize, Receive, null);

    }

    public void Send(string msg)
    {
        msg = msg + "$";
        try
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
            stream.BeginWrite(data, 0, data.Length,null,null);
            
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Send(IGameCommand cmd)
    {
        Send(GameCommandTranslator.CommandToString(cmd));
    }

    private void Receive(IAsyncResult AR)
    {
        try
        {
            int byteLength = stream.EndRead(AR);
            if (byteLength <= 0)
            {
                // TODO: disconnect
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(buffer, data, byteLength);

            dataDebugger.DebugData(data, "from the server");
            dataHandler.HandleData(data);


            stream.BeginRead(buffer, 0, bufferSize, Receive, null);
        }
        catch (Exception _ex)
        {
            Debug.LogError($"Error receiving TCP data: {_ex}");
        }
    }

}
