using System;
using System.Net.Sockets;
using ClockNet.GameState.Commands;
using ClockNet.Networking.Data;
using UnityEngine;

namespace ClockNet.Networking
{
    /// <summary>
    /// Client class handles all socket operations done by a multiplayer client.
    /// </summary>
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

        /// <summary>
        /// Attempts making connection. Upon success begins to read.
        /// </summary>
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

        /// <summary>
        /// Sends bytes of data to the currently connected server.
        /// </summary>
        /// <param name="msg">String of an message (will be turned into bytes[])</param>
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
        /// <summary>
        /// Sends bytes of data to the currently connected server.
        /// </summary>
        /// <param name="cmd">IGameCommand to be serialized into string</param>
        public void Send(IGameCommand cmd)
        {
            Send(GameCommandTranslator.CommandToString(cmd));
        }

        /// <summary>
        /// Receives data from the server and handles it using dataHandler and dataDebugger.
        /// </summary>
        /// <param name="AR">Async result.</param>
        private void Receive(IAsyncResult AR)
        {
            try
            {
                int byteLength = stream.EndRead(AR);
                if (byteLength <= 0)
                {
                    Debug.Log("Disconnected from the server!");
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
}
