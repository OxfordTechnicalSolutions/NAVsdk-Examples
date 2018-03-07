using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CLRDatastoreExample
{
    public partial class MainForm : Form
    {
        //datastore access
        private OxTS.NavLib.Realtime.DataStore m_datastore;

        private Mutex m_data_mutex;
        private List<String> m_display_measurements;

        private bool m_aspect_setup;

        //checkbox variables
        private bool m_show_basic_data;
        private bool m_show_interface_data;
        private bool m_show_resource_data;
        private bool m_show_navigation_data;

        private bool m_direct_mode;

        private List<OxTS.NavLib.Realtime.QueryStream> current_streams;
        private List<OxTS.NavLib.Realtime.QueryInterface> current_interfaces;
        private List<OxTS.NavLib.Realtime.QueryGroup> current_groups;
        private List<OxTS.NavLib.Realtime.QueryResource> current_resources;

        //List of streams with added measurements
        private List<Tuple<OxTS.NavLib.Realtime.Aspect, List<String>>> m_added_measurements;

        List<String> m_grid_measurements = new List<string>();


        public MainForm()
        {
            InitializeComponent();

            m_show_basic_data = true;
            m_show_interface_data = false;
            m_show_resource_data = false;
            m_show_navigation_data = false;

            m_display_measurements = new List<string>();
            AddBasicMeasurementsToDisplayList();

            m_direct_mode = true;
            m_aspect_setup = false;

            current_streams = new List<OxTS.NavLib.Realtime.QueryStream>();
            current_interfaces = new List<OxTS.NavLib.Realtime.QueryInterface>();
            current_groups = new List<OxTS.NavLib.Realtime.QueryGroup>();
            current_resources = new List<OxTS.NavLib.Realtime.QueryResource>();

            m_added_measurements = new List<Tuple<OxTS.NavLib.Realtime.Aspect, List<string>>>();
        }
        private void Init()
        {
            try
            {
                InitDatastore();
                InitTimer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception thrown:\n" + ex.Message);
            }

            m_data_mutex = new Mutex();
        }

        private void InitTimer()
        {
            m_ui_update_timer = new System.Windows.Forms.Timer(components);
            m_ui_update_timer.Interval = 100;
            m_ui_update_timer.Tick += new System.EventHandler(this.UpdateUIWithDecodedData);

            m_decode_timer = new System.Windows.Forms.Timer(components);
            m_decode_timer.Interval = 100;
            m_decode_timer.Tick += new System.EventHandler(this.DecodeData);

            m_ui_update_timer.Start();
            m_decode_timer.Start();
        }

        private void InitDatastore()
        {
            m_datastore = new OxTS.NavLib.Realtime.DataStore();
            OxTS.NavLib.Enum.RETURN_TYPE ret = m_datastore.start_ethernet(3000, 3001);
            ret &= m_datastore.start_ethernet(50475, 3001);
        }

        void AddAllMeasurements()
        {
            List<String> measurements = new List<string>();

            foreach (var stream in current_streams)
            {
                List<OxTS.NavLib.Realtime.StreamMeasurement> stream_measurements = new List<OxTS.NavLib.Realtime.StreamMeasurement>();
                m_datastore.query_stream_measurements(out stream_measurements, stream.StreamId);

                stream_measurements.RemoveAll(f => f.array);

                measurements.AddRange(stream_measurements.ConvertAll<String>(f => f.tag));
            }

            m_display_measurements.AddRange(measurements.Distinct());
        }

        void AddBasicMeasurementsToDisplayList()
        {
            m_display_measurements.Add("Nano");
            m_display_measurements.Add("StreamId");
            m_display_measurements.Add("ResourceEthIp");
            m_display_measurements.Add("SerialNumber");
            m_display_measurements.Add("ProductModel");
            m_display_measurements.Add("DevID");
            m_display_measurements.Add("StreamCodec");
            m_display_measurements.Add("ResourceTrans");
            m_display_measurements.Add("StreamCodecPacketRate");
        }

        void AddInterfaceMeasurementsToDisplayList()
        {
            m_display_measurements.Add("InterfaceId");
            m_display_measurements.Add("InterfaceType");
            m_display_measurements.Add("InterfacePortReceive");
            m_display_measurements.Add("InterfacePortSend");
        }

        void AddResourceMeasurementsToDisplayList()
        {
            m_display_measurements.Add("ResourceId");
            m_display_measurements.Add("ResourceCreation");
            m_display_measurements.Add("ResourceEthIp");
            m_display_measurements.Add("ResourceEthPort");
            m_display_measurements.Add("ResourceSerPort");
            m_display_measurements.Add("ResourceSerBaud");
            m_display_measurements.Add("ResourceSerDataBits");
            m_display_measurements.Add("ResourceSerStopBits");
            m_display_measurements.Add("ResourceSerParity");
            m_display_measurements.Add("ResourceFileSize");
            m_display_measurements.Add("ResourceTrans");
            m_display_measurements.Add("ResourceReceivedEvents");
            m_display_measurements.Add("ResourceReceivedBytes");
            m_display_measurements.Add("ResourceReceivedEventRate");
            m_display_measurements.Add("ResourceReceivedByteRate");
            m_display_measurements.Add("ResourceEntireModified");
            m_display_measurements.Add("ResourceEntireAge");
            m_display_measurements.Add("ResourceEntireLag");
        }

        void AddNavigationMeasurementsToDisplayList()
        {
            m_display_measurements.Add("Lat");
            m_display_measurements.Add("Lon");
            m_display_measurements.Add("Alt");
            m_display_measurements.Add("Heading");
            m_display_measurements.Add("Pitch");
            m_display_measurements.Add("Roll");
            m_display_measurements.Add("Vn");
            m_display_measurements.Add("Ve");
            m_display_measurements.Add("Vd");
            m_display_measurements.Add("Wx");
            m_display_measurements.Add("Wy");
            m_display_measurements.Add("Wz");
            m_display_measurements.Add("Ax");
            m_display_measurements.Add("Ay");
            m_display_measurements.Add("Az");
        }

        private void SetupStreamHierachy()
        {
            m_tv_device_mapping.BeginUpdate();
            m_tv_device_mapping.Nodes.Clear();

            TreeNode interface_node = new TreeNode();
            foreach (OxTS.NavLib.Realtime.QueryInterface ds_interface in current_interfaces)
            {
                List<TreeNode> resources = new List<TreeNode>();
                foreach (OxTS.NavLib.Realtime.QueryResource ds_resource in current_resources)
                {
                    if (ds_resource.InterfaceId == ds_interface.InterfaceId)
                    {
                        List<TreeNode> groups = new List<TreeNode>();
                        foreach (OxTS.NavLib.Realtime.QueryGroup ds_group in current_groups)
                        {
                            if (ds_group.ResourceId == ds_resource.ResourceId)
                            {
                                List<TreeNode> streams = new List<TreeNode>();
                                foreach (OxTS.NavLib.Realtime.QueryStream ds_stream in current_streams)
                                {
                                    if (ds_stream.GroupId == ds_group.GroupId)
                                    {
                                        streams.Add(new TreeNode(ds_stream.StreamId.ToString() + " --- " + ds_stream.ResourceAddress + " --- " + ds_stream.StreamProductName));
                                    }
                                }
                                groups.Add(new TreeNode(ds_group.GroupId.ToString(), streams.ToArray()));
                            }
                        }
                        resources.Add(new TreeNode(ds_resource.ResourceId.ToString() + " --- " + ds_resource.ResourceAddress, groups.ToArray()));
                    }
                }

                interface_node = new TreeNode(OxTS.NavLib.Util.EnumUtils.GetVerbose(ds_interface.InterfaceType), resources.ToArray());

                m_tv_device_mapping.Nodes.Add(interface_node);
                m_tv_device_mapping.ExpandAll();
            }

            m_tv_device_mapping.EndUpdate();
        }

        private void SetupAspects()
        {
            if (m_data_mutex.WaitOne())
            {
                m_display_measurements.Clear();


                if (m_show_basic_data)
                {
                    AddBasicMeasurementsToDisplayList();
                }

                if (m_show_interface_data)
                {
                    AddInterfaceMeasurementsToDisplayList();
                }

                if (m_show_resource_data)
                {
                    AddResourceMeasurementsToDisplayList();
                }

                if (m_show_navigation_data)
                {
                    AddNavigationMeasurementsToDisplayList();
                }


                m_datastore.update();

                List<OxTS.NavLib.Realtime.StoreMeasurement> sm = new List<OxTS.NavLib.Realtime.StoreMeasurement>();
                List<OxTS.NavLib.Realtime.QueryStream> old_streams = new List<OxTS.NavLib.Realtime.QueryStream>(current_streams);
                //get list of streams, interfaces, groups and resources
                m_datastore.query_streams(out current_streams);
                m_datastore.query_interfaces(out current_interfaces);
                m_datastore.query_groups(out current_groups);
                m_datastore.query_resources(out current_resources);

                m_added_measurements.Clear();
                m_datastore.direct_clear();
                m_datastore.retain_clear();

                current_streams = current_streams.FindAll(f => f.StreamCodec != OxTS.NavLib.Enum.CODEC_TYPE.CODEC_TYPE_CAN);
                foreach (OxTS.NavLib.Realtime.QueryStream qs in current_streams)
                {
                    OxTS.NavLib.Realtime.Aspect temp_aspect = new OxTS.NavLib.Realtime.Aspect();
                    List<string> added_measurement_names = new List<string>();
                    foreach (String measurement_name in m_display_measurements)
                    {
                        if (m_direct_mode)
                        {
                            //direct
                            if (m_datastore.direct_add_element(qs.StreamId, measurement_name) == OxTS.NavLib.Enum.RETURN_TYPE.RETURN_TYPE_NORMAL)
                            {
                                m_datastore.direct_use_element(ref temp_aspect, qs.StreamId, measurement_name);

                                added_measurement_names.Add(measurement_name);
                            }
                        }
                        else
                        {
                            //retain
                            if (m_datastore.retain_add_element(qs.StreamId, measurement_name) == OxTS.NavLib.Enum.RETURN_TYPE.RETURN_TYPE_NORMAL)
                            {
                                m_datastore.retain_use_element(ref temp_aspect, qs.StreamId, measurement_name);

                                added_measurement_names.Add(measurement_name);
                            }
                        }
                    }

                    m_added_measurements.Add(new Tuple<OxTS.NavLib.Realtime.Aspect, List<string>>(temp_aspect, added_measurement_names));
                }

                bool update_streams_list = false;

                if (old_streams.Count == current_streams.Count)
                {
                    foreach(OxTS.NavLib.Realtime.QueryStream stream in old_streams)
                    {
                        bool found_stream = false;

                        foreach (OxTS.NavLib.Realtime.QueryStream new_stream in current_streams)
                        {
                            if(stream.StreamId == new_stream.StreamId)
                            {
                                found_stream = true;
                            }
                        }

                        if(!found_stream)
                        {
                            update_streams_list = true;
                        }
                    }
                }
                else
                {
                    update_streams_list = true;
                }

                if (update_streams_list)
                {
                    SetupStreamHierachy();
                }

                m_data_mutex.ReleaseMutex();
            }



            m_aspect_setup = true;
        }


        private void DecodeData(object sender, EventArgs e)
        {
            System.Collections.Generic.List<OxTS.NavLib.Realtime.QueryStream> qss = new System.Collections.Generic.List<OxTS.NavLib.Realtime.QueryStream>();

            OxTS.NavLib.Enum.RETURN_TYPE ret = m_datastore.query_streams(out qss);

            if (qss.Count != current_streams.Count)
            {
                m_aspect_setup = false;
            }

            if (!m_aspect_setup)
            {
                SetupAspects();
            }

            m_datastore.update();

        }

        private void UpdateUIWithDecodedData(object sender, EventArgs e)
        {
            if (m_data_mutex.WaitOne())
            {
                if (m_aspect_setup)
                {
                    m_gv_stream_information.Rows.Clear();
                    m_gv_stream_information.Columns.Clear();

                    List<Tuple<List<String>, List<OxTS.NavLib.Lib.Dynamic>>> dynamic_list = new List<Tuple<List<String>, List<OxTS.NavLib.Lib.Dynamic>>>();
                    List<Tuple<List<String>, List<OxTS.NavLib.Lib.Variable<double>>>> variable_list = new List<Tuple<List<String>, List<OxTS.NavLib.Lib.Variable<double>>>>();

                    foreach (Tuple<OxTS.NavLib.Realtime.Aspect, List<String>> aspect_meas in m_added_measurements)
                    {
                        List<OxTS.NavLib.Lib.Dynamic> meas_list = new List<OxTS.NavLib.Lib.Dynamic>();
                        OxTS.NavLib.Realtime.Trace trace = new OxTS.NavLib.Realtime.Trace();

                        if (m_direct_mode)
                        {
                            //get measurements in a dynamic list
                            m_datastore.direct_get(aspect_meas.Item1, out meas_list);
                            if (aspect_meas.Item2.Count == meas_list.Count)
                            {
                                dynamic_list.Add(new Tuple<List<String>, List<OxTS.NavLib.Lib.Dynamic>>(aspect_meas.Item2, meas_list));
                            }
                        }
                        else
                        {
                            //get measurements in a trace
                            m_datastore.retain_get(aspect_meas.Item1, ref trace);
                            if (trace.get_strips().Count == aspect_meas.Item2.Count)
                            {
                                List<OxTS.NavLib.Lib.Variable<double>> var_list = new List<OxTS.NavLib.Lib.Variable<double>>();
                                foreach (OxTS.NavLib.Realtime.Strip strip in trace.get_strips())
                                {
                                    if (strip.size() > 0)
                                    {
                                        //get the latest data which should be at the end of the strip
                                        var_list.Add(strip.get_variables().Last());
                                    }
                                }

                                variable_list.Add(new Tuple<List<String>, List<OxTS.NavLib.Lib.Variable<double>>>(aspect_meas.Item2, var_list));
                            }
                        }
                    }

                    //aspect list + 1 as we also want to include the measurement name
                    for (int i = 0; i < m_added_measurements.Count + 1; ++i)
                    {
                        DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
                        dgvc.Visible = true;
                        dgvc.Width = 100;

                        m_gv_stream_information.Columns.Add(dgvc);
                    }

                    List<String> new_grid_measurements = new List<string>();
                    //Get measurement names
                    if (m_direct_mode)
                    {
                        foreach (Tuple<List<String>, List<OxTS.NavLib.Lib.Dynamic>> measurement_list in dynamic_list)
                        {
                            DataGridViewColumn column = new DataGridViewColumn();

                            for (int i = 0; i < measurement_list.Item1.Count; ++i)
                            {
                                int grid_position = new_grid_measurements.IndexOf(measurement_list.Item1[i]);

                                if (grid_position == -1)
                                {
                                    new_grid_measurements.Add(measurement_list.Item1[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Tuple<List<String>, List<OxTS.NavLib.Lib.Variable<double>>> measurement_list in variable_list)
                        {
                            DataGridViewColumn column = new DataGridViewColumn();

                            for (int i = 0; i < measurement_list.Item1.Count; ++i)
                            {
                                int grid_position = new_grid_measurements.IndexOf(measurement_list.Item1[i]);

                                if (grid_position == -1)
                                {
                                    new_grid_measurements.Add(measurement_list.Item1[i]);
                                }
                            }
                        }
                    }


                    List<String> inoldnotnew = m_grid_measurements.Except(new_grid_measurements).ToList();
                    List<String> innewnotold = new_grid_measurements.Except(m_grid_measurements).ToList();

                    if (inoldnotnew.Count() > 0 || innewnotold.Count() > 0)
                    {
                        m_grid_measurements = new_grid_measurements;
                    }


                    //populate grid view
                    foreach (String measurement in m_grid_measurements)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

                        cell.Value = measurement;
                        row.Cells.Add(cell);

                        if (m_direct_mode)
                        {
                            foreach (Tuple<List<String>, List<OxTS.NavLib.Lib.Dynamic>> measurement_list in dynamic_list)
                            {
                                cell = new DataGridViewTextBoxCell();

                                String measurement_display_value = "";
                                int meas_index = measurement_list.Item1.IndexOf(measurement);
                                if (meas_index != -1)
                                {
                                    cell.Value = measurement_list.Item2[meas_index].ToString();

                                    if (!measurement_list.Item2[meas_index].flags.good)
                                    {
                                        cell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                                    }
                                }
                                else
                                {
                                    cell.Style = new DataGridViewCellStyle { BackColor = Color.Black };
                                }

                                row.Cells.Add(cell);
                            }
                        }
                        else
                        {
                            foreach (Tuple<List<String>, List<OxTS.NavLib.Lib.Variable<double>>> measurement_list in variable_list)
                            {
                                cell = new DataGridViewTextBoxCell();

                                String measurement_display_value = "";
                                int meas_index = measurement_list.Item1.IndexOf(measurement);
                                if (meas_index != -1)
                                {
                                    cell.Value = measurement_list.Item2[meas_index].data.ToString();

                                    if (!measurement_list.Item2[meas_index].flags.good)
                                    {
                                        cell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                                    }
                                }
                                else
                                {
                                    cell.Style = new DataGridViewCellStyle { BackColor = Color.Black };
                                }

                                row.Cells.Add(cell);
                            }
                        }

                        m_gv_stream_information.Rows.Add(row);
                    }

                }
            }
            m_data_mutex.ReleaseMutex();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void cb_basic_CheckedChanged(object sender, EventArgs e)
        {
            m_show_basic_data = !m_show_basic_data;
            m_aspect_setup = false;
        }

        private void cb_interface_CheckedChanged(object sender, EventArgs e)
        {
            m_show_interface_data = !m_show_interface_data;
            m_aspect_setup = false;
        }

        private void cb_resource_CheckedChanged(object sender, EventArgs e)
        {
            m_show_resource_data = !m_show_resource_data;
            m_aspect_setup = false;
        }

        private void cb_navigation_CheckedChanged(object sender, EventArgs e)
        {
            m_show_navigation_data = !m_show_navigation_data;
            m_aspect_setup = false;
        }

        private void m_rb_retain_button_CheckedChanged(object sender, EventArgs e)
        {
            m_direct_mode = !m_rb_retain_button.Checked;
            m_aspect_setup = false;
        }

        private void m_rb_direct_button_CheckedChanged(object sender, EventArgs e)
        {
            m_direct_mode = m_rb_direct_button.Checked;
            m_aspect_setup = false;
        }
    }
}
