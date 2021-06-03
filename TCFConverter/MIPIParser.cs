using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TCFConverter
{
    class MIPIParser
    {         
        List<Tuple<string, string>> xmltuplelistTx = new List<Tuple<string, string>>();
        List<Tuple<string, string>> xmltuplelistRx = new List<Tuple<string, string>>();
        List<Tuple<string, string>> xmltuplelistTxMask = new List<Tuple<string, string>>();
        List<Tuple<string, string>> xmltuplelistRxMask = new List<Tuple<string, string>>();

        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;

        string prefix = "";
        string triggermask = "";

        

        public void ParseMIPIcmd(XmlNode xmlnode,  string filepath)
        {

            TCFSplit tcfsplit = new TCFSplit();
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();  // Create Excel Instance 
            Workbook workbook = application.Workbooks.Open(Filename: @filepath); // Try Catch                                                                                   
            Worksheet worksheet = workbook.Worksheets.get_Item("Condition_PA");

            XmlNodeList test = xmlnode.ChildNodes;
            foreach(XmlNode node in test)
            {
                if (node.Name == "Prefix")
                {
                    prefix = node.InnerXml;
                }
                if (node.Name == "TriggerMask")
                {
                    triggermask = node.InnerXml;
                }
            }
            xmltuplelistTx = ReadFromXML("TxRegister",test);
            xmltuplelistRx = ReadFromXML("RxRegister",test);

            xmltuplelistTxMask = ReadFromXML("TxMask", test);
            xmltuplelistRxMask = ReadFromXML("RxMask", test);

            int ind_exf = tcfsplit.FindColumn(worksheet.UsedRange, "C TYPE");
            int ind_MIPICommands= tcfsplit.FindColumn(worksheet.UsedRange, "MIPI Commands");
            int ctype_range = worksheet.UsedRange.Rows.Count;
            
            for (int i = 3; i < ctype_range; i++)
            {
                string string_Ctype = worksheet.Cells[i, ind_exf].Value2;
                if (string_Ctype != null)
                {
                    worksheet.Cells[i, ind_MIPICommands].Formula = "=" + MakeMIPICommand(string_Ctype, xmltuplelistTx, xmltuplelistTxMask, i) + "&" + MakeMIPICommand(string_Ctype, xmltuplelistRx, xmltuplelistRxMask, i);        
                    
                }
                int totalProgress = (int)((double)i / ctype_range * 100);
                UpdateProgress(totalProgress);                
            }

            string mipiokpath = filepath.Substring(0,filepath.LastIndexOf('.'));
            workbook.SaveAs(mipiokpath + "_mipi_completed" + ".xlsx");
            UpdateProgress(100);
        }

        public List<Tuple<string,string>> ReadFromXML(string nodename, XmlNodeList xmlnodelist)
        {
            List<Tuple<string, string>> xmltuplelist = new List<Tuple<string, string>>();
            foreach (XmlNode x in xmlnodelist)
            {
                if (x.Name == nodename)
                {
                    XmlNodeList Registerlist = x.ChildNodes;
                    for (int i = 0; i < Registerlist.Count; i++)
                    {
                        XmlNode test3 = Registerlist.Item(i);
                        xmltuplelist.Add(new Tuple<string,string>(test3.LocalName, test3.InnerText));                        
                    }                    
                }                   
            }
            return xmltuplelist;
        }
        
        public string MakeMIPICommand(string substr, List<Tuple<string, string>> tuplelist, List<Tuple<string, string>> tuplelistmask, int j)
        {
            StringBuilder sb = new System.Text.StringBuilder();
           if(substr == "R_PT_M")
            {
                sb.Append(prefix);
                for (int i = 0; i < tuplelist.Count(); i++)
                {
                    if (tuplelistmask[i].Item2 == "1")
                    {
                        sb.Append("&" + "\"(0x@\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }
                    else
                    {
                        sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }                   
                }
            }
            else if (substr == "R_PT_N")
            {
                sb.Append(prefix);
                for (int i = 0; i < tuplelist.Count(); i++)
                {
                    sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");


                }
            }
            else if (substr == "R_TM_M")
            {
                sb.Append(prefix + "&" + triggermask);
                for (int i = 0; i < tuplelist.Count() - 2; i++)
                {
                    if (tuplelistmask[i].Item2 == "1")
                    {
                        sb.Append("&" + "\"(0x@\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }
                    else
                    {
                        sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }

                }
            }
            else if (substr == "R_TM_N")
            {
                sb.Append(prefix + "&" + triggermask);
                for (int i = 0; i < tuplelist.Count() - 2; i++)
                {
                    sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");

                }
            }

            string returnstring = sb.ToString();
            return returnstring;
        }
       
    }
}
