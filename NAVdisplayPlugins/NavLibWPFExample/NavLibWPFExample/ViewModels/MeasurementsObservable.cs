using System;
using System.Collections.Generic;
using System.Linq;
using OxTS.NavLib.Common.Enums;
using OxTS.NavLib.Common.Measurement;
using OxTS.NavLib.RealTime;
using OxTS.NavLib.DataStoreManager.Interface;

namespace NavLibWPFExample
{
    public class MeasurementsObservable : ObservableObject
    {
        #region Construction

        /// <summary>
        /// Use this constructor when you want to use a manually chosen stream, still need the WarmUpPluginSettingsObservable for access
        /// to the RTDS manager
        /// </summary>
        /// <param name="wu_plugin_settings"></param>
        public MeasurementsObservable(IRealTimeDataStoreManagerAsync rtdsman, IStreamItemViewModel manually_chosen_stream)
        {
            //Initialiase internal members/references
            m_rtdsman                   = rtdsman;
            m_stream_item               = manually_chosen_stream;
            m_history_measurement_list  = new List<List<MeasurementValue>>();
            m_measurement_list          = new List<MeasurementValue>();
            m_measurement_request_list  = new List<Tuple<uint, string>>();
                                        
            //Default values           
            HistorySize                 = TimeSpan.FromMilliseconds(3000);
            NewDataOnly                 = false;
            MeasurementType             = MeasurementType.Multiple;
            m_is_valid                  = false;

            CallerId                    = ulong.MaxValue;

            //Set a default batch of measurements
            SetMeasurements(
                new List<string>
                {
                    "Ax",
                    "Ay",
                    "Az",
                    "Lat",
                    "Lon",
                },
                false,
                TimeSpan.Zero,
                false
            );

            //Pass the measurements we just set to the real time data store
            ConfigureMeasurements();
        }

        #endregion

        #region Properties

        public bool IsValid     { get { return m_is_valid; } set { OnPropertyChanged("IsValid"); m_is_valid = value; } }

        public ulong CallerId   { get; set; }

        /// <summary>
        /// List of configured properties for easy iterating
        /// </summary>
        public List<MeasurementValue> MeasurementList { get { return m_measurement_list; } }

        /// <summary>
        /// List of configured history properties for easy iterating
        /// </summary>
        public List<List<MeasurementValue>> HistoryMeasurementList { get { return m_history_measurement_list; } }

        /// <summary>
        /// History size of the measurements if you want to get it from GetStreamData();
        /// This is set by default to the value in the WarmUpPluginSettingsObservable object 
        /// </summary>
        public TimeSpan HistorySize { get; set; }

        /// <summary>
        /// Setting this to true will make the real time data store provide only the new data in the case where the request internal is 
        /// shorter than the history size 
        /// </summary>
        public bool NewDataOnly { get; set; }

        /// <summary>
        /// Use this measurement to set whether or not you want to have history when requesting the datafrom the RTDS. Use Multiple for single
        /// data points and MultipleStreamed for a list of data points with history. When using MultipleStreamed measurements the default 
        /// settings are: HistorySize = WarmUpPluginSettingsObservable.DefaultGetHistorySize, NewDataOnly = true 
        /// </summary>
        public MeasurementType MeasurementType { get; set; }

        public int ConfiguredMeasurementCount         { get { return m_measurement_list.Count; } }

        public int ConfiguredHistoryMeasurementCount  { get { return m_history_measurement_list.Count; } }


        //Properties can be exposed like this 
        public double Ax  { get { return GetMeasurement("Ax").Value;  } }
        public double Ay  { get { return GetMeasurement("Ay").Value;  } }
        public double Az  { get { return GetMeasurement("Az").Value;  } }
        public double Lat { get { return GetMeasurement("Lat").Value; } }
        public double Lon { get { return GetMeasurement("Lon").Value; } }
                                                                                                       
        #endregion

        #region Members

        private IRealTimeDataStoreManagerAsync    m_rtdsman;
        private List<List<MeasurementValue>>      m_history_measurement_list;
        private List<MeasurementValue>            m_measurement_list;
        private List<Tuple<uint, string>>         m_measurement_request_list;
        private bool                              m_is_valid;
        private IStreamItemViewModel              m_stream_item;

        #endregion

        #region Class Methods

        public void SetMeasurements(List<string> required_measurements, bool history_required, TimeSpan history_size , bool new_data)
        {

            //Update the request list
            foreach( string measurement_name in required_measurements)
            {
                m_measurement_request_list.Add(new Tuple<uint, string>(m_stream_item.IStreamItem.StreamId, measurement_name));
            }

            //Set the configurations
            if (history_required)
            {
                HistorySize     = history_size;
                MeasurementType = MeasurementType.MultipleStreamed;
                NewDataOnly     = new_data;
            }
            else
            {
                MeasurementType = MeasurementType.Multiple;
            }
        }

