using OxTS.NavLib.DataStoreManager.Interface;
using OxTS.NavLib.Enum;
using OxTS.NavLib.StreamItems;


namespace NavLibWPFExample
{
    public class IStreamItemViewModel : ObservableObject
    {
        #region Construction

        /// <summary>
        /// Creates an observable wrapper around the IStreamItem, and extends the information available from it with a measurments class
        /// </summary>
        /// <param name="istream_item"></param>
        /// <param name="wu_plugin_settings"></param>
        public IStreamItemViewModel(IStreamItem istream_item, IRealTimeDataStoreManagerAsync rtdsman)
        {
            IStreamItem          = istream_item;
            DeviceId             = istream_item.DeviceId;
            DeviceProductName    = istream_item.DeviceProductName;
            DeviceProductModel   = istream_item.ProductModel;
            DeviceProductType    = istream_item.DeviceProductType;
            DeviceSerialNumber   = istream_item.DeviceSerialNumber;
            Address              = istream_item.Address;
            CodecType            = istream_item.CodecType;
            Alive                = istream_item.Alive;
            MaxLagTime           = 10;
            CurrentLagTime       = istream_item.LagTime < 10 ? (int)istream_item.LagTime : 10;      
            Initialised          = istream_item.Initialised;

            Measurements = new MeasurementsObservable(rtdsman, this);
        }

        #endregion

        #region Members
        private IStreamItem       m_istream_item;
        private string            m_vm_stream_id;
        private string            m_vm_stream_product_name;
        private string            m_vm_stream_product_model;
        private string            m_vm_stream_product_type;
        private string            m_vm_stream_serial_number;
        private string            m_vm_stream_address;
        CODEC_TYPE                m_vm_stream_codec_type;
        private bool              m_alive;
        private int               m_max_lag_time;
        private int               m_current_lag_time;
        private bool              m_initialised;

        private MeasurementsObservable m_measurements;


        #endregion

        #region Properties

        public MeasurementsObservable Measurements        {get { return m_measurements; }            set {
                OnPropertyChanged("Measurements");        m_measurements            = value;}}
        public IStreamItem            IStreamItem         {get { return m_istream_item; }            set { OnPropertyChanged("IStreamItem");         m_istream_item            = value;}}
        public string                 DeviceId            {get { return m_vm_stream_id; }            set { OnPropertyChanged("DeviceId");            m_vm_stream_id            = value;}}
        public string                 DeviceProductName   {get { return m_vm_stream_product_name; }  set { OnPropertyChanged("DeviceProductName");   m_vm_stream_product_name  = value;}}
        public string                 DeviceProductModel  {get { return m_vm_stream_product_model; } set { OnPropertyChanged("DeviceProductModel");  m_vm_stream_product_model = value;}}
        public string                 DeviceProductType   {get { return m_vm_stream_product_type; }  set { OnPropertyChanged("DeviceProductType");   m_vm_stream_product_type  = value;}}
        public string                 DeviceSerialNumber  {get { return m_vm_stream_serial_number; } set { OnPropertyChanged("DeviceSerialNumber");  m_vm_stream_serial_number = value;}}
        public string                 Address             {get { return m_vm_stream_address; }       set { OnPropertyChanged("Address");             m_vm_stream_address       = value;}}
        public CODEC_TYPE             CodecType           {get { return m_vm_stream_codec_type; }    set { OnPropertyChanged("CodecType");           m_vm_stream_codec_type    = value;}}
        public bool                   Alive               {get { return m_alive; }                   set { OnPropertyChanged("Alive");               m_alive                   = value;}}
        public int                    MaxLagTime          {get { return m_max_lag_time; }            set { OnPropertyChanged("MaxLagTime");          m_max_lag_time            = value;}}
        public int                    CurrentLagTime      {get { return m_current_lag_time; }        set { OnPropertyChanged("CurrentLagTime");      m_current_lag_time        = value;}}
        public bool                   Initialised         {get { return m_initialised; }             set { OnPropertyChanged("Initialised");         m_initialised             = value;}}

        #endregion

        #region Methods
        ///// <summary>
        ///// Update with an supplied IStreamItem, designed for updating with the same IStreamItem object initially 
        ///// </summary>
        ///// <param name="istream_item"></param>
        //public void UpdateIStreamItemViewModel(IStreamItem istream_item)
        //{
        //    IStreamItem          = istream_item;
        //    DeviceId             = istream_item.DeviceId;
        //    DeviceProductName    = istream_item.DeviceProductName;
        //    DeviceProductModel   = istream_item.ProductModel;
        //    DeviceProductType    = istream_item.DeviceProductType;
        //    DeviceSerialNumber   = istream_item.DeviceSerialNumber;
        //    Address              = istream_item.Address;
        //    CodecType            = istream_item.CodecType;
        //    Alive                = istream_item.Alive;
        //    CurrentLagTime       = istream_item.LagTime < 10 ? (int)istream_item.LagTime : 10;
        //    Initialised          = istream_item.Initialised;

        //}

        /// <summary>
        /// Update with the latest from the currently stored IStreamItem
        /// </summary>
        /// <param name="istream_item"></param>
        public void Update()
        {
            IStreamItem          = m_istream_item;
            DeviceId             = m_istream_item.DeviceId;
            DeviceProductName    = m_istream_item.DeviceProductName;
            DeviceProductModel   = m_istream_item.ProductModel;
            DeviceProductType    = m_istream_item.DeviceProductType;
            DeviceSerialNumber   = m_istream_item.DeviceSerialNumber;
            Address              = m_istream_item.Address;
            CodecType            = m_istream_item.CodecType;
            Alive                = m_istream_item.Alive;
            CurrentLagTime       = m_istream_item.LagTime < 10 ? (int)m_istream_item.LagTime : 10;
            Initialised          = m_istream_item.Initialised;
        }


        #endregion
    }

}

