using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;


namespace TCFConverter
{    
    public struct Struct_xlsx
    {
        public Workbook workbook;
        public Worksheet worksheet;
        public Range range;       
    };
    public partial class Converter : Form
    {
        TCFManager ExcelLoader = new TCFManager();
        FileManager FolderCreator = new FileManager();
        RnDMerge RndMerge = new RnDMerge();
        XMLLoader XmlLoader = new XMLLoader();
        XmlDocument ConfigXml;
        XMLParmameter XmlParmeterMain = new XMLParmameter();
        MIPIParser MIPIParser = new MIPIParser();
        XmlNodeList XmlList;
        List<string> MainBandList = new List<string>();
        
        string rootfolderpath = @"\\cifs.kosinas01.sen.broadcom.net\WSD\NPI_Share\TCF\";
        string backslash = "\\";
        string merge = "Merge";
        string xmlfilename;
        string TCFfilepath;

        string mainproject;
        string mainrevision;
        string product;
        XmlNode band;

        uint processID;
        Microsoft.Office.Interop.Excel.Application common_excel;
        Struct_xlsx common_xlsx;

        public Converter()
        {
            InitializeComponent();            
            this.propertygrid.SelectedObject = XmlLoader;

            btn_Load_RnD.Enabled = false;
            btn_Load_TCF.Enabled = false;
            btn_Generate_MIPI.Enabled = false;
            btn_Convert.Enabled = false;
            btn_Insert_RnD.Enabled = false;
            btn_Copy_RnD.Enabled = false;

            ExcelLoader.UpdateProgress += UpdateProgress;
            MIPIParser.UpdateProgress += UpdateProgress;
            RndMerge.UpdateProgress += UpdateProgress;           
        }

        private void Converter_Load(object sender, EventArgs e)
        {
            band_ListView.View = View.Details;
        }

