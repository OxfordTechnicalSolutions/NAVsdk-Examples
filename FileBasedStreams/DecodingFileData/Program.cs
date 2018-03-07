
using OxTS.NavLib.Common.Measurement;
using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.StreamItems;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Decoding_File_Data_Retain_Example
{
    class Program
    {
        /**********************************************************************************
        Decoding File Data

        This example shows how to decode stream data contained within a file using the "retain" method.
        **********************************************************************************/
        static void Main(string[] args)
        {
            //create new realtime data store manager
            FileDataStoreManager file_ds_manager = new FileDataStoreManager();

            //The file datastore can load different types of files such as NCOM, XCOM and RCOM
            //RCOM
            List<FileStreamItem> file_streams =  file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example1.rcom");

            //XCOM
            // List<FileStreamItem> file_streams = file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example1.xcom");

            //NCOM
            // List<FileStreamItem> file_streams = file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example1.ncom");

            //Get all available streams in the data store
            List<IStreamItem> streams = file_ds_manager.GetAllStreamItems();

            foreach(var stream in streams)
            {
                System.Console.WriteLine(stream.DeviceProductName + "   " + stream.DeviceProductType);
            }

            List<Tuple<uint, OxTS.NavLib.Common.Measurement.MeasurementItem>> measurements = new List<Tuple<uint, OxTS.NavLib.Common.Measurement.MeasurementItem>>();
            measurements.Add(new Tuple<uint, OxTS.NavLib.Common.Measurement.MeasurementItem>(streams[0].StreamId, new OxTS.NavLib.Common.Measurement.MeasurementItem("Nano")));
            //Add additional measurements here depending on the stream type

            file_ds_manager.ConfigureMeasurementsToRead(measurements, OxTS.NavLib.Common.Enums.MeasurementType.Multiple);


            //increment of 10000000, gives up 10Hz
            file_ds_manager.DecodeData(file_ds_manager.GetStoreStartTime(), file_ds_manager.GetStoreEndTime(), 100000000);

            Int64 current_time = file_ds_manager.GetStoreStartTime();
            Int64 end_time = file_ds_manager.GetStoreEndTime();

            Int64 One_Second = 1000000000;


            //For each stream in datastore
            while (current_time < end_time)
            {
                List<MeasurementValue> meas_data = file_ds_manager.GetStreamDataForInstant(current_time, 0);

                foreach (MeasurementValue data in meas_data)
                {
                    System.Console.WriteLine(data.MeasurementName + " : " + data.ValueString);

                }

                System.Console.WriteLine();

                current_time += One_Second;
            }

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadLine();

            file_ds_manager.Dispose();
        }
    }
}
