using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace TCFConverter
{
    public class RnDMerge
    {
        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;       

        public Struct_xlsx MergeRnDFile(string prjpath, string mergepath, List<String> mergelist, Microsoft.Office.Interop.Excel.Application target_excel, Struct_xlsx target_xlsx, string TCFpath)
        {          
            Struct_xlsx struct_xlsx_merge = new Struct_xlsx();
            List<int> tmp_list = new List<int>();
            Dictionary<int, string> tmpdic = new Dictionary<int, string>();
            bool isFirst = true;
           

            Workbook merge_workbook = target_excel.Workbooks.Add(); //Source Workbook

            Worksheet merged_worksheet = merge_workbook.Worksheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;   // Open Target xlsx file.
            Range merge_range = merged_worksheet.UsedRange;

            merged_worksheet.Name = "Condition_PA_New";            

            FileManager fm = new FileManager();

            for (int i = 0; i < mergelist.Count; i++)
            {                
                int version = fm.RecentRevisionFileCheck(mergelist[i], prjpath + mergelist[i]);
                string finalfilename = prjpath + mergelist[i] + "\\" + mergelist[i] + "_rev" + version.ToString() + ".xlsx";

                struct_xlsx_merge.workbook = target_excel.Workbooks.Open(Filename: finalfilename);
                struct_xlsx_merge.worksheet = struct_xlsx_merge.workbook.Worksheets.get_Item(1);
                if (isFirst)
                {
                    Microsoft.Office.Interop.Excel.Range oldRange_merge_Header = struct_xlsx_merge.worksheet.Range[struct_xlsx_merge.worksheet.Cells[1, 1], struct_xlsx_merge.worksheet.Cells[2, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];
                    Microsoft.Office.Interop.Excel.Range newrange_merge_Header = merged_worksheet.Range[merged_worksheet.Cells[1, 1], merged_worksheet.Cells[2, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];

                    oldRange_merge_Header.Copy(newrange_merge_Header);

                    Microsoft.Office.Interop.Excel.Range oldRange_merge = struct_xlsx_merge.worksheet.Range[struct_xlsx_merge.worksheet.Cells[3, 1], struct_xlsx_merge.worksheet.Cells[struct_xlsx_merge.worksheet.UsedRange.Rows.Count, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];  //합칠 대상의 File의 Range
                    Microsoft.Office.Interop.Excel.Range newrange_merge = merged_worksheet.Range[merged_worksheet.Cells[3, 1], merged_worksheet.Cells[struct_xlsx_merge.worksheet.UsedRange.Rows.Count, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];
                    oldRange_merge.Copy(newrange_merge);
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range oldRange_merge = struct_xlsx_merge.worksheet.Range[struct_xlsx_merge.worksheet.Cells[3, 1], struct_xlsx_merge.worksheet.Cells[struct_xlsx_merge.worksheet.UsedRange.Rows.Count, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];  //합칠 대상의 File의 Range
                    Microsoft.Office.Interop.Excel.Range newrange_merge = merged_worksheet.Range[merged_worksheet.Cells[merged_worksheet.UsedRange.Rows.Count + 2, 1], merged_worksheet.Cells[merged_worksheet.UsedRange.Rows.Count + 2 + struct_xlsx_merge.worksheet.UsedRange.Rows.Count, struct_xlsx_merge.worksheet.UsedRange.Columns.Count]];
                    oldRange_merge.Copy(newrange_merge);
                }

                isFirst = false;

                int totalProgress = (int)(((double)i / mergelist.Count) * 100);
                UpdateProgress(totalProgress);
            }

            //Filter added. 
            //int mergedcolumncnt = merged_worksheet.UsedRange.Columns.Count;
            //Microsoft.Office.Interop.Excel.Range hiddenrange = merged_worksheet.Range[merged_worksheet.Cells[2, 1], merged_worksheet.Cells[2, mergedcolumncnt]];
            //hiddenrange.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, false);


            //Freeze Rows
            //merged_worksheet.Activate();
            //merged_worksheet.Application.ActiveSheet.Rows[3].Select();
            //merged_worksheet.Application.ActiveWindow.FreezePanes = true;


            // "#END" added.
            merged_worksheet.Cells[merged_worksheet.UsedRange.Rows.Count + 1, 1] = "#END";

            target_xlsx.range.Clear();
            
            //Save Merged Excel File
            string mergefilename = "merged" + "_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";  // File name including Band number   
            string movepath = System.IO.Path.Combine(mergepath + mergefilename);
            merge_workbook.SaveAs(Filename: movepath);
            target_xlsx.range.Interior.Color = ColorTranslator.ToOle(Color.White);
            int row = merged_worksheet.UsedRange.Rows.Count;
            int column =  merged_worksheet.UsedRange.Columns.Count;
            

            Range targetRange = target_xlsx.worksheet.Range[target_xlsx.worksheet.Cells[1,1], target_xlsx.worksheet.Cells[row, column]];         
            merged_worksheet.UsedRange.Copy();

            targetRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);            
            target_xlsx.workbook.Save();

            UpdateProgress(100);
            return target_xlsx;
        }

        private static System.Data.DataTable GetDataSetFromExcelFile(string finalfilename, string band)
        {
            // 엑셀 문서 내용 추출
            object missing = System.Reflection.Missing.Value;
             

            string strProvider = string.Empty;
            strProvider = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + finalfilename + @";Extended Properties=Excel 12.0";

            OleDbConnection excelConnection = new OleDbConnection(strProvider);
            excelConnection.Open();

            string strQuery = "SELECT * FROM" + "[" + band + "$]";

            OleDbCommand dbCommand = new OleDbCommand(strQuery, excelConnection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

            System.Data.DataTable dTable = new System.Data.DataTable();
            dataAdapter.Fill(dTable);                
            

            dTable.Dispose();
            dataAdapter.Dispose();
            dbCommand.Dispose();

            excelConnection.Close();
            excelConnection.Dispose();

            return dTable;
        }
       
    }
}
