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
            this.btn_Load_RnD = new System.Windows.Forms.Button();
            this.btn_Load_TCF = new System.Windows.Forms.Button();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_Copy_RnD = new System.Windows.Forms.Button();
            this.btn_Load_XML_Config = new System.Windows.Forms.Button();
            this.btn_Generate_MIPI = new System.Windows.Forms.Button();
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.btn_Load_Selected_RnD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Load_RnD
            // 
            this.btn_Load_RnD.Location = new System.Drawing.Point(310, 21);
            this.btn_Load_RnD.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Load_RnD.Name = "btn_Load_RnD";
            this.btn_Load_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_RnD.TabIndex = 0;
            this.btn_Load_RnD.Text = "Merge RnD File";
            this.btn_Load_RnD.UseVisualStyleBackColor = true;
            this.btn_Load_RnD.Click += new System.EventHandler(this.btn_Merge_RnD_Click);
            // 
            // btn_Load_TCF
            // 
            this.btn_Load_TCF.Location = new System.Drawing.Point(454, 21);
            this.btn_Load_TCF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Load_TCF.Name = "btn_Load_TCF";
            this.btn_Load_TCF.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_TCF.TabIndex = 1;
            this.btn_Load_TCF.Text = "Load TCF File";
            this.btn_Load_TCF.UseVisualStyleBackColor = true;
            this.btn_Load_TCF.Click += new System.EventHandler(this.btn_Load_TCF_Click);
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(454, 103);
            this.btn_Convert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(126, 57);
            this.btn_Convert.TabIndex = 2;
            this.btn_Convert.Text = "Split TCF File";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Split_Click);
            // 
            // btn_Copy_RnD
            // 
            this.btn_Copy_RnD.Location = new System.Drawing.Point(163, 21);
            this.btn_Copy_RnD.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Copy_RnD.Name = "btn_Copy_RnD";
            this.btn_Copy_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Copy_RnD.TabIndex = 3;
            this.btn_Copy_RnD.Text = "Copy All RnD File";
            this.btn_Copy_RnD.UseVisualStyleBackColor = true;
            this.btn_Copy_RnD.Click += new System.EventHandler(this.btn_Copy_RnD_Click);
            // 
            // btn_Load_XML_Config
            // 
            this.btn_Load_XML_Config.Location = new System.Drawing.Point(163, 103);
            this.btn_Load_XML_Config.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Load_XML_Config.Name = "btn_Load_XML_Config";
            this.btn_Load_XML_Config.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_XML_Config.TabIndex = 4;
            this.btn_Load_XML_Config.Text = "Load Config XML File";
            this.btn_Load_XML_Config.UseVisualStyleBackColor = true;
            this.btn_Load_XML_Config.Click += new System.EventHandler(this.btn_Load_XML_Config_Click);
            // 
            // btn_Generate_MIPI
            // 
            this.btn_Generate_MIPI.Location = new System.Drawing.Point(310, 103);
            this.btn_Generate_MIPI.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Generate_MIPI.Name = "btn_Generate_MIPI";
            this.btn_Generate_MIPI.Size = new System.Drawing.Size(126, 57);
            this.btn_Generate_MIPI.TabIndex = 5;
            this.btn_Generate_MIPI.Text = "Generate MIPI Command";
            this.btn_Generate_MIPI.UseVisualStyleBackColor = true;
            this.btn_Generate_MIPI.Click += new System.EventHandler(this.btn_Generate_MIPI_Click);
            // 
            // progressbar
            // 
            this.progressbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressbar.Location = new System.Drawing.Point(0, 183);
            this.progressbar.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(659, 33);
            this.progressbar.TabIndex = 7;
            // 
            // btn_Load_Selected_RnD
            // 
            this.btn_Load_Selected_RnD.Location = new System.Drawing.Point(16, 21);
            this.btn_Load_Selected_RnD.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Load_Selected_RnD.Name = "btn_Load_Selected_RnD";
            this.btn_Load_Selected_RnD.Size = new System.Drawing.Size(126, 57);
            this.btn_Load_Selected_RnD.TabIndex = 8;
            this.btn_Load_Selected_RnD.Text = "Merge Selected File";
            this.btn_Load_Selected_RnD.UseVisualStyleBackColor = true;
            this.btn_Load_Selected_RnD.Click += new System.EventHandler(this.btn_Load_Selected_RnD_Click);
            // 
            // Converter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 216);
            this.Controls.Add(this.btn_Load_Selected_RnD);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.btn_Generate_MIPI);
            this.Controls.Add(this.btn_Load_XML_Config);
            this.Controls.Add(this.btn_Copy_RnD);
            this.Controls.Add(this.btn_Convert);
            this.Controls.Add(this.btn_Load_TCF);
            this.Controls.Add(this.btn_Load_RnD);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Converter";
            this.Text = "TCF Converter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Load_RnD;
        private System.Windows.Forms.Button btn_Load_TCF;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_Copy_RnD;
        private System.Windows.Forms.Button btn_Load_XML_Config;
        private System.Windows.Forms.Button btn_Generate_MIPI;
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.Button btn_Load_Selected_RnD;
    }
}

