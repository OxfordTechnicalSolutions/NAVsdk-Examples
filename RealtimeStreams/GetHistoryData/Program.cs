using OxTS.NavLib.Common.Enums;
using OxTS.NavLib.Common.Measurement;
using OxTS.NavLib.DataStoreManager.Manager;
using OxTS.NavLib.RealTime;
using OxTS.NavLib.StreamItems;
using OxTS.NavLib.Common.Licensing;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GetHistoryData
{
    class Program
    {
        /**********************************************************************************
         Get History Data

         This example shows how to add measurements to be retrieved from a stream and how 
         to then retrieve those measurements and match the values to the names.  It also
         shows how to get a trace of historical data between 
         **********************************************************************************/
        static void Main(string[] args)
        {
            RealTimeDataStoreManager realtime_ds_manager = null;
            try {
            //create new realtime data store manager
            realtime_ds_manager = new RealTimeDataStoreManager();
            }
            //catch any licensing exceptions
            catch (OxTSLicenseNotFoundException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            //wait a few seconds for some streams to initialize
            Thread.Sleep(3000);

            //Get all stream items in realtime data store
            List<IStreamItem> streams = realtime_ds_manager.GetAllStreamItems();

            //dictionary to remember which measurements have been added to which streams
            Dictionary<uint, List<String>> measurements_added = new Dictionary<uint, List<string>>();

            //dictionary of realtime databases for each stream
            Dictionary<uint, RealTimeDataStream> real_time_streams = new Dictionary<uint, RealTimeDataStream>();

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
                        break;
                    //RCOM streams for Range units
                    case OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_RCOM:
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
                //The MeasurementType used defines how the data is packaged, using MultipleStreamed allows for history data
                ulong CallerId = realtime_ds_manager.ConfigureMeasurementsToRead(stream_measurements, MeasurementType.MultipleStreamed);
                
                //Add configured measurements to our dictionary
                measurements_added.Add(stream.StreamId, stream_measurements.ConvertAll<String>(o => o.Item2));

                //setup a real time data base to get streamed data using the caller id
                RealTimeDataStream data_stream = realtime_ds_manager.GetRealTimeData(CallerId) as RealTimeDataStream;

                //Set the history to show a maximum of 5 seconds of data
                data_stream.SetHistorySize(new TimeSpan(0, 0, 30));

                //Set the data stream to only return new data
                data_stream.NewDataOnly = true;

                //Add our new realtime database to dictionary
                real_time_streams.Add(stream.OriginalStreamId, data_stream);  
            }

            while (true)
            {
                //For each stream
                foreach (IStreamItem stream in streams)
                {
                    //get output measurements from data base
                    List<List<MeasurementValue>> output_measurements = real_time_streams[stream.OriginalStreamId].GetStreamData();

                    //the data list updates in realtime so will not return if it is currently being written             
                    if (output_measurements.Count > 0)
                    {
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine("Measurements from stream " + stream.Address + " " + stream.StreamId + "   TYPE: " + stream.DeviceSerialNumber + "  " + stream.CodecName);
                        Console.WriteLine("----------------------------------------");

                        //Output data using our dictionary to find which value corresponds to which name
                        if (measurements_added[stream.StreamId].Count <= output_measurements.Count)
                        {
                            for (int i = 0; i < measurements_added[stream.StreamId].Count; ++i)
                            {
                                if (output_measurements[i].Count > 0)
                                {
                                    //Oldest data will be at the beginning, newest at the end
                                    Console.WriteLine(measurements_added[stream.StreamId][i] + ": " + output_measurements[i].Count + " Available");
                                    Console.WriteLine("Oldest " + measurements_added[stream.StreamId][i] + ":" + output_measurements[i][0].Value + " : " + +output_measurements[i][0].Time);
                                    Console.WriteLine("Newest " + measurements_added[stream.StreamId][i] + ":" + output_measurements[i][output_measurements[i].Count - 1].Value + " : " + +output_measurements[i][output_measurements[i].Count - 1].Time);
                                }
                            }
                        }

                        Thread.Sleep(500);
                    }             
                }
            }
        }
    }
}
