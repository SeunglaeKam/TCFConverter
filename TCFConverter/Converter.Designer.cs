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
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.band_ListView = new System.Windows.Forms.ListView();
            this.checkbox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BandColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_Copy_RnD = new System.Windows.Forms.Button();
            this.btn_Generate_MIPI = new System.Windows.Forms.Button();
            this.btn_Insert_RnD = new System.Windows.Forms.Button();
            this.btn_Load_TCF = new System.Windows.Forms.Button();
            this.btn_Load_XML_Config = new System.Windows.Forms.Button();
            this.btn_Load_RnD = new System.Windows.Forms.Button();
            this.propertygrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.label);
            this.splitContainer1.Panel1.Controls.Add(this.textBox);
            this.splitContainer1.Panel1.Controls.Add(this.band_ListView);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Convert);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Copy_RnD);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Generate_MIPI);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Insert_RnD);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Load_TCF);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Load_XML_Config);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Load_RnD);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertygrid);
            this.splitContainer1.Size = new System.Drawing.Size(1440, 875);
            this.splitContainer1.SplitterDistance = 712;
            this.splitContainer1.TabIndex = 18;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(52, 13);
            this.label.TabIndex = 23;
            this.label.Text = "TCF Path";
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(0, 25);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(712, 20);
            this.textBox.TabIndex = 22;
            // 
            // band_ListView
            // 
            this.band_ListView.AllowDrop = true;
            this.band_ListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.band_ListView.CheckBoxes = true;
            this.band_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.checkbox,
            this.BandColumn});
            this.band_ListView.HideSelection = false;
            this.band_ListView.Location = new System.Drawing.Point(0, 379);
            this.band_ListView.Name = "band_ListView";
            this.band_ListView.OwnerDraw = true;
            this.band_ListView.Size = new System.Drawing.Size(712, 496);
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
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(337, 135);
            this.btn_Convert.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(126, 57);
            this.btn_Convert.TabIndex = 17;
            this.btn_Convert.Text = "Split TCF File";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Split_Click);
            // 
            // btn_Copy_RnD
            // 
            this.btn_Copy_RnD.Location = new System.Drawing.Point(337, 61);
            this.btn_Copy_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Copy_RnD.Name = "btn_Copy_RnD";
            this.btn_Copy_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Copy_RnD.TabIndex = 16;
            this.btn_Copy_RnD.Text = "Copy RnD File";
            this.btn_Copy_RnD.UseVisualStyleBackColor = true;
            this.btn_Copy_RnD.Click += new System.EventHandler(this.btn_Copy_RnD_Click);
            // 
            // btn_Generate_MIPI
            // 
            this.btn_Generate_MIPI.Location = new System.Drawing.Point(193, 61);
            this.btn_Generate_MIPI.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Generate_MIPI.Name = "btn_Generate_MIPI";
            this.btn_Generate_MIPI.Size = new System.Drawing.Size(126, 57);
            this.btn_Generate_MIPI.TabIndex = 15;
            this.btn_Generate_MIPI.Text = "Generate MIPI Command";
            this.btn_Generate_MIPI.UseVisualStyleBackColor = true;
            this.btn_Generate_MIPI.Click += new System.EventHandler(this.btn_Generate_MIPI_Click);
            // 
            // btn_Insert_RnD
            // 
            this.btn_Insert_RnD.Location = new System.Drawing.Point(193, 135);
            this.btn_Insert_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Insert_RnD.Name = "btn_Insert_RnD";
            this.btn_Insert_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Insert_RnD.TabIndex = 14;
            this.btn_Insert_RnD.Text = "Insert RnD File";
            this.btn_Insert_RnD.UseVisualStyleBackColor = true;
            this.btn_Insert_RnD.Click += new System.EventHandler(this.btn_Insert_RnD_Click);
            // 
            // btn_Load_TCF
            // 
            this.btn_Load_TCF.Location = new System.Drawing.Point(47, 207);
            this.btn_Load_TCF.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_TCF.Name = "btn_Load_TCF";
            this.btn_Load_TCF.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_TCF.TabIndex = 6;
            this.btn_Load_TCF.Text = "Load TCF File";
            this.btn_Load_TCF.UseVisualStyleBackColor = true;
            this.btn_Load_TCF.Click += new System.EventHandler(this.btn_Load_TCF_Click);
            // 
            // btn_Load_XML_Config
            // 
            this.btn_Load_XML_Config.Location = new System.Drawing.Point(47, 135);
            this.btn_Load_XML_Config.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_XML_Config.Name = "btn_Load_XML_Config";
            this.btn_Load_XML_Config.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_XML_Config.TabIndex = 5;
            this.btn_Load_XML_Config.Text = "Load Config XML File";
            this.btn_Load_XML_Config.UseVisualStyleBackColor = true;
            this.btn_Load_XML_Config.Click += new System.EventHandler(this.btn_Load_XML_Config_Click);
            // 
            // btn_Load_RnD
            // 
            this.btn_Load_RnD.Location = new System.Drawing.Point(47, 61);
            this.btn_Load_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_RnD.Name = "btn_Load_RnD";
            this.btn_Load_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_RnD.TabIndex = 1;
            this.btn_Load_RnD.Text = "Merge RnD File";
            this.btn_Load_RnD.UseVisualStyleBackColor = true;
            this.btn_Load_RnD.Click += new System.EventHandler(this.btn_Merge_RnD_Click);
            // 
            // propertygrid
            // 
            this.propertygrid.AllowDrop = true;
            this.propertygrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertygrid.Location = new System.Drawing.Point(0, 0);
            this.propertygrid.Name = "propertygrid";
            this.propertygrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertygrid.Size = new System.Drawing.Size(724, 875);
            this.propertygrid.TabIndex = 20;
            this.propertygrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Prop_Value_Changed);
            // 
            // Converter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 875);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Converter";
            this.Text = "TCF Converter";            
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Converter_FormClosed);
            this.Load += new System.EventHandler(this.Converter_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.ListView band_ListView;
        private System.Windows.Forms.ColumnHeader checkbox;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_Copy_RnD;
        private System.Windows.Forms.Button btn_Generate_MIPI;
        private System.Windows.Forms.Button btn_Insert_RnD;
        private System.Windows.Forms.Button btn_Load_TCF;
        private System.Windows.Forms.Button btn_Load_XML_Config;
        private System.Windows.Forms.Button btn_Load_RnD;
        public System.Windows.Forms.PropertyGrid propertygrid;
        private System.Windows.Forms.ColumnHeader BandColumn;
        private System.Windows.Forms.Label label;
    }
}

