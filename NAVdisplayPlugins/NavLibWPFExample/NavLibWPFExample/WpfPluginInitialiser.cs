using OxTS.NavLib.DataStoreManager.Interface;
using OxTS.NavLib.PluginInterface;
using System.ComponentModel.Composition;
using System.Xml.Serialization;

namespace NavLibWPFExample
{
    [Export(typeof(INAVdisplayPlugin))]
    public class WpfPluginInitialiser: INAVdisplayPlugin
    {
        IRealTimeDataStoreManagerAsync rtdsman_async;
        public void Show()
        {
            MainWindow              = new MainWindow();

            SelectUnitView          = new SelectUnitViewModel(rtdsman_async);

            MainWindow.DataContext  = SelectUnitView;
            MainWindow.Visibility   = System.Windows.Visibility.Visible;
            serialiser = new WPFExampleSerialiser();
        }

        private IXmlSerializable serialiser;
        public MainWindow MainWindow { get; set; }
        public SelectUnitViewModel SelectUnitView { get; set; }

        #region INavDisplayPlugin Implementation
        public bool MultipleInstancesAllowed
        {
            get
            {
                return false;
            }
        }

        public PluginTypes PluginType
        {
            get
            {
                return PluginTypes.Utility;
            }
        }

        public IXmlSerializable Serialiser => serialiser;

        public void ConnectToDatastore(IRealTimeDataStoreManagerAsync rtds_man)
        {
            rtdsman_async = rtds_man;
        }

        public void Dispose()
        {
        }

        public string GetMenuTextKey()
        {
            return "NavLibWPFExample";
        }

        public string GetNAVlibLicense()
        {
            return "Enter License key here";
        }

        public string GetTypeName()
        {
            return "NavLibWPFExample";
        }

        public string GetVersion()
        {
            return "0.1.0.0";
        }

        /// <summary>
        ///Called from the launching application when the plugin is selected
        /// </summary>
        public void LaunchPlugin()
        {
            Show();
        }

        #endregion
    }
}
