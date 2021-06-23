using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.OleDb;

namespace TCFConverter
{
    public class TCFManager
    {       
        FileManager FolderCreator = new FileManager();        
        public static List<Tuple<string, int, int>> Tuple_List_All = new List<Tuple<string, int, int>>();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
       
        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;

        public void LoadExcel(List<string> BandList, string prj, string filepath, out bool isLoaded, out uint proid, out Microsoft.Office.Interop.Excel.Application ExcelApplication, out Struct_xlsx struct_xlsx, out Dictionary<string, int> ColumnIndexDic)
        {
            ExcelApplication = new Microsoft.Office.Interop.Excel.Application();
            GetWindowThreadProcessId(new IntPtr(ExcelApplication.Hwnd), out proid);
            ColumnIndexDic = new Dictionary<string, int>();

            if (filepath != "")
            {
                struct_xlsx.workbook = ExcelApplication.Workbooks.Open(filepath,0,true,5,"","",true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                ExcelApplication.Visible = false;
                ExcelApplication.DisplayAlerts = false;
                ExcelApplication.ScreenUpdating = false;
                ExcelApplication.EnableEvents = false;            
                ExcelApplication.DisplayStatusBar = false;            

                Worksheet worksheet_main = struct_xlsx.workbook.Worksheets.get_Item("Main");
                Range prj_range = worksheet_main.Columns["A"].Find("Title", Missing.Value, XlFindLookIn.xlValues, Missing.Value, Missing.Value, XlSearchDirection.xlNext, false, false, Missing.Value);
                              
                int prj_row = prj_range.Row;
                int prj_col = prj_range.Column;
               
                if (Convert.ToString((worksheet_main.Cells[prj_row, prj_col + 1] as Range).Value2) == prj)
                {
                    // Load Condition_PA tab in TCF for RF1
                    struct_xlsx.worksheet = struct_xlsx.workbook.Worksheets.get_Item("Condition_PA");
                    struct_xlsx.range = struct_xlsx.worksheet.UsedRange;

                    ColumnIndexDic = MakeIndexDictionary(struct_xlsx);
                    Tuple_List_All = FindRange(struct_xlsx, BandList, ColumnIndexDic);

                    //Unhide All Data                
                    struct_xlsx.worksheet.Activate();
                    struct_xlsx.worksheet.Select();
                    if (struct_xlsx.worksheet.FilterMode)
                        struct_xlsx.worksheet.ShowAllData();

                    // Delete Data under "#END"              
                    Range end_range = struct_xlsx.worksheet.Columns["A"].Find("#END", Missing.Value, XlFindLookIn.xlValues, Missing.Value, Missing.Value, XlSearchDirection.xlNext, false, false, Missing.Value);
                    int endrow = end_range.Row;
                    string test = endrow.ToString();
                    string test2 = struct_xlsx.range.Rows.Count.ToString();

                    if (endrow.ToString() != struct_xlsx.range.Rows.Count.ToString())
                    {
                        string test3 = (endrow + 1).ToString();
                        struct_xlsx.worksheet.Range[test3 + ":" + test2].Delete();
                    }
                    isLoaded = true;                    
                }
                else
                {
                    MessageBox.Show("Choose Correct TCF File(You Selected Different Project TCF).");                   
                    isLoaded = false;
                    struct_xlsx.range = null;
                    struct_xlsx.workbook = null;
                    struct_xlsx.worksheet = null;
                }
            }
            else
            {
                isLoaded = false;
                struct_xlsx.range = null;
                struct_xlsx.workbook = null;
                struct_xlsx.worksheet = null;
            }            
        }

        public void SpiltTCF(string rootfolderpath, List<string> SplitList, Struct_xlsx split_xlsx, Microsoft.Office.Interop.Excel.Application loadedexcel, Dictionary<string, int> ColumnIndexDic)
        {                                
            int count = 0;
            List<Tuple<string, int, int>>  tuple_list = FindRange(split_xlsx, SplitList, ColumnIndexDic);
            string extension = ".xlsx";           
            foreach (var x in tuple_list)
            {        

                Workbook new_workbook = loadedexcel.Workbooks.Add();                
                Worksheet new_worksheet = new_workbook.Worksheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;   // Open Target xlsx file.                
                new_worksheet.Name = x.Item1;               

                Microsoft.Office.Interop.Excel.Range oldrangeheader = split_xlsx.worksheet.Range[split_xlsx.worksheet.Cells[1, 1], split_xlsx.worksheet.Cells[2, split_xlsx.worksheet.UsedRange.Columns.Count]];
                Microsoft.Office.Interop.Excel.Range newrangeheader = new_worksheet.Range[new_worksheet.Cells[1, 1], new_worksheet.Cells[2, split_xlsx.worksheet.UsedRange.Columns.Count]];


                Microsoft.Office.Interop.Excel.Range oldrange = split_xlsx.worksheet.Range[split_xlsx.worksheet.Cells[x.Item2, 1], split_xlsx.worksheet.Cells[x.Item3, split_xlsx.worksheet.UsedRange.Columns.Count]];
                Microsoft.Office.Interop.Excel.Range newrange = new_worksheet.Range[new_worksheet.Cells[3, 1], new_worksheet.Cells[(x.Item3 - x.Item2) + 3, split_xlsx.worksheet.UsedRange.Columns.Count]];


                oldrangeheader.Copy(newrangeheader);
                oldrange.Copy(newrange);


                //Freeze Rows
                new_worksheet.Activate();
                new_worksheet.Application.ActiveSheet.Rows[3].Select();
                new_worksheet.Application.ActiveWindow.FreezePanes = true;


                if (x.Item1 != "C_Prior" && x.Item1 != "C_Post")
                {
                    //Hardcoded Hidden Range
                    Range hiddenrange = new_worksheet.Range["L:W"];
                    Range hiddenrange2 = new_worksheet.Range["AD:CW"];
                    Range hiddenrange3 = new_worksheet.Range["DW:FD"];

                    int index_Pcon = ColumnIndexDic["Para.Pcon"];
                    int index_Ieff = ColumnIndexDic["Para.Ieff"];
                    int index_H2 = ColumnIndexDic["Para.H2"];
                    int index_ACLR1 = ColumnIndexDic["Para.ACLR1"];
                    int index_TxLeakage = ColumnIndexDic["Para.TxLeakage"];                    

                    hiddenrange.Columns.ColumnWidth = 0;
                    hiddenrange2.Columns.ColumnWidth = 0;
                    hiddenrange3.Columns.ColumnWidth = 0;

                    //Unhide Neccessary Range 
                    new_worksheet.Cells[1, index_Pcon].EntireColumn.ColumnWidth = 10;
                    new_worksheet.Cells[1, index_Ieff].EntireColumn.ColumnWidth = 10;
                    new_worksheet.Cells[1, index_H2].EntireColumn.ColumnWidth = 10;
                    new_worksheet.Cells[1, index_ACLR1].EntireColumn.ColumnWidth = 10;
                    new_worksheet.Cells[1, index_TxLeakage].EntireColumn.ColumnWidth = 10;
                }
                
                //Save Splitted File 

                string path = rootfolderpath + x.Item1 + "\\";

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] fi = di.GetFiles("*.xlsx");

                if (fi.Length == 0)
                {
                    new_workbook.SaveAs(Filename: path +  x.Item1 + "_" + "rev0" + extension);
                }
                else
                {
                    int filecount = FolderCreator.RecentRevisionFileCheck(x.Item1, rootfolderpath +  x.Item1);
                    new_workbook.SaveAs(Filename: path + x.Item1 + "_" + "rev" + (filecount + 1).ToString() + extension);
                }
                count++;

                int totalProgress = (int)((double)count / tuple_list.Count * 100);              

                UpdateProgress(totalProgress);
                System.Windows.Forms.Application.DoEvents();

                new_workbook.Close();                
            }            
        }
        public bool InsertRnD(string TCFfilepath, XMLParmameter xmltcfparam, List<int> nameList_insert_index, List<string> nameList_insert, string insertpath, string xmlpath, Struct_xlsx insert_xlsx, Microsoft.Office.Interop.Excel.Application loadedexcel, Dictionary<string, int> ColumnIndexDic)
        {
            Tuple<string, int, int> Tuple_Insert;
            List<string> allitem_str = new List<string>();            
           
            string project = xmltcfparam.Project;
            string rev = xmltcfparam.Revision;
            string product = xmltcfparam.Product;
            List<string> band =  xmltcfparam.Band;                  

            for (int i = 0; i < nameList_insert.Count; i++)
            {
                int version = FolderCreator.RecentRevisionFileCheck(nameList_insert[i], insertpath + project + "\\" + product + "\\" + rev + "\\" + nameList_insert[i]);
                string finalfilename = insertpath + project + "\\" + product + "\\" + rev + "\\" + nameList_insert[i] + "\\" + nameList_insert[i] + "_rev" + version.ToString() + ".xlsx";

                ///Find for Inserting 
                Tuple_Insert = FindRange(insert_xlsx, nameList_insert[i], ColumnIndexDic); 

                Workbook insertworkbook = loadedexcel.Workbooks.Add();
                insertworkbook = loadedexcel.Workbooks.Open(Filename: finalfilename);

                Worksheet insertworksheet = insertworkbook.Worksheets.get_Item(nameList_insert[i]);
                Range insert_range = insertworksheet.Range["3" + ":" + insertworksheet.UsedRange.Rows.Count];

                /// 삽입하려는 Band가 Target 파일에 존재하지 않는 경우 
                if (Tuple_Insert.Item2 == 0) 
                {
                    if (nameList_insert_index[i] != 0)
                    {
                        string preband = band[nameList_insert_index[i] - 1];
                        int lastrange = FindlastRange(insert_xlsx.worksheet, preband, ColumnIndexDic).Item2;   

                        insert_range.Copy();
                        insert_xlsx.worksheet.Range[(lastrange + 2).ToString() + ":" + ((lastrange + 2) + insertworksheet.UsedRange.Rows.Count - 2).ToString()].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                    else //C_Prior case
                    {
                        insert_range.Copy();
                        insert_xlsx.worksheet.Range["3" + ":" + (insertworksheet.UsedRange.Rows.Count - 2).ToString()].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                else
                {
                    if (nameList_insert_index[i] != 0)
                    {
                        string preband = band[nameList_insert_index[i] - 1];
                        insert_xlsx.worksheet.Range[Tuple_Insert.Item2.ToString() + ":" + Tuple_Insert.Item3.ToString()].Delete();
                        int lastrange = FindlastRange(insert_xlsx.worksheet, preband, ColumnIndexDic).Item2;      

                        insert_range.Copy();
                        insert_xlsx.worksheet.Range[(lastrange + 2).ToString() + ":" + ((lastrange + 2) + insertworksheet.UsedRange.Rows.Count - 2).ToString()].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                    else //C_Prior case
                    {
                        insert_xlsx.worksheet.Range[Tuple_Insert.Item2.ToString() + ":" + Tuple_Insert.Item3.ToString()].Delete();

                        insert_range.Copy();
                        insert_xlsx.worksheet.Range[ "3" + ":" + (insertworksheet.UsedRange.Rows.Count - 2).ToString()].Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                
                int totalProgress = (int)((double)i / nameList_insert.Count * 100);

                UpdateProgress(totalProgress);
                System.Windows.Forms.Application.DoEvents();

            } 
            
            string insertfinalpath = TCFfilepath.Substring(0, TCFfilepath.LastIndexOf('.'));
            insert_xlsx.workbook.SaveAs(insertfinalpath + "_insert_completed" + ".xlsx");
            UpdateProgress(100);
            return true;
        }

        public List<Tuple<string, int, int>> FindRange(Struct_xlsx find_xlsx, List<string> foldernameList, Dictionary<string, int> ColumnIndexDic)
        {

            //int index_extractfolder = FindColumn(find_xlsx.range, "Extract folder");
            int index_extractfolder = ColumnIndexDic["Extract folder"];

            List<Tuple<string, int, int>> range_Tuple = new List<Tuple<string, int, int>>();
            List<int> list_Range = new List<int>();
            List<int> list_Range_last = new List<int>();

            Worksheet sheet_for_findrange = find_xlsx.worksheet;
            Range range_extract_folder = sheet_for_findrange.Range[sheet_for_findrange.Cells[1, index_extractfolder], sheet_for_findrange.Cells[find_xlsx.range.Rows.Count, index_extractfolder]];


            foreach (var x in foldernameList)
            {
                Range findRange = range_extract_folder.Find(x, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlNext, false, false, Missing.Value);
                Range findRange_last = range_extract_folder.Find(x, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, false, Missing.Value);
                if (findRange == null)
                {
                    list_Range.Add(0);
                    list_Range_last.Add(0);                    
                }
                else
                {
                    list_Range.Add(findRange.Row);
                    list_Range_last.Add(findRange_last.Row);
                }               
            }           

            for (int j = 0; j < foldernameList.Count(); j++)
            {
                range_Tuple.Add(new Tuple<string, int, int>(foldernameList[j], list_Range[j], list_Range_last[j]));
            }
            return range_Tuple;
        }
        //Overloading
        public Tuple<string, int, int> FindRange(Struct_xlsx insert_xlsx, string foldername, Dictionary<string, int> ColumnIndexDic)
        {
            int index_extractfolder = ColumnIndexDic["Extract folder"];
       
            int list_range;
            int list_range_last;

            Worksheet sheet_for_findrange = insert_xlsx.worksheet;
            Range range_extract_folder = sheet_for_findrange.Range[sheet_for_findrange.Cells[1, index_extractfolder], sheet_for_findrange.Cells[insert_xlsx.range.Rows.Count, index_extractfolder]];

            Range findRange = range_extract_folder.Find(foldername, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlNext, false, false, Missing.Value);
            Range findRange_last = range_extract_folder.Find(foldername, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, false, Missing.Value);

            if (findRange == null)
            {
                list_range  = 0;
                list_range_last = 0;
            }
            else
            {
                list_range = findRange.Row;
                list_range_last = findRange_last.Row;
            }
            Tuple<string, int, int> range_Tuple = new Tuple<string, int, int>(foldername, list_range, list_range_last);
           
            return range_Tuple;
        }
        public Tuple<string,int> FindlastRange(Worksheet find_sheet, string bandname, Dictionary<string, int> ColumnIndexDic)
        {
            int index_extractfolder = ColumnIndexDic["Extract folder"];                    

            //Worksheet sheet_for_findrange = struct_xlsx.worksheet;
            Range range_extract_folder = find_sheet.Range[find_sheet.UsedRange.Cells[1, index_extractfolder], find_sheet.UsedRange.Cells[find_sheet.UsedRange.Rows.Count, index_extractfolder]];
            Range findRange_last = range_extract_folder.Find(bandname, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, false, Missing.Value);

            Tuple<string, int> range = new Tuple<string, int>(bandname,findRange_last.Row);     
            
            return range;
        }   
        public Dictionary<string,int> MakeIndexDictionary(Struct_xlsx xlsx)
        {
            object[,] obj = xlsx.range.get_Value();
            Dictionary<string, int> Dic = new Dictionary<string, int>();
            for (int i = 1; i <= xlsx.range.Columns.Count; i++)
            {
                if (obj[2, i] != null)
                {
                    if (!Dic.ContainsKey(obj[2, i].ToString()))
                    {
                         Dic.Add(obj[2, i].ToString(), i);
                    }
                }
            }
            return Dic;
        }
    }
}
