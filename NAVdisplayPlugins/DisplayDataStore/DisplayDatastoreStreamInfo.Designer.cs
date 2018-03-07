namespace DisplayDataStoreStreamInfo
{
    partial class DisplayDatastoreStreamInfo
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
            this.show_info = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // show_info
            // 
            this.show_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.show_info.Location = new System.Drawing.Point(13, 13);
            this.show_info.Name = "show_info";
            this.show_info.Size = new System.Drawing.Size(519, 407);
            this.show_info.TabIndex = 0;
            this.show_info.Text = "";
            // 
            // DisplayDatastoreStreamInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 432);
            this.Controls.Add(this.show_info);
            this.Name = "DisplayDatastoreStreamInfo";
            this.Text = "Display Datastore Stream Info";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox show_info;
    }
}