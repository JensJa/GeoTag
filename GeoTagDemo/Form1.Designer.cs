namespace GeoTagDemo
{
    partial class Form1
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
            mapCtrl1 = new GeoTag.mapCtrl();
            tb_latitude = new TextBox();
            tb_longitude = new TextBox();
            gb_mapProvider = new GroupBox();
            rb_google = new RadioButton();
            rb_osm = new RadioButton();
            textBox1 = new TextBox();
            button1 = new Button();
            gb_mapProvider.SuspendLayout();
            SuspendLayout();
            // 
            // mapCtrl1
            // 
            mapCtrl1.activeMapProvider = GeoTag.MapProvider.OpenStreetMap;
            mapCtrl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mapCtrl1.latitude = 0D;
            mapCtrl1.Location = new Point(130, 145);
            mapCtrl1.longitude = 0D;
            mapCtrl1.Name = "mapCtrl1";
            mapCtrl1.Size = new Size(909, 642);
            mapCtrl1.TabIndex = 0;
            // 
            // tb_latitude
            // 
            tb_latitude.Location = new Point(130, 35);
            tb_latitude.Name = "tb_latitude";
            tb_latitude.Size = new Size(140, 23);
            tb_latitude.TabIndex = 1;
            // 
            // tb_longitude
            // 
            tb_longitude.Location = new Point(285, 35);
            tb_longitude.Name = "tb_longitude";
            tb_longitude.Size = new Size(140, 23);
            tb_longitude.TabIndex = 2;
            // 
            // gb_mapProvider
            // 
            gb_mapProvider.Controls.Add(rb_google);
            gb_mapProvider.Controls.Add(rb_osm);
            gb_mapProvider.Location = new Point(844, 20);
            gb_mapProvider.Name = "gb_mapProvider";
            gb_mapProvider.Size = new Size(153, 86);
            gb_mapProvider.TabIndex = 3;
            gb_mapProvider.TabStop = false;
            gb_mapProvider.Text = "Map Provider";
            // 
            // rb_google
            // 
            rb_google.AutoSize = true;
            rb_google.Location = new Point(23, 54);
            rb_google.Name = "rb_google";
            rb_google.Size = new Size(92, 19);
            rb_google.TabIndex = 1;
            rb_google.Text = "GoogleMaps";
            rb_google.UseVisualStyleBackColor = true;
            // 
            // rb_osm
            // 
            rb_osm.AutoSize = true;
            rb_osm.Checked = true;
            rb_osm.Location = new Point(23, 29);
            rb_osm.Name = "rb_osm";
            rb_osm.Size = new Size(108, 19);
            rb_osm.TabIndex = 0;
            rb_osm.TabStop = true;
            rb_osm.Text = "OpenStreetmap";
            rb_osm.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(130, 83);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(684, 23);
            textBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(1032, 33);
            button1.Name = "button1";
            button1.Size = new Size(137, 40);
            button1.TabIndex = 5;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1226, 859);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(gb_mapProvider);
            Controls.Add(tb_longitude);
            Controls.Add(tb_latitude);
            Controls.Add(mapCtrl1);
            Name = "Form1";
            Text = "Form1";
            gb_mapProvider.ResumeLayout(false);
            gb_mapProvider.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GeoTag.mapCtrl mapCtrl1;
        private TextBox tb_latitude;
        private TextBox tb_longitude;
        private GroupBox gb_mapProvider;
        private RadioButton rb_google;
        private RadioButton rb_osm;
        private TextBox textBox1;
        private Button button1;
    }
}