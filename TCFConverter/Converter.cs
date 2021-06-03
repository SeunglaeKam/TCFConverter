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
#region Microsoft Excel Loading
using Microsoft.Office.Interop.Excel;
#endregion
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
        TCFSplit excelloader = new TCFSplit();
        FileManager foldercreater = new FileManager();
        RnDMerge rndmerge = new RnDMerge();
        XMLLoader xmlloader;
        XmlDocument configxml;
        XmlNodeList xmllist;
        MIPIParser mipiparser = new MIPIParser();

        public List<string> foldernameList = new List<string>();       
        static string rootfolderpath = @"\\cifs.kosinas01.sen.broadcom.net\WSD\NPI_Share\TCF\";      

        Struct_xlsx xlsx_TCF;
        int ind_exf;
       

        public Converter()
        {
            InitializeComponent();

            excelloader.UpdateProgress += UpdateProgress;
            mipiparser.UpdateProgress += UpdateProgress;
            rndmerge.UpdateProgress += UpdateProgress;
        }
        private void UpdateProgress(int ProgressPercentage)
        {

            progressbar.Value = ProgressPercentage;

        }

        #region Merge All Copied File
        private void btn_Merge_RnD_Click(object sender, EventArgs e)
        {
            if (xmlloader != null)
            {
                OpenFileDialog ofd_RnD = new OpenFileDialog();
                List<string> filenameslist = new List<string>();

                //Multi-Select Enable
                ofd_RnD.Multiselect = true;
                ofd_RnD.Title = "RnD Format File Open";
                ofd_RnD.FileName = "";
                ofd_RnD.Filter = "RnD_Format File (*.xlsx) | *.xlsx; | All Files (*.*) | *.*";

                //Loading File Open Window
                DialogResult dr_TCF = ofd_RnD.ShowDialog();

                //OK Button Click
                if (dr_TCF == DialogResult.OK)
                {
                    filenameslist = ofd_RnD.FileNames.ToList();
                    /////Merge
                    rndmerge.MergeRnDFile(rootfolderpath + xmllist.Item(0).FirstChild.InnerText + "\\" + "Merge" + "\\", filenameslist);
                    MessageBox.Show("Success Merging RnD Format File.");
                    UpdateProgress(0);
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
            if(xmlloader != null)
            {
                
                OpenFileDialog ofd_TCF = new OpenFileDialog();
                ofd_TCF.Title = "TCF File Open";
                ofd_TCF.FileName = "";
                ofd_TCF.Filter = "TCF File (*.xlsx) | *.xlsx; | All Files (*.*) | *.*";
                string filePath_TCF = "";
                string str = "";

                DialogResult dr_TCF = ofd_TCF.ShowDialog();

                if (dr_TCF == DialogResult.OK)
                {
                    filePath_TCF = ofd_TCF.FileName;
                    
                    //Load Excel
                    xlsx_TCF = excelloader.LoadExcel(xmllist.Item(0).FirstChild.InnerText, filePath_TCF);


                    if (xlsx_TCF.workbook == null)
                    {
                        MessageBox.Show("Fail to Load Excel File");
                    }
                    ind_exf = excelloader.FindColumn(xlsx_TCF.range, "Extract folder");
                    if (ind_exf == 0)
                    {
                        MessageBox.Show("Extract folder is not found.");
                        return;
                    }

                    for (int i = 3; i < xlsx_TCF.range.Rows.Count; i++)
                    {
                        str = (string)(xlsx_TCF.range.Cells[i, ind_exf] as Range).Value2;
                        if (str != "" && str != "n70" && str != null)
                        {
                            foldernameList.Add(str);
                            int totalProgress = (int)((double)i / xlsx_TCF.range.Rows.Count * 100);
                            UpdateProgress(totalProgress);
                        }                      

                    }
                    UpdateProgress(100);
                    MessageBox.Show("Success Loading TCF");
                    
                    UpdateProgress(0);
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
        }
        #endregion

        #region Split Loaded TCF File
        private void btn_Split_Click(object sender, EventArgs e)
        {         
            if (foldernameList.Count() != 0)
            {
                foldernameList = foldernameList.Distinct().ToList();    // TCF상의 Extract Folder 순서 List 
                string path = rootfolderpath + xmllist.Item(0).FirstChild.InnerText + "\\";
                for (int j = 0; j < foldernameList.Count(); j++)
                {

                    foldercreater.CreateFolder(path + (j.ToString() + "_" + foldernameList[j]));

                }
                excelloader.SpiltTCF(path, excelloader.FindRange(foldernameList));
                MessageBox.Show("Split Complete.");
                UpdateProgress(0);
            }
            else
            {
                MessageBox.Show("Load TCF File First.");
            }
        }
        #endregion

        #region Copy RnD File for Merging
        private void btn_Copy_RnD_Click(object sender, EventArgs e)
        {
           
            if(xmlloader != null)
            {
                if (!foldercreater.FolderCheck(rootfolderpath + xmllist.Item(0).FirstChild.InnerText + "\\" + "Merge"))
                {
                    Directory.CreateDirectory(rootfolderpath + xmllist.Item(0).FirstChild.InnerText + "\\" +  "Merge");
                }

                foldercreater.CopyFile(rootfolderpath + xmllist.Item(0).FirstChild.InnerText + "\\");
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
                string fileFullName = ofd_xml.FileName;
                xmlloader = new XMLLoader();
                configxml = xmlloader.LoadingXml(fileFullName);
                xmllist = configxml.GetElementsByTagName("Project");
                string prj = xmllist.Item(0).FirstChild.InnerText;
                MessageBox.Show("Success Loading Config File.");
            }
            else
            {

            }            
        }
        #endregion

        #region Generate MIPI CMD 
        private void btn_Generate_MIPI_Click(object sender, EventArgs e)
        {
            if (xmlloader != null)
            {
                OpenFileDialog ofd_MIPI = new OpenFileDialog();
                
                ofd_MIPI.Title = "TCF File Open";
                ofd_MIPI.FileName = "";
                ofd_MIPI.Filter = "Config File (*.xlsx) | *.xlsx; | All Files (*.*) | *.*";

                DialogResult dr_xml = ofd_MIPI.ShowDialog();

                if (dr_xml == DialogResult.OK)
                {
                    string fileFullName = ofd_MIPI.FileName;                    
                    mipiparser.ParseMIPIcmd(xmllist.Item(0),fileFullName);
                    MessageBox.Show("MIPI Cmd Generate Complete.");
                    UpdateProgress(0);
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

        private void btn_Load_Selected_RnD_Click(object sender, EventArgs e)
        {
            if(foldernameList.Count != 0)
            {                
                rndmerge.MergeSelectedRnDFile(excelloader.FindRange(foldernameList));
            }      
            else
            {
                MessageBox.Show("Load TCF File First.");
            }
        }
    }

    public class FileManager
    {
        public void CreateFolder(string folderpath)
        {
            if(!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }                                   
        }

        public void CopyFile(string folderpath)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(folderpath);
            DirectoryInfo[] drinfo = directoryInfo.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);  // Directory Info                      
            
            if (FolderCheck(folderpath))
            {
                foreach (var folder in drinfo)
                {
                    if (folder.Name != "Merge")  // Folder Name
                    {
                        string input = folder.Name;
                        int start = folder.Name.IndexOf("_")+1;                       
                        input = input.Substring(start, folder.Name.Length - start);
                        string version = RecentRevisionFileCheck(folder.Name, folderpath + folder.Name).ToString();
                        var result = folder.GetFiles("*.*", SearchOption.AllDirectories).OrderBy(t => t.LastWriteTime).ToList();

                        string targetfilename = folder.Name + "_rev" + version + ".xlsx";

                        string target_path = System.IO.Path.Combine(folderpath, "Merge\\" + targetfilename);
                        System.IO.File.Copy(folderpath + folder + "\\" + targetfilename, target_path, true);
                    }
                }
            }            
        }       

        public bool FolderCheck(string folderpath)
        {
            if (System.IO.Directory.Exists(folderpath))
            {
                return true;
            }
            return false;
        }

        public int RecentRevisionFileCheck(string band, string folderpath)
        {
            DirectoryInfo di = new DirectoryInfo(folderpath);
            FileInfo[] fi =  di.GetFiles();
            List<int> numList = new List<int>();
            foreach(var x in fi)
            {
                string tmp = x.ToString().Replace(band+"_rev", "");
                string number = tmp.Replace(".xlsx", "");
                numList.Add(Convert.ToInt16(number));                
            }

            return numList.Max();
        }


    }

   
}
