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

namespace TCFConverter
{
    class MIPIParser
    {
        List<Tuple<string, string, string>> xmltuplelistTx = new List<Tuple<string, string, string>>();
        List<Tuple<string, string, string>> xmltuplelistRx = new List<Tuple<string, string, string>>();
        List<Tuple<string, string>> xmltuplelistTxDAQ = new List<Tuple<string, string>>();
        List<Tuple<string, string>> xmltuplelistRxLNA = new List<Tuple<string, string>>();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public delegate void UpdateProgressDelegate(int ProgressPercentage);
        public event UpdateProgressDelegate UpdateProgress;

        string txusid = "";
        string rxusid = "";
        string prefix = "";
        string txtriggermask = "";
        string rxtriggermask = "";
        XmlNode txregister;
        XmlNode rxregister;
        XmlNode txdaq;
        XmlNode rxlna;
        public void ParseMIPIcmd(XmlNode xmlnode, XMLParmameter xmlparam,  string filepath, out uint proid)
        {           

            TCFManager tcfsplit = new TCFManager();
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();  
            Workbook workbook = application.Workbooks.Open(Filename: @filepath);
            Worksheet worksheet = workbook.Worksheets.get_Item(1);     
            if(workbook.Worksheets.Count != 1)
            {
                worksheet = workbook.Worksheets.get_Item("Condition_PA");
            }

            GetWindowThreadProcessId(new IntPtr(application.Hwnd), out proid);

            Dictionary<string, int> mipi_dic = new Dictionary<string, int>();
            Dictionary<string, int> mipi_dic_rx = new Dictionary<string, int>();

            XmlNodeList childnode = xmlnode.ChildNodes;
            txusid = xmlparam.TXUSID;
            rxusid = xmlparam.RXUSID;
            prefix = xmlparam.PreFix;
            txtriggermask = xmlparam.TxTriggerMask;
            rxtriggermask = xmlparam.RxTriggerMask;
            txregister = xmlparam.TxRegister;
            rxregister = xmlparam.RxRegister;           

            xmltuplelistTx = ReadFromXML3("TxRegister", txregister);
            xmltuplelistRx = ReadFromXML3("RxRegister", rxregister);

            int ind_exf = TCFManager.ColumnIndexDic["Mipi_Compliance"];
            int ind_MIPICommands = TCFManager.ColumnIndexDic["MIPI Commands"];
            int ind_band = TCFManager.ColumnIndexDic["BAND"];
            int ind_switch_tx = TCFManager.ColumnIndexDic["Switch_TX"];
            int ind_switch_ant = TCFManager.ColumnIndexDic["Switch_ANT"];
            int ind_switch_rx = TCFManager.ColumnIndexDic["Switch_RX"];
            int ind_testmode = TCFManager.ColumnIndexDic["Test Mode"];
            int ind_powermode = TCFManager.ColumnIndexDic["Power_Mode"];
            int ind_trx = TCFManager.ColumnIndexDic["TRX"];
            int index_extractfolder = TCFManager.ColumnIndexDic["Extract folder"];

            int ctype_range = worksheet.UsedRange.Rows.Count;
            
            for (int i = 3; i <= ctype_range; i++)
            {
                string string_Ctype = worksheet.Cells[i, ind_exf].Value2;
                if (string_Ctype != null)
                {
                    string Tx_MIPI_cmd = MakeMIPICommand(false, string_Ctype, xmltuplelistTx, i);
                    string Rx_MIPI_cmd = MakeMIPICommand(true, string_Ctype, xmltuplelistRx, i);

                    worksheet.Cells[i, ind_MIPICommands].Formula = "=" + txusid + "&" + Tx_MIPI_cmd + "&" + rxusid +  "&" + Rx_MIPI_cmd;

                    //string s_band =  worksheet.Cells[i, ind_band].Value2;
                    //string s_switch_tx = worksheet.Cells[i, ind_switch_tx].Value2;
                    //string s_switch_ant = worksheet.Cells[i, ind_switch_ant].Value2;
                    //string s_switch_rx = worksheet.Cells[i, ind_switch_rx].Value2;
                    //string s_testmode = worksheet.Cells[i, ind_testmode].Value2;
                    //string s_powermode = worksheet.Cells[i, ind_powermode].Value2;
                    //string s_trx = worksheet.Cells[i, ind_trx].Value2;

                    //if(!mipi_dic.ContainsKey(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + "," + s_powermode + "," + s_trx))
                    //{
                    //    mipi_dic.Add(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + ","  + s_powermode + "," + s_trx, i);
                    //    mipi_dic_rx.Add(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + "," + s_powermode + "," + s_trx, i);
                    //}
                    
                }
                int totalProgress = (int)((double)i / ctype_range * 100);
                UpdateProgress(totalProgress);                
            }

            //if(tcfsplit.FindRow(ind_testmode, worksheet.UsedRange, "DC") != 0) //Only DC Existing Case
            //{
            //    int dc_row = worksheet.UsedRange.Find("DC", Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, false, Missing.Value).Row;

            //    for (int j = 3; j <= dc_row; j++)
            //    {
            //        string s_testmode = worksheet.Cells[j, ind_testmode].Value2;
            //        if (s_testmode == "DC")
            //        {
            //            if (mipi_dic.ContainsKey(worksheet.Cells[j, ind_band].Value2 + "," + worksheet.Cells[j, ind_switch_tx].Value2 + "," + worksheet.Cells[j, ind_switch_ant].Value2 + "," + worksheet.Cells[j, ind_switch_rx].Value2 + "," + worksheet.Cells[j, ind_powermode].Value2 + "," + worksheet.Cells[j, ind_trx].Value2))
            //            {
            //                string Tx_DC_MIPI_cmd = MakeDCMIPICommand(false, xmltuplelistTx, mipi_dic[worksheet.Cells[j, ind_band].Value2 + "," + worksheet.Cells[j, ind_switch_tx].Value2 + "," + worksheet.Cells[j, ind_switch_ant].Value2 + "," + worksheet.Cells[j, ind_switch_rx].Value2 + "," + worksheet.Cells[j, ind_powermode].Value2 + "," + worksheet.Cells[j, ind_trx].Value2]);
            //                string Rx_DC_MIPI_cmd = MakeDCMIPICommand(true, xmltuplelistRx, mipi_dic[worksheet.Cells[j, ind_band].Value2 + "," + worksheet.Cells[j, ind_switch_tx].Value2 + "," + worksheet.Cells[j, ind_switch_ant].Value2 + "," + worksheet.Cells[j, ind_switch_rx].Value2 + "," + worksheet.Cells[j, ind_powermode].Value2 + "," + worksheet.Cells[j, ind_trx].Value2]);

            //                worksheet.Cells[j, ind_MIPICommands].Formula = "=" + txusid + "&" + Tx_DC_MIPI_cmd + "&" + rxusid + "&" + Rx_DC_MIPI_cmd;
            //            }
            //        }
            //    }
            //}            
            string mipiokpath = filepath.Substring(0,filepath.LastIndexOf('.'));
            workbook.SaveAs(mipiokpath + "_mipi_completed" + ".xlsx");
            UpdateProgress(100);
        }
        //Overloading
        public void ParseMIPIcmd(XmlNode xmlnode, XMLParmameter xmlparam, string filepath, Struct_xlsx mipi_xlsx)
        {
            TCFManager tcfsplit = new TCFManager();           
            Workbook workbook = mipi_xlsx.workbook;
            Worksheet worksheet = workbook.Worksheets.get_Item("Condition_PA");                    

            Dictionary<string, int> mipi_dic = new Dictionary<string, int>();
            Dictionary<string, int> mipi_dic_rx = new Dictionary<string, int>();

            XmlNodeList childnode = xmlnode.ChildNodes;
            txusid = xmlparam.TXUSID;
            rxusid = xmlparam.RXUSID;
            prefix = xmlparam.PreFix;
            txtriggermask = xmlparam.TxTriggerMask;
            rxtriggermask = xmlparam.RxTriggerMask;
            txregister = xmlparam.TxRegister;
            rxregister = xmlparam.RxRegister;
            txdaq = xmlparam.TxDAQ;
            rxlna = xmlparam.RxLNA;
            xmltuplelistTx = ReadFromXML3("TxRegister", txregister);
            xmltuplelistRx = ReadFromXML3("RxRegister", rxregister);
            xmltuplelistTxDAQ = ReadFromXML2("TxDAQ", txdaq);
            xmltuplelistRxLNA = ReadFromXML2("RxLNA", rxlna);

            int ind_exf = TCFManager.ColumnIndexDic["Mipi_Compliance"];
            int ind_MIPICommands = TCFManager.ColumnIndexDic["MIPI Commands"];
            int index_extractfolder = TCFManager.ColumnIndexDic["Extract folder"];
            //int ind_TXDAQ1 = TCFManager.ColumnIndexDic["TXDAQ1"];
            //int ind_TXDAQ2 = TCFManager.ColumnIndexDic["TXDAQ2"];
            //int ind_RXLNA1 = TCFManager.ColumnIndexDic["RXLNA1"];
            //int ind_RXLNA2 = TCFManager.ColumnIndexDic["RXLNA2"];
            //int ind_RXLNA3 = TCFManager.ColumnIndexDic["RXLNA3"];
            //int ind_RXLNA4 = TCFManager.ColumnIndexDic["RXLNA4"];
            //int ind_RXLNA5 = TCFManager.ColumnIndexDic["RXLNA5"];


            //int ind_band = TCFManager.ColumnIndexDic["BAND"];
            //int ind_switch_tx = TCFManager.ColumnIndexDic["Switch_TX"];
            //int ind_switch_ant = TCFManager.ColumnIndexDic["Switch_ANT"];
            //int ind_switch_rx = TCFManager.ColumnIndexDic["Switch_RX"];
            //int ind_testmode = TCFManager.ColumnIndexDic["Test Mode"];
            //int ind_powermode = TCFManager.ColumnIndexDic["Power_Mode"];
            //int ind_trx = TCFManager.ColumnIndexDic["TRX"];



            for (int i = 3; i <= worksheet.UsedRange.Rows.Count; i++)
            {
                string string_Ctype = worksheet.Cells[i, ind_exf].Value2;
                string string_extract = worksheet.Cells[i, index_extractfolder].Value2;
                if (string_Ctype != null && string_extract !="C_Prior" && string_extract != "C_Post")
                {
                    string Tx_MIPI_cmd = MakeMIPICommand(false, string_Ctype, xmltuplelistTx, i);
                    string Rx_MIPI_cmd = MakeMIPICommand(true, string_Ctype, xmltuplelistRx, i);
                    
                    worksheet.Cells[i, ind_MIPICommands].Formula = "=" + txusid + "&" + Tx_MIPI_cmd + "&" + rxusid + "&" + Rx_MIPI_cmd;
                    //foreach(Tuple<string,string> item in xmltuplelistTxDAQ)
                    //{
                    //    int daq_ind = TCFManager.ColumnIndexDic[item.Item2]; 
                    //    worksheet.Cells[i, daq_ind].Formula = "=" + item.Item1 + i.ToString();
                    //}
                    //foreach (Tuple<string, string> itemRx in xmltuplelistRxDAQ)
                    //{
                    //    int lna_ind = TCFManager.ColumnIndexDic[itemRx.Item2];
                    //    worksheet.Cells[i, lna_ind].Formula = "=" + itemRx.Item1 + i.ToString();
                    //}

                    //string s_band = worksheet.Cells[i, ind_band].Value2;
                    //string s_switch_tx = worksheet.Cells[i, ind_switch_tx].Value2;
                    //string s_switch_ant = worksheet.Cells[i, ind_switch_ant].Value2;
                    //string s_switch_rx = worksheet.Cells[i, ind_switch_rx].Value2;
                    //string s_testmode = worksheet.Cells[i, ind_testmode].Value2;
                    //string s_powermode = worksheet.Cells[i, ind_powermode].Value2;
                    //string s_trx = worksheet.Cells[i, ind_trx].Value2;

                    //if (worksheet.Cells[i, index_extractfolder].Value2 != "C_Prior" && !mipi_dic.ContainsKey(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + "," + s_powermode + "," + s_trx))
                    //{
                    //    mipi_dic.Add(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + "," + s_powermode + "," + s_trx, i);
                    //    mipi_dic_rx.Add(s_band + "," + s_switch_tx + "," + s_switch_ant + "," + s_switch_rx + "," + s_powermode + "," + s_trx, i);
                    //}
                }
                int totalProgress = (int)((double)i/ worksheet.UsedRange.Rows.Count * 100);
                UpdateProgress(totalProgress);
            }

            Range daq_range_target = worksheet.Range[worksheet.Cells[3, xmltuplelistTxDAQ[0].Item1], worksheet.Cells[worksheet.UsedRange.Rows.Count, xmltuplelistTxDAQ[xmltuplelistTxDAQ.Count - 1].Item1]];
            Range daq_range_dest = worksheet.Range[worksheet.Cells[3, TCFManager.ColumnIndexDic[xmltuplelistTxDAQ[0].Item2]], worksheet.Cells[worksheet.UsedRange.Rows.Count, TCFManager.ColumnIndexDic[xmltuplelistTxDAQ[xmltuplelistTxDAQ.Count - 1].Item2]]];
            daq_range_target.Copy(daq_range_dest);

            Range lna_range_target = worksheet.Range[worksheet.Cells[3, xmltuplelistRxLNA[0].Item1], worksheet.Cells[worksheet.UsedRange.Rows.Count, xmltuplelistRxLNA[xmltuplelistRxLNA.Count - 1].Item1]];
            Range lna_range_dest = worksheet.Range[worksheet.Cells[3, TCFManager.ColumnIndexDic[xmltuplelistRxLNA[0].Item2]], worksheet.Cells[worksheet.UsedRange.Rows.Count, TCFManager.ColumnIndexDic[xmltuplelistRxLNA[xmltuplelistRxLNA.Count - 1].Item2]]];
            lna_range_target.Copy(lna_range_dest);


            string mipiokpath = filepath.Substring(0, filepath.LastIndexOf('.'));
            workbook.SaveAs(mipiokpath + "_mipi_completed" + ".xlsx");
            UpdateProgress(100);
        }

