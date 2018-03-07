using OxTS.NavLib.DataStoreManager.Interface;
using OxTS.NavLib.PluginInterface;
using OxTS.NavLib.StreamItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace DisplayDataStoreStreamInfo
{
    /// <summary>
    /// Main Plugin class inheriting Form and INAVdisplayPlugin 
    /// </summary>
    [Export(typeof(INAVdisplayPlugin))]
    public partial class DisplayDatastoreStreamInfo : Form, INAVdisplayPlugin
    {
        /// <summary>
        /// Datastore access
        /// </summary>
        private IRealTimeDataStoreManagerAsync m_rtds_man;

        /// <summary>
        /// Timer to control update event
        /// </summary>
        private Timer update_timer;

        /// <summary>
        /// Constructor
        /// </summary>
        public DisplayDatastoreStreamInfo()
        {
            InitializeComponent();

            update_timer = new Timer();
            update_timer.Interval = 1000;
            update_timer.Tick += new EventHandler(UpdateInfo);
            update_timer.Start();
        }

        /// <summary>
        /// Update stream information event
        /// </summary>
        private void UpdateInfo(object sender, System.EventArgs e)
        {
            if (m_rtds_man != null)
            {
                List<IStreamItem> streams = m_rtds_man.GetAllStreamItems().Result;

                String diagnostic_info = "streamId\t\t" + "TagID".PadRight(20) + "\tGroupId\t\t" + "ResourceId".PadRight(20) + "\t\tAddress\n";

                foreach (IStreamItem item in streams)
                {
                    String user_tag = item.UserTag;
                    if (user_tag.Length > 20)
                    {
                        user_tag = user_tag.Substring(0, 20);
                    }
                    else
                    {
                        user_tag = user_tag.PadRight(20);
                    }

                    diagnostic_info += item.StreamId.ToString() + "\t\t" + user_tag + "\t" + item.GetGroupId() + "\t\t" + item.ResourceId.ToString().PadRight(20) + "\t\t" + item.Address + "\n";
                }

                show_info.Text = diagnostic_info;
            }
        }

        /// <summary>
        /// Should the launching application allow multiple instances of this plugin to be opened?
        /// </summary>
        public bool MultipleInstancesAllowed
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// PLugin type, use Utility for plugins to appear in the Utility menu in NAVdisplay
        /// </summary>
        public PluginTypes PluginType
        {
            get
            {
                return PluginTypes.Utility;
            }
        }

        /// <summary>
        /// Called from the launching application to give the plugin access to the datastore
        /// </summary>
        /// <param name="rtds_man"></param>
        public void ConnectToDatastore(IRealTimeDataStoreManagerAsync rtds_man)
        {
            m_rtds_man = rtds_man;
        }

        /// <summary>
        /// Implementation of IPluginBase function
        /// </summary>
        /// <returns>Name that will appear in NAVdisplay utilities menu</returns>
        public string GetMenuTextKey()
        {
            return "NAVlib Plugin";
        }

        /// <summary>
        /// Implementation of IPluginBase function
        /// </summary>
        /// <returns>Plugin version</returns>
        public string GetVersion()
        {
            return "1.0.0.1";
        }

        /// <summary>
        /// Implementation of IPluginBase function
        /// </summary>
        /// <returns>Internal name tof plugin</returns>
        public string GetTypeName()
        {
            return "NAVlib Plugin";
        }

        /// <summary>
        /// Called from the launching application to get licence information
        /// </summary>
        /// <returns></returns>
        public string GetNAVlibLicense()
        {
           return "Enter your license here";
        }

        /// <summary>
        ///Called from the launching application when the plugin is selected
        /// </summary>
        public void LaunchPlugin()
        {
            Show();
        }
    }
}
