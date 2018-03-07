using OxTS.NavLib.Common.Enums;
using OxTS.NavLib.Common.Measurement;
using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.RealTime;
using OxTS.NavLib.StreamItems;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Get_Direct_Measurements
{
    class Program
    {
        private static ulong caller_id;

        /**********************************************************************************
Get Direct Measurements

This example shows how to add measurements to be retrieved from a stream and how 
to then retrieve those measurements and match the values to the names.
**********************************************************************************/
        static void Main(string[] args)
        {
            RealTimeDataStoreManager realtime_ds_manager = new RealTimeDataStoreManager();

            //dictionary to store stream ids aand associated database
            Dictionary<uint, RealTimeDataBase> stream_data_dict = new Dictionary<uint, RealTimeDataBase>();

            //wait a few seconds for some streams to initialize
            Thread.Sleep(3000);

            //Get all stream items in realtime data store
            List<IStreamItem> streams = realtime_ds_manager.GetAllStreamItems();
            List<IStreamItem> setup_streams = new List<IStreamItem>();

            //dictionary to remember which measurements have been added to which streams
            Dictionary<uint, List<String>> measurements_added = new Dictionary<uint, List<string>>();

            //for each realtime stream
            foreach (IStreamItem stream in streams)
            {
                //measurements to add for stream
                List<Tuple<uint, string>> stream_measurements = new List<Tuple<uint, string>>();

                //Different measuremnts are available in different streams
                switch (stream.CodecType)
                {
                    //BCOM streams for base units
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_BCOM:
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Nano"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Lat"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Lon"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Alt"));
                        break;
                    //MCOM, NCOM for RT units
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_MCOM:
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_NCOM:
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Nano"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Ax"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Ay"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Az"));
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Dist3d"));
                        break;
                    //RCOM streams for Range units
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_RCOM:
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Nano"));
                        break;
                    //CAN stream
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_CAN:
                        stream_measurements.Add(new Tuple<uint, string>(stream.StreamId, "Nano"));
                        break;
                    //There are other streams possible but we just ignore these here
                    default:
                        break;
                }

                Console.WriteLine("Added measurements to stream" + stream.StreamId);
                //Configure the measurements to be read, StreamId is used as a unique identifier here for when we want to retrieve the measurements
                caller_id = realtime_ds_manager.ConfigureMeasurementsToRead(stream_measurements, MeasurementType.Multiple);

                if (caller_id != uint.MaxValue)
                {
                    //Get database associated with that stream
                    RealTimeDataBase database = realtime_ds_manager.GetRealTimeData(caller_id);

                    //add original stream id and associated database to the dictionary
                    //we use original stream id in case the stream disconnects/reconnects which will
                    //cause a new stream id to be assigned
                    stream_data_dict.Add(stream.OriginalStreamId, database);
                }
            }

            while (true)
            {
                //For each stream
                foreach (IStreamItem stream in streams)
                {
                    //Get data from the database
                    List<MeasurementValue> output_measurements = stream_data_dict[stream.OriginalStreamId].GetData();

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Measurements from stream " + stream.StreamId + "   " + stream.DeviceSerialNumber + "   TYPE: " + stream.CodecName);
                    Console.WriteLine("----------------------------------------");


                        for (int i = 0; i < output_measurements.Count; ++i)
                        {
                            Console.WriteLine(output_measurements[i].MeasurementName + ":" + output_measurements[i].ValueString);
                        }
                    

                    Thread.Sleep(1000);
                }
            }


            realtime_ds_manager.Dispose();
        }

    }
}
