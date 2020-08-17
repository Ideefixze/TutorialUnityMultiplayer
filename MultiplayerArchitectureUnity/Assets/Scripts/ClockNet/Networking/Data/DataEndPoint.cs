/*
 * All DataEndPoints have to be initialized before use!
 */
namespace ClockNet.Networking.Data
{
    public class DataEndPoint 
    {
        public IDataHandler dataHandler { get; private set; }
        public IDataDebugger dataDebugger { get; private set; }

        public void InitializeDataEndPoint(IDataHandler handler, IDataDebugger debugger)
        {
            this.dataHandler = handler;
            this.dataDebugger = debugger;
        }
    }
}