        #region ListView Property
        private void band_ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new System.Drawing.Point(e.Bounds.Left + 4, e.Bounds.Top + 4), value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal : System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        private void band_ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void band_ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void band_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(this.band_ListView.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                this.band_ListView.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in this.band_ListView.Items)
                    item.Checked = !value; this.band_ListView.Invalidate();
            }
        }
        #endregion

        private void UpdateProgress(int ProgressPercentage)
        {
            progressbar.Value = ProgressPercentage;
        }              

        #region Merge All Copied File
        private void btn_Merge_RnD_Click(object sender, EventArgs e)
        {
            List<string> MergeNameList = new List<string>();

            string prjpath = rootfolderpath + mainproject + backslash + product + backslash + mainrevision + backslash; 
            string mergepath = prjpath + merge + backslash;
            
            foreach (ListViewItem item in band_ListView.CheckedItems)
            {
                MergeNameList.Add(item.SubItems[1].Text);
            }
            if (XmlLoader != null)
            {                
                if (MergeNameList.Count != 0)
                {
                    if (!FolderCreator.FolderCheck(mergepath))
                    {
                        Directory.CreateDirectory(mergepath);
                    }
                    RndMerge.MergeRnDFile(prjpath, mergepath, MergeNameList, common_excel, common_xlsx, TCFfilepath);
                    MessageBox.Show("Success Merging RnD Format File.");
                    MIPIParser.ParseMIPIcmd(XmlList.Item(0), XmlParmeterMain, TCFfilepath, common_xlsx);
                    MessageBox.Show("MIPI Cmd Generate Complete.");
                    UpdateProgress(0);                    
                }
                else
                {
                    MessageBox.Show("Choose Band.");
                }
            }
            else
            {
                MessageBox.Show("Load XML Config File.");
            }
        }
        #endregion

        #region Load TCF File
        private void btn_Load_TCF_Click(object sender, EventArgs e)
        {
            Stopwatch SW1 = new Stopwatch();
            SW1.Start();
            if (XmlLoader != null)
            {                
                OpenFileDialog ofd_TCF = new OpenFileDialog();
                ofd_TCF.Title = "TCF File Open";
                ofd_TCF.FileName = "";
                ofd_TCF.Filter = "TCF File (*.xlsx) | *.xlsx; | All Files (*.*) | *.*";
      
                DialogResult dr_TCF = ofd_TCF.ShowDialog();
               
                if (dr_TCF == DialogResult.OK)
                {
                    TCFfilepath = ofd_TCF.FileName;
                    ExcelLoader.LoadExcel(MainBandList, mainproject, TCFfilepath, out bool isLoaded, out uint mainprocessID, out Microsoft.Office.Interop.Excel.Application loaded_excel, out Struct_xlsx loaded_xlsx);
                    processID = mainprocessID;
                    common_excel = loaded_excel;
                    common_xlsx = loaded_xlsx;
                    //Load Excel         
                    if (!isLoaded)
                    {
                        MessageBox.Show("Fail to Load Excel File");
                    }
                    else
                    {
                        MessageBox.Show("Success Loading TCF");
                        textBox.Text = TCFfilepath;
                        btn_Convert.Enabled = true;                   
                    }          
                }
                else if (dr_TCF == DialogResult.Cancel)
                {
                    MessageBox.Show("TCF File Open Fail.");
                }
            }
            else
            {
                MessageBox.Show("Load XML Config File.");
            }
            MessageBox.Show("Loading " +SW1.Elapsed.TotalMilliseconds);

        }
        #endregion

        #region Split Loaded TCF File
        private void btn_Split_Click(object sender, EventArgs e)
        {
            List<string> SplitNameList = new List<string>();

            foreach(ListViewItem item in band_ListView.CheckedItems)
            {
                SplitNameList.Add(item.SubItems[1].Text);
            }

            if (SplitNameList.Count() != 0)
            {               
                string path = rootfolderpath  + mainproject + backslash + product + backslash + mainrevision + backslash;
                for (int j = 0; j < SplitNameList.Count(); j++)
                {
                    FolderCreator.CreateFolder(path + SplitNameList[j]);
                }

                ExcelLoader.SpiltTCF(path, SplitNameList, common_xlsx, common_excel);
                MessageBox.Show("Split Complete.");
                UpdateProgress(0);
            }
            else
            {
                MessageBox.Show("Choose Band");
            }
        }
        #endregion

        #region Copy RnD File for Merging
        private void btn_Copy_RnD_Click(object sender, EventArgs e)
        {
            List<string> CopyNameList = new List<string>();
            foreach (ListViewItem item in band_ListView.CheckedItems)
            {
                CopyNameList.Add(item.SubItems[1].Text);
            }

            if (XmlLoader != null)
            {
                if (!FolderCreator.FolderCheck(rootfolderpath + mainproject + backslash + product + backslash + mainrevision + backslash + merge))
                {
                    Directory.CreateDirectory(rootfolderpath + mainproject + backslash + product + backslash + mainrevision + backslash + merge);
                }
                if(!FolderCreator.CopyFile(CopyNameList, rootfolderpath + mainproject + backslash + product + backslash + mainrevision + backslash))
                {
                    MessageBox.Show("Copy Fail. Check File Name.");
                }
                else
                {
                    MessageBox.Show("Copy Success.");
                }
            }
            else
            {
                MessageBox.Show("Load XML Config File.");
            }
        }
        #endregion

        #region Load XML Config File
        private void btn_Load_XML_Config_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd_xml = new OpenFileDialog();
            ofd_xml.Title = "Config File Open";
            ofd_xml.FileName = "";
            ofd_xml.Filter = "Config File (*.xml) | *.xml; | All Files (*.*) | *.*";

            DialogResult dr_xml = ofd_xml.ShowDialog();

            if (dr_xml == DialogResult.OK)
            {
                xmlfilename = ofd_xml.FileName;

                XmlParmeterMain = XmlLoader.ParsingXML(xmlfilename);

                mainproject = XmlParmeterMain.Project;
                mainrevision = XmlParmeterMain.Revision;
                product = XmlParmeterMain.Product;
                band = XmlParmeterMain.Band;

                ConfigXml = XmlLoader.LoadingXml(xmlfilename);
                XmlList = ConfigXml.GetElementsByTagName("Project");
                MessageBox.Show("Success Loading Config File.");
                if(band_ListView.Items.Count != 0)
                {
                    band_ListView.Items.Clear();
                }
                for (int i = 0; i < band.ChildNodes.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Add(band.ChildNodes.Item(i).Name);
                    band_ListView.Items.Add(lvi);
                    MainBandList.Add(band.ChildNodes.Item(i).Name);
                }                
                band_ListView.EndUpdate();
                
                btn_Load_RnD.Enabled = true;
                btn_Load_TCF.Enabled = true;
                btn_Generate_MIPI.Enabled = true;               
                btn_Insert_RnD.Enabled = true;
                btn_Copy_RnD.Enabled = true;
            }
            else
            {
                MessageBox.Show("File Open Fail.");
            }            
            
            
        }
        #endregion

        #region Generate MIPI CMD 
        private void btn_Generate_MIPI_Click(object sender, EventArgs e)
        {
            if (XmlLoader != null)
            {
                OpenFileDialog ofd_MIPI = new OpenFileDialog();
                
                ofd_MIPI.Title = "TCF File Open";
                ofd_MIPI.FileName = "";
                ofd_MIPI.Filter = "Config File (*.xlsx) | *.xlsx; | All Files (*.*) | *.*";

                DialogResult dr_xml = ofd_MIPI.ShowDialog();

                if (dr_xml == DialogResult.OK)
                {
                    string fileFullName = ofd_MIPI.FileName;                 
                    //XmlList = ConfigXml.GetElementsByTagName("Project");
                    MIPIParser.ParseMIPIcmd(XmlList.Item(0), XmlParmeterMain, fileFullName, out uint mipiprocessID);
                    MessageBox.Show("MIPI Cmd Generate Complete.");
                    UpdateProgress(0);
                    if (mipiprocessID != 0)
                    {
                        System.Diagnostics.Process excelProcess = System.Diagnostics.Process.GetProcessById((int)mipiprocessID);
                        excelProcess.CloseMainWindow();
                        excelProcess.Refresh();
                        excelProcess.Kill();
                    }
                }
                else
                {
                    MessageBox.Show("TCF File Open Fail.");
                }
            }
            else
            {
                MessageBox.Show("Load XML Config File.");
            }
        }
        #endregion
        #region Insert RnD File 
        private void btn_Insert_RnD_Click(object sender, EventArgs e)
        {
            List<string> nameList_insert = new List<string>();
            List<int> nameList_insert_index = new List<int>();
             
            foreach (ListViewItem item in band_ListView.CheckedItems)
            {                
                nameList_insert.Add(item.SubItems[1].Text);
                nameList_insert_index.Add(item.Index);
            }
            if (XmlLoader != null && TCFfilepath != null)
            {
                ExcelLoader.InsertRnD(XmlParmeterMain, nameList_insert_index, nameList_insert, rootfolderpath, xmlfilename, common_xlsx, common_excel);
                MessageBox.Show("Insert Complete.");
                UpdateProgress(0);
            }
            else
            {
                MessageBox.Show("Load XML File or TCF File.");
            }            
        }
        #endregion
        private void Prop_Value_Changed(object s, PropertyValueChangedEventArgs e)
        {
            GridItem griditem =  e.ChangedItem;
            string gridlabel = griditem.Label;
            switch (gridlabel)
            {
                case "Project Name":
                    XmlParmeterMain.Project = griditem.Value.ToString();
                    break;
                case "ProjectNumber":
                    XmlParmeterMain.Product = griditem.Value.ToString();
                    break;
                case "Revision":
                    XmlParmeterMain.Revision = griditem.Value.ToString();
                    break;
                case "TxUSID":
                    XmlParmeterMain.TXUSID = griditem.Value.ToString();
                    break;
                case "RxUSID":
                    XmlParmeterMain.RXUSID = griditem.Value.ToString();
                    break;
                case "Prefix":
                    XmlParmeterMain.PreFix = griditem.Value.ToString();
                    break;
                case "TxTriggerMask":
                    XmlParmeterMain.TxTriggerMask = griditem.Value.ToString();
                    break;
                case "RxTriggerMask":
                    XmlParmeterMain.RxTriggerMask = griditem.Value.ToString();
                    break;                    
            }            
        }

        private void Converter_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (processID != 0)
            {
                System.Diagnostics.Process excelProcess = System.Diagnostics.Process.GetProcessById((int)processID);
                excelProcess.CloseMainWindow();
                excelProcess.Refresh();
                excelProcess.Kill();
            }
        }
    }  
}
