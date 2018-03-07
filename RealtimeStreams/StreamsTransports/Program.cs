using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.StreamItems;
using System.Collections.Generic;
using System.Threading;

namespace Streams_Transports_Example
{
    class Program
    {
        /**********************************************************************************
        Streams and Transports

        This example shows how to get basic information on the available streams in the
        realtime data store.
        **********************************************************************************/
        static void Main(string[] args)
        {
            //create new realtime data store manager
            RealTimeDataStoreManager realtime_ds_manager = new RealTimeDataStoreManager();

            //Sleep for a few seconds to allow streams to initialize
            Thread.Sleep(3000);

            while (true)
            {
                //Get all available streams in the realtime data store
                List<IStreamItem> streams = realtime_ds_manager.GetAllStreamItems();

                //For each stream in realtime datastore
                foreach (IStreamItem stream in streams)
                {
                    System.Console.Clear();
                    //Output stream information
                    System.Console.WriteLine("Resource Id: " + stream.ResourceId);
                    System.Console.WriteLine("Stream Id: " + stream.StreamId);
                    System.Console.WriteLine("Original Stream Id: " + stream.OriginalStreamId);
                    System.Console.WriteLine("Stream Name: " + stream.StreamName);
                    System.Console.WriteLine("Transport Name: " + stream.TransportName);
                    System.Console.WriteLine("Stream Codec: " + stream.CodecName);
                    System.Console.WriteLine("User Tag: " + stream.UserTag);
                    System.Console.WriteLine("Device Product Name: " + stream.DeviceProductName);
                    System.Console.WriteLine("Device Product Type: " + stream.DeviceProductType);
                    System.Console.WriteLine("Device Serial number: " + stream.DeviceSerialNumber);


                    System.Console.WriteLine();
                    System.Console.WriteLine("End of Stream Data");
                    System.Console.WriteLine("Press Enter key for next stream...");
                    System.Console.ReadLine();
                }

                System.Console.Clear();
                System.Console.WriteLine("End of Streams");
                System.Console.WriteLine("Press Enter to start again...");
                System.Console.ReadLine();

            }
        }
    }
}
