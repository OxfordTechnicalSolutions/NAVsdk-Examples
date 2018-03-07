using OxTS.NavLib.Common.Measurement;
using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.StreamItems;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Decoding_File_Data_Direct_Example
{
    class Program
    {
        /**********************************************************************************
        Decoding File Data Direct

        This example shows how to decode stream data contained within a file using the 
        "direct" method.
        **********************************************************************************/
        static void Main(string[] args)
        {
            //create new realtime data store manager
            FileDataStoreManager file_ds_manager = new FileDataStoreManager();

            file_ds_manager.AddFile(@"..\..\..\..\Example_Data_Files\example1.ncom");

            //Get all available streams in the data store
            List<IStreamItem> streams = file_ds_manager.GetAllStreamItems();

            //No need to configure measurements or decode when using direct but we still need the list of measurements with stream id
            List<OxTS.NavLib.Common.Measurement.MeasurementItem> measurements = new List<OxTS.NavLib.Common.Measurement.MeasurementItem>();
            measurements.Add(new OxTS.NavLib.Common.Measurement.MeasurementItem("Nano"));
            measurements.Add(new OxTS.NavLib.Common.Measurement.MeasurementItem("Ax"));
            measurements.Add(new OxTS.NavLib.Common.Measurement.MeasurementItem("Ay"));
            measurements.Add(new OxTS.NavLib.Common.Measurement.MeasurementItem("Az"));


            Int64 current_time = file_ds_manager.GetStoreStartTime();
            Int64 end_time = file_ds_manager.GetStoreEndTime();
            Int64 One_Second = 1000000000;

            //Tell the data store to seek to the correct position
            file_ds_manager.BeginDirectDecode(current_time);

            //For each stream in datastore
            while (current_time < end_time)
            {
                List<MeasurementValue> meas_data = file_ds_manager.GetDirectStreamDataForInstant(current_time, streams[0].StreamId, measurements);

                foreach (MeasurementValue data in meas_data)
                {
                    System.Console.WriteLine(data.MeasurementName + " : " + data.ValueString);
                }

                System.Console.WriteLine();

                current_time += One_Second;
            }

            //close handles to stream data
            file_ds_manager.EndDirectDecode();

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadLine();

        }

    }
}
