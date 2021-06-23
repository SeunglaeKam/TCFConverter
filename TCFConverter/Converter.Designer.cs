namespace TCFConverter
{
    partial class Converter
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
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.band_ListView = new System.Windows.Forms.ListView();
            this.checkbox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BandColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.propertygrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_Copy_RnD = new System.Windows.Forms.Button();
            this.btn_Generate_MIPI = new System.Windows.Forms.Button();
            this.btn_Insert_RnD = new System.Windows.Forms.Button();
            this.btn_Load_TCF = new System.Windows.Forms.Button();
            this.btn_Load_XML_Config = new System.Windows.Forms.Button();
            this.btn_Load_RnD = new System.Windows.Forms.Button();
            this.TCF_panel = new System.Windows.Forms.Panel();
            this.XML_Panel = new System.Windows.Forms.Panel();
            this.Split_Panel = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.textBox_TCF = new System.Windows.Forms.TextBox();
            this.TCF_label = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.textBox_Config = new System.Windows.Forms.TextBox();
            this.XML_Label = new System.Windows.Forms.Label();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.textBox_Split = new System.Windows.Forms.TextBox();
            this.Split_Label = new System.Windows.Forms.Label();
            this.btn_TCF = new System.Windows.Forms.Button();
            this.btn_Config = new System.Windows.Forms.Button();
            this.btn_Split = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.TCF_panel.SuspendLayout();
            this.XML_Panel.SuspendLayout();
            this.Split_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressbar
            // 
            this.progressbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressbar.Location = new System.Drawing.Point(0, 842);
            this.progressbar.Margin = new System.Windows.Forms.Padding(1);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(1440, 33);
            this.progressbar.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.band_ListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertygrid);
            this.splitContainer1.Size = new System.Drawing.Size(1440, 842);
            this.splitContainer1.SplitterDistance = 712;
            this.splitContainer1.TabIndex = 19;
            // 
            // band_ListView
            // 
            this.band_ListView.AllowDrop = true;
            this.band_ListView.CheckBoxes = true;
            this.band_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.checkbox,
            this.BandColumn});
            this.band_ListView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.band_ListView.HideSelection = false;
            this.band_ListView.Location = new System.Drawing.Point(0, 379);
            this.band_ListView.Name = "band_ListView";
            this.band_ListView.OwnerDraw = true;
            this.band_ListView.Size = new System.Drawing.Size(712, 463);
            this.band_ListView.TabIndex = 21;
            this.band_ListView.UseCompatibleStateImageBehavior = false;
            this.band_ListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.band_ListView_ColumnClick);
            this.band_ListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.band_ListView_DrawColumnHeader);
            this.band_ListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.band_ListView_DrawItem);
            this.band_ListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.band_ListView_DrawSubItem);
            // 
            // checkbox
            // 
            this.checkbox.Tag = "Band";
            this.checkbox.Text = "";
            this.checkbox.Width = 30;
            // 
            // BandColumn
            // 
            this.BandColumn.Text = "Band";
            // 
            // propertygrid
            // 
            this.propertygrid.AllowDrop = true;
            this.propertygrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertygrid.Location = new System.Drawing.Point(0, 0);
            this.propertygrid.Name = "propertygrid";
            this.propertygrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertygrid.Size = new System.Drawing.Size(724, 842);
            this.propertygrid.TabIndex = 20;
            this.propertygrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Prop_Value_Changed);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.Split_Panel);
            this.splitContainer2.Panel1.Controls.Add(this.XML_Panel);
            this.splitContainer2.Panel1.Controls.Add(this.TCF_panel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btn_Convert);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Copy_RnD);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Generate_MIPI);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Insert_RnD);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Load_TCF);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Load_XML_Config);
            this.splitContainer2.Panel2.Controls.Add(this.btn_Load_RnD);
            this.splitContainer2.Size = new System.Drawing.Size(712, 379);
            this.splitContainer2.SplitterDistance = 212;
            this.splitContainer2.TabIndex = 24;
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(342, 82);
            this.btn_Convert.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(126, 57);
            this.btn_Convert.TabIndex = 24;
            this.btn_Convert.Text = "Split TCF File";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Split_Click);
            // 
            // btn_Copy_RnD
            // 
            this.btn_Copy_RnD.Location = new System.Drawing.Point(472, 21);
            this.btn_Copy_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Copy_RnD.Name = "btn_Copy_RnD";
            this.btn_Copy_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Copy_RnD.TabIndex = 23;
            this.btn_Copy_RnD.Text = "Copy RnD File";
            this.btn_Copy_RnD.UseVisualStyleBackColor = true;
            this.btn_Copy_RnD.Click += new System.EventHandler(this.btn_Copy_RnD_Click);
            // 
            // btn_Generate_MIPI
            // 
            this.btn_Generate_MIPI.Location = new System.Drawing.Point(342, 21);
            this.btn_Generate_MIPI.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Generate_MIPI.Name = "btn_Generate_MIPI";
            this.btn_Generate_MIPI.Size = new System.Drawing.Size(126, 57);
            this.btn_Generate_MIPI.TabIndex = 22;
            this.btn_Generate_MIPI.Text = "Generate MIPI Command";
            this.btn_Generate_MIPI.UseVisualStyleBackColor = true;
            this.btn_Generate_MIPI.Click += new System.EventHandler(this.btn_Generate_MIPI_Click);
            // 
            // btn_Insert_RnD
            // 
            this.btn_Insert_RnD.Location = new System.Drawing.Point(213, 82);
            this.btn_Insert_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Insert_RnD.Name = "btn_Insert_RnD";
            this.btn_Insert_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Insert_RnD.TabIndex = 21;
            this.btn_Insert_RnD.Text = "Insert RnD File";
            this.btn_Insert_RnD.UseVisualStyleBackColor = true;
            this.btn_Insert_RnD.Click += new System.EventHandler(this.btn_Insert_RnD_Click);
            // 
            // btn_Load_TCF
            // 
            this.btn_Load_TCF.Location = new System.Drawing.Point(83, 82);
            this.btn_Load_TCF.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_TCF.Name = "btn_Load_TCF";
            this.btn_Load_TCF.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_TCF.TabIndex = 20;
            this.btn_Load_TCF.Text = "Load TCF File";
            this.btn_Load_TCF.UseVisualStyleBackColor = true;
            this.btn_Load_TCF.Click += new System.EventHandler(this.btn_Load_TCF_Click);
            // 
            // btn_Load_XML_Config
            // 
            this.btn_Load_XML_Config.Location = new System.Drawing.Point(213, 21);
            this.btn_Load_XML_Config.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_XML_Config.Name = "btn_Load_XML_Config";
            this.btn_Load_XML_Config.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_XML_Config.TabIndex = 19;
            this.btn_Load_XML_Config.Text = "Load Config XML File";
            this.btn_Load_XML_Config.UseVisualStyleBackColor = true;
            this.btn_Load_XML_Config.Click += new System.EventHandler(this.btn_Load_XML_Config_Click);
            // 
            // btn_Load_RnD
            // 
            this.btn_Load_RnD.Location = new System.Drawing.Point(83, 21);
            this.btn_Load_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_RnD.Name = "btn_Load_RnD";
            this.btn_Load_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_RnD.TabIndex = 18;
            this.btn_Load_RnD.Text = "Merge RnD File";
            this.btn_Load_RnD.UseVisualStyleBackColor = true;
            this.btn_Load_RnD.Click += new System.EventHandler(this.btn_Merge_RnD_Click);
            // 
            // TCF_panel
            // 
            this.TCF_panel.Controls.Add(this.splitContainer3);
            this.TCF_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TCF_panel.Location = new System.Drawing.Point(0, 0);
            this.TCF_panel.Name = "TCF_panel";
            this.TCF_panel.Size = new System.Drawing.Size(712, 70);
            this.TCF_panel.TabIndex = 26;
            // 
            // XML_Panel
            // 
            this.XML_Panel.Controls.Add(this.splitContainer4);
            this.XML_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.XML_Panel.Location = new System.Drawing.Point(0, 70);
            this.XML_Panel.Name = "XML_Panel";
            this.XML_Panel.Size = new System.Drawing.Size(712, 70);
            this.XML_Panel.TabIndex = 27;
            // 
            // Split_Panel
            // 
            this.Split_Panel.Controls.Add(this.splitContainer5);
            this.Split_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Split_Panel.Location = new System.Drawing.Point(0, 140);
            this.Split_Panel.Name = "Split_Panel";
            this.Split_Panel.Size = new System.Drawing.Size(712, 70);
            this.Split_Panel.TabIndex = 28;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.TCF_label);
            this.splitContainer3.Panel1.Controls.Add(this.textBox_TCF);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.btn_TCF);
            this.splitContainer3.Size = new System.Drawing.Size(712, 70);
            this.splitContainer3.SplitterDistance = 658;
            this.splitContainer3.TabIndex = 27;
            // 
            // textBox_TCF
            // 
            this.textBox_TCF.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_TCF.Location = new System.Drawing.Point(10, 50);
            this.textBox_TCF.Name = "textBox_TCF";
            this.textBox_TCF.ReadOnly = true;
            this.textBox_TCF.Size = new System.Drawing.Size(648, 20);
            this.textBox_TCF.TabIndex = 27;
            // 
            // TCF_label
            // 
            this.TCF_label.AutoSize = true;
            this.TCF_label.Location = new System.Drawing.Point(22, 19);
            this.TCF_label.Name = "TCF_label";
            this.TCF_label.Size = new System.Drawing.Size(71, 13);
            this.TCF_label.TabIndex = 28;
            this.TCF_label.Text = "TCF File Path";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.XML_Label);
            this.splitContainer4.Panel1.Controls.Add(this.textBox_Config);
            this.splitContainer4.Panel1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.btn_Config);
            this.splitContainer4.Size = new System.Drawing.Size(712, 70);
            this.splitContainer4.SplitterDistance = 658;
            this.splitContainer4.TabIndex = 27;
            // 
            // textBox_Config
            // 
            this.textBox_Config.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_Config.Location = new System.Drawing.Point(10, 50);
            this.textBox_Config.Name = "textBox_Config";
            this.textBox_Config.ReadOnly = true;
            this.textBox_Config.Size = new System.Drawing.Size(648, 20);
            this.textBox_Config.TabIndex = 27;
            // 
            // XML_Label
            // 
            this.XML_Label.AutoSize = true;
            this.XML_Label.Location = new System.Drawing.Point(22, 21);
            this.XML_Label.Name = "XML_Label";
            this.XML_Label.Size = new System.Drawing.Size(81, 13);
            this.XML_Label.TabIndex = 28;
            this.XML_Label.Text = "Config File Path";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.Split_Label);
            this.splitContainer5.Panel1.Controls.Add(this.textBox_Split);
            this.splitContainer5.Panel1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.btn_Split);
            this.splitContainer5.Size = new System.Drawing.Size(712, 70);
            this.splitContainer5.SplitterDistance = 657;
            this.splitContainer5.TabIndex = 27;
            // 
            // textBox_Split
            // 
            this.textBox_Split.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_Split.Location = new System.Drawing.Point(10, 50);
            this.textBox_Split.Name = "textBox_Split";
            this.textBox_Split.ReadOnly = true;
            this.textBox_Split.Size = new System.Drawing.Size(647, 20);
            this.textBox_Split.TabIndex = 27;
            // 
            // Split_Label
            // 
            this.Split_Label.AutoSize = true;
            this.Split_Label.Location = new System.Drawing.Point(22, 19);
            this.Split_Label.Name = "Split_Label";
            this.Split_Label.Size = new System.Drawing.Size(96, 13);
            this.Split_Label.TabIndex = 28;
            this.Split_Label.Text = "Splited Folder Path";
            // 
            // btn_TCF
            // 
            this.btn_TCF.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_TCF.Location = new System.Drawing.Point(0, 47);
            this.btn_TCF.Name = "btn_TCF";
            this.btn_TCF.Size = new System.Drawing.Size(50, 23);
            this.btn_TCF.TabIndex = 0;
            this.btn_TCF.Text = "Go";
            this.btn_TCF.UseVisualStyleBackColor = true;
            this.btn_TCF.Click += new System.EventHandler(this.btn_TCF_Click);
            // 
            // btn_Config
            // 
            this.btn_Config.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Config.Location = new System.Drawing.Point(0, 47);
            this.btn_Config.Name = "btn_Config";
            this.btn_Config.Size = new System.Drawing.Size(50, 23);
            this.btn_Config.TabIndex = 1;
            this.btn_Config.Text = "Go";
            this.btn_Config.UseVisualStyleBackColor = true;
            this.btn_Config.Click += new System.EventHandler(this.btn_Config_Click);
            // 
            // btn_Split
            // 
            this.btn_Split.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Split.Location = new System.Drawing.Point(0, 47);
            this.btn_Split.Name = "btn_Split";
            this.btn_Split.Size = new System.Drawing.Size(51, 23);
            this.btn_Split.TabIndex = 1;
            this.btn_Split.Text = "Go";
            this.btn_Split.UseVisualStyleBackColor = true;
            this.btn_Split.Click += new System.EventHandler(this.btn_Split_Click_1);
            // 
            // Converter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 875);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.progressbar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Converter";
            this.Text = "TCF Converter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Converter_FormClosed);
            this.Load += new System.EventHandler(this.Converter_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.TCF_panel.ResumeLayout(false);
            this.XML_Panel.ResumeLayout(false);
            this.Split_Panel.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView band_ListView;
        private System.Windows.Forms.ColumnHeader checkbox;
        private System.Windows.Forms.ColumnHeader BandColumn;
        public System.Windows.Forms.PropertyGrid propertygrid;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_Copy_RnD;
        private System.Windows.Forms.Button btn_Generate_MIPI;
        private System.Windows.Forms.Button btn_Insert_RnD;
        private System.Windows.Forms.Button btn_Load_TCF;
        private System.Windows.Forms.Button btn_Load_XML_Config;
        private System.Windows.Forms.Button btn_Load_RnD;
        private System.Windows.Forms.Panel TCF_panel;
        private System.Windows.Forms.Panel Split_Panel;
        private System.Windows.Forms.Panel XML_Panel;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Label Split_Label;
        private System.Windows.Forms.TextBox textBox_Split;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Label XML_Label;
        private System.Windows.Forms.TextBox textBox_Config;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label TCF_label;
        private System.Windows.Forms.TextBox textBox_TCF;
        private System.Windows.Forms.Button btn_Split;
        private System.Windows.Forms.Button btn_Config;
        private System.Windows.Forms.Button btn_TCF;
    }
}