        public void ConfigureMeasurements()
        {
            IsValid = false;

            //Request the required measurements from the RTDSMan for each measurement
            if (m_stream_item.IStreamItem != null)
            {
                //Configure the real time DS manager with the settings for this measurement
                CallerId = m_rtdsman.ConfigureMeasurementsToRead(m_measurement_request_list, MeasurementType).Result;

                //Set the history size and new data setting for the measurement for MultipleStreamed measurements
                if (MeasurementType == MeasurementType.MultipleStreamed)
                {
                    m_rtdsman.GetRealTimeData(CallerId).Result.SetHistorySize(HistorySize);

                    if (NewDataOnly)
                    {
                        ((RealTimeDataStream)m_rtdsman.GetRealTimeData(CallerId).Result).NewDataOnly = true;
                    }
                }

            }
        }

        /// <summary>
        /// Class that returns a single MeasurementValue obtained via a GetData() call
        /// </summary>
        /// <param name="measurement_name"></param>
        /// <returns></returns>
        public MeasurementValue GetMeasurement(string measurement_name)
        {
            if (m_measurement_list.Count > 0)
            {
                foreach (MeasurementValue measurement in m_measurement_list)
                {
                    if (measurement.MeasurementName == measurement_name)
                    {
                        return m_measurement_list.Find(f => f.MeasurementName == measurement_name);
                    }
                }
            }

            return new MeasurementValue();
        }

        /// <summary>
        /// Class that returns a List<MeasurementValue> obtained via a GetStreamData() call
        /// </summary>
        /// <param name="measurement_name"></param>
        /// <returns></returns>
        public List<MeasurementValue> GetHistoryMeasurement(string measurement_name)
        {
            if (m_history_measurement_list.Count > 0)
            {
                for (int list_count = 0; list_count < m_history_measurement_list.Count; ++list_count)
                {
                    if(m_history_measurement_list[list_count].Count > 0)
                    {
                        if (m_history_measurement_list[list_count][0].MeasurementName == measurement_name)
                        {
                            return m_history_measurement_list[list_count];
                        }
                    }
                }
            }

            return new List<MeasurementValue>();
        }

        /// <summary>
        /// Updates the measurments requested according to the PropertyMeasurementType (i.e. Multiple/Multiple Streamed) 
        /// </summary>
        public void UpdateMeasurementList()
        {
            if (MeasurementType == MeasurementType.MultipleStreamed)
            {
                //Enable new data if the measurement requires it
                //TODO:See if this needs moving to another location for efficiency?
                if (NewDataOnly && !((RealTimeDataStream)m_rtdsman.GetRealTimeData(CallerId).Result).NewDataOnly)
                {
                    ((RealTimeDataStream)m_rtdsman.GetRealTimeData(CallerId).Result).NewDataOnly = true;
                }

                List<List<MeasurementValue>> output_measurements = m_rtdsman.GetRealTimeData(CallerId).Result.GetStreamData();

                if (output_measurements.Count > 0)
                {
                    m_history_measurement_list.Clear();

                    //Take only the measurements, leaving out the Trace start/stop List
                    m_history_measurement_list.AddRange(output_measurements.GetRange(0, output_measurements.Count - 1));

                    //Raise the measurement changed event. This is why the measurement names need to be the same as 
                    //in the datastore, its so simple right now!!!
                    foreach (List<MeasurementValue> measurement in m_history_measurement_list)
                    {
                        if (measurement.Count > 0)
                        {
                            //Check to see if any of the properties are invalid and set validy flag of this measurement
                            OnPropertyChanged(measurement[0].MeasurementName);

                            //Trying this with FirstOrDefault as it may be faster than iterating over the whole List
                            IsValid = ((measurement.FirstOrDefault(e => e.IsDataOK == false) != null)) ? false : true;
                        }
                    }
                }
                else
                {
                    IsValid = false;
                }
            }
            else if (MeasurementType == MeasurementType.Multiple)
            {
                List<MeasurementValue> output_measurements = m_rtdsman.GetRealTimeData(CallerId).Result.GetData();

                m_measurement_list.Clear();
                m_measurement_list.AddRange(output_measurements);

                //Check to see if any of the properties are invalid and set validy flag of this measurement
                IsValid = (m_measurement_list.Exists(e => e.IsDataOK == false)) ? false : true;

                //Raise the measurement changed event. This is why the measurement names need to be the same as 
                foreach (MeasurementValue measurement in output_measurements)
                {
                    OnPropertyChanged(measurement.MeasurementName);
                }
            }
            else
            {
                throw new Exception("PropertyMeasurmentType not set or is invalid");
            }
        }

        //Provide the OxTS name for the measurement 
        public string FindOxTSName(string measurement_name)
        {
            return m_rtdsman.GetMeasurementCategorisation().Result.GetNavMeasurement(measurement_name).FullName;
        }



        #endregion

    }
    
}
