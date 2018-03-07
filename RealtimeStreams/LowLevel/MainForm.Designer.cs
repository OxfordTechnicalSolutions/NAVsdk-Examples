namespace CLRDatastoreExample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_ui_update_timer = new System.Windows.Forms.Timer(this.components);
            this.m_decode_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_navigation = new System.Windows.Forms.CheckBox();
            this.cb_resource = new System.Windows.Forms.CheckBox();
            this.cb_interface = new System.Windows.Forms.CheckBox();
            this.cb_basic = new System.Windows.Forms.CheckBox();
            this.m_gv_stream_information = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rb_retain_button = new System.Windows.Forms.RadioButton();
            this.m_rb_direct_button = new System.Windows.Forms.RadioButton();
            this.m_tv_device_mapping = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gv_stream_information)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ui_update_timer
            // 
            this.m_ui_update_timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cb_navigation);
            this.groupBox1.Controls.Add(this.cb_resource);
            this.groupBox1.Controls.Add(this.cb_interface);
            this.groupBox1.Controls.Add(this.cb_basic);
            this.groupBox1.Location = new System.Drawing.Point(12, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 398);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fields";
            // 
            // cb_navigation
            // 
            this.cb_navigation.AutoSize = true;
            this.cb_navigation.Location = new System.Drawing.Point(7, 82);
            this.cb_navigation.Name = "cb_navigation";
            this.cb_navigation.Size = new System.Drawing.Size(77, 17);
            this.cb_navigation.TabIndex = 0;
            this.cb_navigation.Text = "Navigation";
            this.cb_navigation.UseVisualStyleBackColor = true;
            this.cb_navigation.CheckedChanged += new System.EventHandler(this.cb_navigation_CheckedChanged);
            // 
            // cb_resource
            // 
            this.cb_resource.AutoSize = true;
            this.cb_resource.Location = new System.Drawing.Point(7, 59);
            this.cb_resource.Name = "cb_resource";
            this.cb_resource.Size = new System.Drawing.Size(72, 17);
            this.cb_resource.TabIndex = 0;
            this.cb_resource.Text = "Resource";
            this.cb_resource.UseVisualStyleBackColor = true;
            this.cb_resource.CheckedChanged += new System.EventHandler(this.cb_resource_CheckedChanged);
            // 
            // cb_interface
            // 
            this.cb_interface.AutoSize = true;
            this.cb_interface.Location = new System.Drawing.Point(7, 36);
            this.cb_interface.Name = "cb_interface";
            this.cb_interface.Size = new System.Drawing.Size(68, 17);
            this.cb_interface.TabIndex = 0;
            this.cb_interface.Text = "Interface";
            this.cb_interface.UseVisualStyleBackColor = true;
            this.cb_interface.CheckedChanged += new System.EventHandler(this.cb_interface_CheckedChanged);
            // 
            // cb_basic
            // 
            this.cb_basic.AutoSize = true;
            this.cb_basic.Checked = true;
            this.cb_basic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_basic.Location = new System.Drawing.Point(7, 13);
            this.cb_basic.Name = "cb_basic";
            this.cb_basic.Size = new System.Drawing.Size(52, 17);
            this.cb_basic.TabIndex = 0;
            this.cb_basic.Text = "Basic";
            this.cb_basic.UseVisualStyleBackColor = true;
            this.cb_basic.CheckedChanged += new System.EventHandler(this.cb_basic_CheckedChanged);
            // 
            // m_gv_stream_information
            // 
            this.m_gv_stream_information.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gv_stream_information.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_gv_stream_information.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gv_stream_information.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_gv_stream_information.Enabled = false;
            this.m_gv_stream_information.Location = new System.Drawing.Point(561, 12);
            this.m_gv_stream_information.Name = "m_gv_stream_information";
            this.m_gv_stream_information.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.m_gv_stream_information.Size = new System.Drawing.Size(381, 450);
            this.m_gv_stream_information.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_rb_retain_button);
            this.groupBox2.Controls.Add(this.m_rb_direct_button);
            this.groupBox2.Location = new System.Drawing.Point(12, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(186, 73);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // m_rb_retain_button
            // 
            this.m_rb_retain_button.AutoSize = true;
            this.m_rb_retain_button.Location = new System.Drawing.Point(7, 43);
            this.m_rb_retain_button.Name = "m_rb_retain_button";
            this.m_rb_retain_button.Size = new System.Drawing.Size(56, 17);
            this.m_rb_retain_button.TabIndex = 0;
            this.m_rb_retain_button.Text = "Retain";
            this.m_rb_retain_button.UseVisualStyleBackColor = true;
            this.m_rb_retain_button.CheckedChanged += new System.EventHandler(this.m_rb_retain_button_CheckedChanged);
            // 
            // m_rb_direct_button
            // 
            this.m_rb_direct_button.AutoSize = true;
            this.m_rb_direct_button.Checked = true;
            this.m_rb_direct_button.Location = new System.Drawing.Point(7, 20);
            this.m_rb_direct_button.Name = "m_rb_direct_button";
            this.m_rb_direct_button.Size = new System.Drawing.Size(53, 17);
            this.m_rb_direct_button.TabIndex = 0;
            this.m_rb_direct_button.TabStop = true;
            this.m_rb_direct_button.Text = "Direct";
            this.m_rb_direct_button.UseVisualStyleBackColor = true;
            this.m_rb_direct_button.CheckedChanged += new System.EventHandler(this.m_rb_direct_button_CheckedChanged);
            // 
            // m_tv_device_mapping
            // 
            this.m_tv_device_mapping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_tv_device_mapping.Location = new System.Drawing.Point(204, 12);
            this.m_tv_device_mapping.Name = "m_tv_device_mapping";
            this.m_tv_device_mapping.Size = new System.Drawing.Size(351, 450);
            this.m_tv_device_mapping.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(954, 474);
            this.Controls.Add(this.m_tv_device_mapping);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_gv_stream_information);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "CLRDatastore Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gv_stream_information)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer m_ui_update_timer;
        private System.Windows.Forms.Timer m_decode_timer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_navigation;
        private System.Windows.Forms.CheckBox cb_resource;
        private System.Windows.Forms.CheckBox cb_interface;
        private System.Windows.Forms.CheckBox cb_basic;
        private System.Windows.Forms.DataGridView m_gv_stream_information;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton m_rb_retain_button;
        private System.Windows.Forms.RadioButton m_rb_direct_button;
        private System.Windows.Forms.TreeView m_tv_device_mapping;
    }
}

