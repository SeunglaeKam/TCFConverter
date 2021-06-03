using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCFConverter
{
    public class RnDMerge
    {
        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;

        public void MergeRnDFile(string mergepath, List<String> filenameslist)
        {
            
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
            Struct_xlsx struct_xlsx_merge = new Struct_xlsx();
            List<int> tmp_list = new List<int>();
            Dictionary<int, string> tmpdic = new Dictionary<int, string>();
            bool isFirst = true;
            int count = 0;


            Workbook merge_workbook = application.Workbooks.Add();
            Worksheet merged_worksheet = merge_workbook.Worksheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;   // Open Target xlsx file.
            Range merge_range = merged_worksheet.UsedRange;
            merged_worksheet.Name = "Condition_PA";

            for (int i = 0; i < filenameslist.Count(); i++)
            {
                int last = filenameslist[i].LastIndexOf("\\");
                string tmp = filenameslist[i].Substring(last + 1, filenameslist[i].Length - (last + 1));
                int last2 = tmp.IndexOf("_");
                string tmp2 = tmp.Substring(0, last2);
                tmpdic[Convert.ToInt16(tmp2)] = filenameslist[i];
            }

            tmpdic = tmpdic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (var key in tmpdic.Keys)
            {
                struct_xlsx_merge.workbook = application.Workbooks.Open(Filename: tmpdic[key]);
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
                count++;
                isFirst = false;

                int totalProgress = (int)((double)count / tmpdic.Count * 100);
                UpdateProgress(totalProgress);
               
            }

            //Filter added. 
            int mergedcolumncnt = merged_worksheet.UsedRange.Columns.Count;
            Microsoft.Office.Interop.Excel.Range hiddenrange = merged_worksheet.Range[merged_worksheet.Cells[2, 1], merged_worksheet.Cells[2, mergedcolumncnt]];
            hiddenrange.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, false);


            //Freeze Rows
            merged_worksheet.Activate();
            merged_worksheet.Application.ActiveSheet.Rows[3].Select();
            merged_worksheet.Application.ActiveWindow.FreezePanes = true;


            // "#END" added.
            merged_worksheet.Cells[merged_worksheet.UsedRange.Rows.Count + 1, 1] = "#END";

            //Save Merged Excel File
            string mergefilename = "merged" + "_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";  // File name including Band number   
            string movepath = System.IO.Path.Combine(mergepath + mergefilename);
            merge_workbook.SaveAs(Filename: movepath);
            DeleteObject(merged_worksheet);
            DeleteObject(merge_workbook);
            DeleteObject(merge_range);
            DeleteObject(application);
            application.Quit();
        }

        public void MergeSelectedRnDFile(List<Tuple<string, int, int>> tuple_selected)
        {

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
