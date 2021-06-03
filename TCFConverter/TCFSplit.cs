using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCFConverter
{
    public class TCFSplit
    {
        Struct_xlsx struct_xlsx = new Struct_xlsx();
        FileManager foldercreater = new FileManager();
        Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();  // Create Excel Instance 

        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;

        public Struct_xlsx LoadExcel(string prj, string filepath)
        {           
            if (filepath != "")
            {
                struct_xlsx.workbook = application.Workbooks.Open(Filename: @filepath); // Try Catch               
                
                Worksheet worksheet_main = struct_xlsx.workbook.Worksheets.get_Item("Main");
                Range prj_range = worksheet_main.Columns["A"].Find("Title", Missing.Value, XlFindLookIn.xlValues, Missing.Value, Missing.Value, XlSearchDirection.xlNext, false, false, Missing.Value);
                int prj_row = prj_range.Row;
                int prj_col = prj_range.Column;
               
                if(Convert.ToString((worksheet_main.Cells[prj_row, prj_col + 1] as Range).Value2) == prj)
                {
                    // Load Condition_PA tab in TCF for RF1
                    struct_xlsx.worksheet = struct_xlsx.workbook.Worksheets.get_Item("Condition_PA");
                    struct_xlsx.range = struct_xlsx.worksheet.UsedRange;


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

                    if (test != test2)
                    {
                        string test3 = (endrow + 1).ToString();
                        struct_xlsx.worksheet.Range[test3 + ":" + test2].Delete();
                    }
                    return struct_xlsx;
                }
                else
                {
                    MessageBox.Show("Choose Correct TCF File(You Selected Different Project TCF).");
                    struct_xlsx.workbook = null;
                    struct_xlsx.worksheet = null;
                    struct_xlsx.range = null;
                    return struct_xlsx;
                }
            }
            else
            {
                struct_xlsx.workbook = null;
                struct_xlsx.worksheet = null;
                struct_xlsx.range = null;
                return struct_xlsx;
            }
        }


        public void SpiltTCF(string rootfolderpath, List<Tuple<string, int, int>> tuple_list)
        {
            int index_mipi = FindColumn(struct_xlsx.range, "Copy to Mipi");
            int count = 0;
            
            string extension = ".xlsx";           
            foreach (var x in tuple_list)
            {
                Workbook new_workbook = application.Workbooks.Add();
                Worksheet new_worksheet = new_workbook.Worksheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;   // Open Target xlsx file.

                new_worksheet.Name = x.Item1;               

                Microsoft.Office.Interop.Excel.Range oldrangeheader = struct_xlsx.worksheet.Range[struct_xlsx.worksheet.Cells[1, 1], struct_xlsx.worksheet.Cells[2, struct_xlsx.worksheet.UsedRange.Columns.Count]];
                Microsoft.Office.Interop.Excel.Range oldrange = struct_xlsx.worksheet.Range[struct_xlsx.worksheet.Cells[x.Item2, 1], struct_xlsx.worksheet.Cells[x.Item3, struct_xlsx.worksheet.UsedRange.Columns.Count]];


                Microsoft.Office.Interop.Excel.Range newrangeheader = new_worksheet.Range[new_worksheet.Cells[1, 1], new_worksheet.Cells[2, struct_xlsx.worksheet.UsedRange.Columns.Count]];
                Microsoft.Office.Interop.Excel.Range newrange = new_worksheet.Range[new_worksheet.Cells[3, 1], new_worksheet.Cells[x.Item3, struct_xlsx.worksheet.UsedRange.Columns.Count]];


                oldrangeheader.Copy(newrangeheader);
                oldrange.Copy(newrange);


                //Freeze Rows
                new_worksheet.Activate();
                new_worksheet.Application.ActiveSheet.Rows[3].Select();
                new_worksheet.Application.ActiveWindow.FreezePanes = true;


                if (x.Item1 != "C_Prior" && x.Item1 != "C_Post")
                {
                    //Hardcoded Hidden Range
                    Range hiddenrange = new_worksheet.Range["L:U"];
                    Range hiddenrange2 = new_worksheet.Range["AB:CT"];
                    Range hiddenrange3 = new_worksheet.Range["DV:EX"];

                    int index_Pcon = FindColumn(struct_xlsx.range, "Para.Pcon");
                    int index_Ieff = FindColumn(struct_xlsx.range, "Para.Ieff");
                    int index_H2 = FindColumn(struct_xlsx.range, "Para.H2");
                    int index_ACLR1 = FindColumn(struct_xlsx.range, "Para.ACLR1");
                    int index_TxLeakage = FindColumn(struct_xlsx.range, "Para.TxLeakage");

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

                string path = rootfolderpath + count.ToString() + "_" + x.Item1 + "\\";

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                System.IO.FileInfo[] fi = di.GetFiles("*.xlsx");

                if (fi.Length == 0)
                {
                    new_workbook.SaveAs(Filename: path + count.ToString() + "_" + x.Item1 + "_" + "rev0" + extension);
                }
                else
                {
                    int filecount = foldercreater.RecentRevisionFileCheck(count.ToString() + "_" + x.Item1, rootfolderpath + count.ToString() + "_" + x.Item1);
                    new_workbook.SaveAs(Filename: path + count.ToString() + "_" + x.Item1 + "_" + "rev" + (filecount + 1).ToString() + extension);
                }
                count++;

                int totalProgress = (int)((double)count / tuple_list.Count * 100);              

                UpdateProgress(totalProgress);
                System.Windows.Forms.Application.DoEvents();

                new_workbook.Close();
                
            }
            
        }

        public List<Tuple<string, int, int>> FindRange(List<string> foldernameList)
        {

            int index_extractfolder = FindColumn(struct_xlsx.range, "Extract folder");
            List<Tuple<string, int, int>> range_Tuple = new List<Tuple<string, int, int>>();
            List<int> list_Range = new List<int>();
            List<int> list_Range2 = new List<int>();
            Worksheet sheet_for_findrange = struct_xlsx.worksheet;
            Range range_extract_folder = sheet_for_findrange.Range[sheet_for_findrange.Cells[1, index_extractfolder], sheet_for_findrange.Cells[struct_xlsx.range.Rows.Count, index_extractfolder]];


            foreach (var x in foldernameList)
            {
                Range findRange = range_extract_folder.Find(x, Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlNext, false, false, Missing.Value);
                list_Range.Add(findRange.Row);
            }

            for (int i = 0; i < foldernameList.Count() - 1; i++)
            {
                list_Range2.Add(list_Range[i + 1] - 1);
            }
            list_Range2.Add(struct_xlsx.range.Rows.Count - 1);

            for (int j = 0; j < foldernameList.Count(); j++)
            {
                range_Tuple.Add(new Tuple<string, int, int>(foldernameList[j], list_Range[j], list_Range2[j]));

            }
            return range_Tuple;
        }

        public int FindColumn(Range targetrange, string targetStr)
        {

            for (int j = 1; j <= targetrange.Columns.Count; ++j)
            {
                if (targetStr == Convert.ToString((targetrange.Cells[2, j] as Range).Value2))
                {
                    return j;
                }
            }

            return 0;
        }

        public void DeleteObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Memory Allocatoin Releasing Problem." + ex.ToString(), "Warning!");
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
