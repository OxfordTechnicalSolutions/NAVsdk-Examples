using OxTS.NavLib.DataStoreManager.Interface;
using OxTS.NavLib.StreamItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NavLibWPFExample
{
    public class SelectUnitViewModel : ObservableObject
    {
        #region Construction

        public SelectUnitViewModel(IRealTimeDataStoreManagerAsync rtdsman_async)
        {
            //Initialise the real time data store manager, this will serve as the main object used to access 
            //the connected device streams and their measurements
            RTDSManAsync = rtdsman_async;

            //Set up an obeservable collection with simple lock so the GUI and Task
            //threads can access it safely
            Streams = new ObservableCollection<IStreamItemViewModel>();
            BindingOperations.EnableCollectionSynchronization(Streams, m_streams_lock);

            m_streams = new List<IStreamItem>();

            //Run a function to update the list of units on a timer
            m_timer = new Timer(UpdateList, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));

            //Start the task to update the observable properties for the currently selected stream
            UpdateCurrentStreamMeasurementsPeriodic();
        }

        #endregion

        #region Properties

        public ObservableCollection<IStreamItemViewModel> Streams
        {
            get { return m_vm_streams; }
            set
            {
                OnPropertyChanged("Streams"); m_vm_streams = value;
            }
        }
        public IRealTimeDataStoreManagerAsync RTDSManAsync
        {
            get { return m_rtdsman; }
            set
            {
                OnPropertyChanged("RTDSMan"); m_rtdsman = value;
            }
        }
        public bool WindowVisibility
        {
            get { return m_window_visibility; }
            set
            {
                OnPropertyChanged("WindowVisibility"); m_window_visibility = value;
            }
        }

        /// <summary>
        /// This property is bound directily to the 
        /// </summary>
        public IStreamItemViewModel CurrentlySelectedStream { get { return m_currently_selected_istreamitem_vm; } set { OnPropertyChanged("CurrentlySelectedStream"); m_currently_selected_istreamitem_vm = value; } }

        #endregion

        #region Members

        private ObservableCollection<IStreamItemViewModel> m_vm_streams;
        private IRealTimeDataStoreManagerAsync m_rtdsman;
        private IStreamItemViewModel m_currently_selected_istreamitem_vm;

        private List<IStreamItem> m_streams;
        private Timer m_timer;
        private object m_streams_lock = new object();
        private bool m_window_visibility;

        #endregion

        #region Class methods

        /// <summary>
        /// The main function that will populate and update the observable list of units accessible
        /// </summary>
        /// <param name="pay_load"></param>
        private void UpdateList(object pay_load)
        {
            //Create a List from the Streams object, for access to List functions
            List<IStreamItemViewModel> vm_streams_list = new List<IStreamItemViewModel>(m_vm_streams);

            //Populate the internal stream list from RTDSMAN
            m_streams = RTDSManAsync.GetAllStreamItems().Result;

            //Filter for only the streams we want
            m_streams = FilterStreams(m_streams);

            //Create an observable object for each of the IStreamItems in the list
            foreach (IStreamItem istream_item in m_streams)
            {
                IStreamItemViewModel existing_istream_vm = null;

                //Lock the Streams property for editing
                lock (m_streams_lock)
                {
                    //Local copy for potiential match 
                    existing_istream_vm = Streams.FirstOrDefault(e => e.IStreamItem.StreamId == istream_item.StreamId);
                }

                if (existing_istream_vm != null)
                {
                    if (istream_item.Alive)
                    {
                        existing_istream_vm.Update();
                    }
                    else
                    {
                        //Lock the Streams property for editing
                        lock (m_streams_lock)
                        {
                            Streams.Remove(existing_istream_vm);
                        }
                    }
                }
                else
                {
                    if (istream_item.Alive)
                    {
                        //Lock the Streams property for editing
                        lock (m_streams_lock)
                        {
                            Streams.Add(new IStreamItemViewModel(istream_item, RTDSManAsync));
                        }
                    }
                }
            }
        }

        public void Show()
        {
            WindowVisibility = true;
        }

        /// <summary>
        /// Filters streams for a set of requirements
        /// </summary>
        /// <param name="stream_list"></param>
        /// <returns></returns>
        private List<IStreamItem> FilterStreams(List<IStreamItem> stream_list)
        {
            //Need to use the .ToList() here otherwise the collection is altered and an exception results
            foreach (IStreamItem istream_item in stream_list.ToList())
            {
                if (istream_item.IsXCOMStream() ||
                      istream_item.IsRebroadcast ||
                     (istream_item.CodecType != OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_NCOM))
                {
                    stream_list.Remove(istream_item);
                }
                else
                {
                    continue;
                }
            }

            return stream_list;
        }

        private Task UpdateCurrentStreamMeasurementsPeriodic()
        {

            return Task.Run(() =>
            {
                while (true)
                {

                    if (CurrentlySelectedStream != null)
                    {
                        CurrentlySelectedStream.Measurements.UpdateMeasurementList();
                        OnPropertyChanged("CurrentlySelectedStream");
                    }

                    //Wait for 100 milliseconds
                    Task.Delay(TimeSpan.FromMilliseconds(100)).Wait();
                }
            });
        }
        #endregion

    }

}

