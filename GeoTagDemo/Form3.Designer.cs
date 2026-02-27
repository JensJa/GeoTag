namespace GeoTagDemo
{
    partial class Form3
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
            button1 = new Button();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            button2 = new Button();
            button3 = new Button();
            lb_latitude = new Label();
            lb_longitude = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            tb_latitude = new TextBox();
            tb_Longitude = new TextBox();
            tb_url = new TextBox();
            tb_url_google = new TextBox();
            webViewGoogle = new Microsoft.Web.WebView2.WinForms.WebView2();
            button4 = new Button();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)webViewGoogle).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(36, 28);
            button1.Name = "button1";
            button1.Size = new Size(99, 43);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Location = new Point(36, 203);
            webView.Name = "webView";
            webView.Size = new Size(1074, 1112);
            webView.TabIndex = 1;
            webView.ZoomFactor = 1D;
            webView.SourceChanged += webView_SourceChanged;
            // 
            // button2
            // 
            button2.Location = new Point(141, 29);
            button2.Name = "button2";
            button2.Size = new Size(87, 42);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(234, 28);
            button3.Name = "button3";
            button3.Size = new Size(87, 42);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // lb_latitude
            // 
            lb_latitude.AutoSize = true;
            lb_latitude.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_latitude.Location = new Point(375, 35);
            lb_latitude.Name = "lb_latitude";
            lb_latitude.Size = new Size(63, 25);
            lb_latitude.TabIndex = 4;
            lb_latitude.Text = "label1";
            // 
            // lb_longitude
            // 
            lb_longitude.AutoSize = true;
            lb_longitude.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_longitude.Location = new Point(663, 35);
            lb_longitude.Name = "lb_longitude";
            lb_longitude.Size = new Size(63, 25);
            lb_longitude.TabIndex = 5;
            lb_longitude.Text = "label2";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(1057, 28);
            label5.Name = "label5";
            label5.Size = new Size(163, 21);
            label5.TabIndex = 28;
            label5.Text = "52.074900,12.951724";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(1011, 64);
            label4.Name = "label4";
            label4.Size = new Size(167, 21);
            label4.TabIndex = 27;
            label4.Text = "52.074869, 12.951680";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(952, 92);
            label3.Name = "label3";
            label3.Size = new Size(158, 21);
            label3.TabIndex = 26;
            label3.Text = "52.07486, 12.951643";
            // 
            // tb_latitude
            // 
            tb_latitude.Location = new Point(375, 66);
            tb_latitude.Name = "tb_latitude";
            tb_latitude.Size = new Size(154, 23);
            tb_latitude.TabIndex = 29;
            tb_latitude.Text = "52.233913";
            tb_latitude.TextChanged += tb_latitude_TextChanged;
            // 
            // tb_Longitude
            // 
            tb_Longitude.Location = new Point(663, 62);
            tb_Longitude.Name = "tb_Longitude";
            tb_Longitude.Size = new Size(153, 23);
            tb_Longitude.TabIndex = 30;
            tb_Longitude.Text = "13.129894";
            // 
            // tb_url
            // 
            tb_url.Location = new Point(45, 103);
            tb_url.Name = "tb_url";
            tb_url.Size = new Size(856, 23);
            tb_url.TabIndex = 31;
            tb_url.TextChanged += tb_url_TextChanged;
            // 
            // tb_url_google
            // 
            tb_url_google.Location = new Point(45, 149);
            tb_url_google.Name = "tb_url_google";
            tb_url_google.Size = new Size(856, 23);
            tb_url_google.TabIndex = 32;
            // 
            // webViewGoogle
            // 
            webViewGoogle.AllowExternalDrop = true;
            webViewGoogle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webViewGoogle.CreationProperties = null;
            webViewGoogle.DefaultBackgroundColor = Color.White;
            webViewGoogle.Location = new Point(1154, 203);
            webViewGoogle.Name = "webViewGoogle";
            webViewGoogle.Size = new Size(1177, 1112);
            webViewGoogle.TabIndex = 33;
            webViewGoogle.ZoomFactor = 1D;
            // 
            // button4
            // 
            button4.Location = new Point(1176, 103);
            button4.Name = "button4";
            button4.Size = new Size(124, 69);
            button4.TabIndex = 34;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1306, 103);
            button5.Name = "button5";
            button5.Size = new Size(124, 69);
            button5.TabIndex = 35;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2401, 1351);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(webViewGoogle);
            Controls.Add(tb_url_google);
            Controls.Add(tb_url);
            Controls.Add(tb_Longitude);
            Controls.Add(tb_latitude);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(lb_longitude);
            Controls.Add(lb_latitude);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(webView);
            Controls.Add(button1);
            Name = "Form3";
            Text = "Form3";
            Load += Form3_Load;
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ((System.ComponentModel.ISupportInitialize)webViewGoogle).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private Button button2;
        private Button button3;
        private Label lb_latitude;
        private Label lb_longitude;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox tb_latitude;
        private TextBox tb_Longitude;
        private TextBox tb_url;
        private TextBox tb_url_google;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewGoogle;
        private Button button4;
        private Button button5;
    }
}