        public List<Tuple<string, string, string>> ReadFromXML3(string nodename, XmlNode xmlnode)
        {
            List<Tuple<string, string, string>> xmltuplelist = new List<Tuple<string, string, string>>();
           if (xmlnode.Name == nodename)
            {
                for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                {
                    XmlNode nodeitem = xmlnode.ChildNodes.Item(i);
                    string[] split_str = nodeitem.InnerText.Split(',');
                    xmltuplelist.Add(new Tuple<string, string, string>(nodeitem.LocalName, split_str[0], split_str[1]));
                }
           }            
            return xmltuplelist;
        }

        public List<Tuple<string, string>> ReadFromXML2(string nodename, XmlNode xmlnode)
        {
            List<Tuple<string, string>> xmltuplelist = new List<Tuple<string, string>>();
            if (xmlnode.Name == nodename)
            {
                for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                {
                    XmlNode nodeitem = xmlnode.ChildNodes.Item(i);
                    string split_str  = nodeitem.InnerText;
                    xmltuplelist.Add(new Tuple<string, string>(nodeitem.LocalName, split_str));
                }
            }
            return xmltuplelist;
        }

        public string MakeMIPICommand(bool isRx, string substr, List<Tuple<string, string, string>> tuplelist,  int j)
        {
            StringBuilder sb = new System.Text.StringBuilder();

            switch(substr)
            {
                case "R_PT_M":
                    sb.Append(prefix);
                    for (int i = 0; i < tuplelist.Count(); i++)
                    {
                        if (tuplelist[i].Item3 == "1")
                        {
                            sb.Append("&" + "\"(0x@\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                        }
                        else
                        {
                            sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                        }
                    }
                    break;
                case "R_PT_N":                    
                    sb.Append(prefix);
                    for (int i = 0; i < tuplelist.Count(); i++)
                    {
                        sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }
                    break;
                case "R_TM_M":
                    if (isRx)
                    {
                        sb.Append(prefix + "&" + rxtriggermask);
                    }
                    else
                    {
                        sb.Append(prefix + "&" + txtriggermask);
                    }
                    for (int i = 0; i < tuplelist.Count() - 2; i++)
                    {
                        if (tuplelist[i].Item3 == "1")
                        {
                            sb.Append("&" + "\"(0x@\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                        }
                        else
                        {
                            sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                        }
                    }
                    break;
                case "R_TM_N":
                    if (isRx)
                    {
                        sb.Append(prefix + "&" + rxtriggermask);
                    }
                    else
                    {
                        sb.Append(prefix + "&" + txtriggermask);
                    }
                    for (int i = 0; i < tuplelist.Count() - 2; i++)
                    {
                        sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
                    }
                    break;
            }           
            string returnstring = sb.ToString();
            return returnstring;
        }

        public void MakeDAQ(bool isRx, List<Tuple<string, string>> tuplelist, int j)
        {
            for (int i = 0; i < tuplelist.Count(); i++)
            {
                
            }
        }

        public string MakeDCMIPICommand(bool isRx, List<Tuple<string, string, string>> tuplelist, int j)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            if (isRx)
            {
                sb.Append(prefix + "&" + rxtriggermask);
            }
            else
            {
                sb.Append(prefix + "&" + txtriggermask);
            }
            for (int i = 0; i < tuplelist.Count() - 2; i++)
            {
                sb.Append("&" + "\"(0x\"&" + tuplelist[i].Item1 + "$2&\",0x\"&" + tuplelist[i].Item1 + "$" + j.ToString() + "&\")\"");
            }   
            string returnstring = sb.ToString();
            return returnstring;
        }

        public void MakeNewDCCommand(Dictionary<string, int> history, Struct_xlsx mipi_xlsx, List<Tuple<string, string, string>> tuplelistTx, List<Tuple<string, string, string>> tuplelistRx)
        {

            TCFManager tcfsplit = new TCFManager();
            Workbook workbook = mipi_xlsx.workbook;
            Worksheet worksheet = workbook.Worksheets.get_Item("Condition_PA");

            int dc_start_row = worksheet.UsedRange.Find("DC", Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlNext, false, false, Missing.Value).Row;
            int dc_last_row = worksheet.UsedRange.Find("DC", Missing.Value, XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious, false, false, Missing.Value).Row;

            int ind_band = TCFManager.ColumnIndexDic["BAND"];
            int ind_switch_tx = TCFManager.ColumnIndexDic["Switch_TX"];
            int ind_switch_ant = TCFManager.ColumnIndexDic["Switch_ANT"];
            int ind_switch_rx = TCFManager.ColumnIndexDic["Switch_RX"];
            int ind_testmode = TCFManager.ColumnIndexDic["Test Mode"];
            int ind_powermode = TCFManager.ColumnIndexDic["Power_Mode"];
            int ind_trx = TCFManager.ColumnIndexDic["TRX"];     

            for (int j = dc_start_row; j <= dc_last_row; j++)
            {
                string s_testmode = worksheet.Cells[j, ind_testmode].Value2;
                if (s_testmode == "DC")
                {
                    if (history.ContainsKey(worksheet.Cells[j, ind_band].Value2 + "," + worksheet.Cells[j, ind_switch_tx].Value2 + "," + worksheet.Cells[j, ind_switch_ant].Value2 + "," + worksheet.Cells[j, ind_switch_rx].Value2 + "," + worksheet.Cells[j, ind_powermode].Value2 + "," + worksheet.Cells[j, ind_trx].Value2))
                    {
                        int cur_index = history[worksheet.Cells[j, ind_band].Value2 + "," + worksheet.Cells[j, ind_switch_tx].Value2 + "," + worksheet.Cells[j, ind_switch_ant].Value2 + "," + worksheet.Cells[j, ind_switch_rx].Value2 + "," + worksheet.Cells[j, ind_powermode].Value2 + "," + worksheet.Cells[j, ind_trx].Value2];
                        for (int i = 0; i < tuplelistTx.Count; i++)
                        {
                            string aa = worksheet.Cells[cur_index, tuplelistTx[i].Item1].Text.ToString();
                            //worksheet.Cells[j, tuplelistTx[i].Item1].Value = "HI";
                            worksheet.Cells[j, tuplelistTx[i].Item1].Value = worksheet.Cells[cur_index, tuplelistTx[i].Item1].Text.ToString();
                        }
                        
                    }
                }
            }           

        }

    }
}
