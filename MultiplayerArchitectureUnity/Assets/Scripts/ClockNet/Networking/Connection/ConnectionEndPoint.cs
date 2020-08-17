namespace ClockNet.Networking.Connection
{
    public class ConnectionEndPoint 
    {
        public IConnectionHandler connectionHandler { get; private set; }

        public void InitializeConnectionEndPoint(IConnectionHandler handler)
        {
            connectionHandler = handler;
        }
    }
}
