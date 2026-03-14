namespace ExifEditDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_filename = new TextBox();
            but_work = new Button();
            lb_exifdata = new ListBox();
            lb_S = new ListBox();
            SuspendLayout();
            // 
            // tb_filename
            // 
            tb_filename.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_filename.Location = new Point(47, 24);
            tb_filename.Name = "tb_filename";
            tb_filename.Size = new Size(1133, 23);
            tb_filename.TabIndex = 0;
            // 
            // but_work
            // 
            but_work.Location = new Point(47, 70);
            but_work.Name = "but_work";
            but_work.Size = new Size(75, 23);
            but_work.TabIndex = 1;
            but_work.Text = "Worrk";
            but_work.UseVisualStyleBackColor = true;
            but_work.Click += but_work_Click;
            // 
            // lb_exifdata
            // 
            lb_exifdata.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lb_exifdata.FormattingEnabled = true;
            lb_exifdata.Location = new Point(406, 128);
            lb_exifdata.Name = "lb_exifdata";
            lb_exifdata.Size = new Size(773, 544);
            lb_exifdata.TabIndex = 2;
            // 
            // lb_S
            // 
            lb_S.FormattingEnabled = true;
            lb_S.Location = new Point(25, 130);
            lb_S.Name = "lb_S";
            lb_S.Size = new Size(362, 529);
            lb_S.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1191, 695);
            Controls.Add(lb_S);
            Controls.Add(lb_exifdata);
            Controls.Add(but_work);
            Controls.Add(tb_filename);
            Name = "Form1";
            Text = "Form1";
            DragDrop += FormMain_DragDrop;
            DragEnter += FormMain_DragEnter;
            DragOver += FormMain_DragOver;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_filename;
        private Button but_work;
        private ListBox lb_exifdata;
        private ListBox lb_S;
    }
}
