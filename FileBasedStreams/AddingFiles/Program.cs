using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.StreamItems;
using System.Collections.Generic;
using System.Threading;

namespace Adding_Files_Example
{
    class Program
    {
        /**********************************************************************************
        Adding Files

        This example shows how to add files to the file data store and get basic information 
        on the available streams within those files.
        **********************************************************************************/
        static void Main(string[] args)
        {
            //create new realtime data store manager
            FileDataStoreManager file_ds_manager = new FileDataStoreManager();

            file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example1.ncom");
            file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example2.ncom");

            //increment of 100000000ns, gives us 1Hz
            file_ds_manager.DecodeData(file_ds_manager.GetStoreStartTime(), file_ds_manager.GetStoreEndTime(), 100000000);


            //Get all available streams in the data store
            List<IStreamItem> streams = file_ds_manager.GetAllStreamItems();

            //For each stream in datastore
            foreach (IStreamItem stream in streams)
            {
                System.Console.Clear();
                //Output stream information
                System.Console.WriteLine("File name: " + stream.Address);
                System.Console.WriteLine("Resource ID: " + stream.ResourceId);
                System.Console.WriteLine("Stream ID: " + stream.StreamId);
                System.Console.WriteLine("Stream Name: " + stream.StreamName);
                System.Console.WriteLine("Transport Name: " + stream.TransportName);
                System.Console.WriteLine("Stream Codec: " + stream.CodecName);
                System.Console.WriteLine("User Tag: " + stream.UserTag);
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
