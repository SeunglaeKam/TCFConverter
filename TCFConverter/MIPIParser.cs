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
        //Common XML Configuration Settings
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
        List<string> txregister;
        List<string> rxregister;
        List<string> txdaq;
        List<string> rxlna;

        public void ParseMIPIcmd(XMLParmameter xmlparam,  string filepath, out uint proid)
        {           
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();  
            Workbook workbook = application.Workbooks.Open(Filename: @filepath);
            Worksheet worksheet = workbook.Worksheets.get_Item(1);
            TCFManager tcfManager = new TCFManager();
            Dictionary<string, int> MIPIOnlyDic = new Dictionary<string, int>();


            if (workbook.Worksheets.Count != 1)
            {
                worksheet = workbook.Worksheets.get_Item("Condition_PA");
            }

            GetWindowThreadProcessId(new IntPtr(application.Hwnd), out proid);

            Struct_xlsx parsemipi_xlsx = new Struct_xlsx();
            parsemipi_xlsx.workbook = workbook;
            parsemipi_xlsx.worksheet = worksheet;
            parsemipi_xlsx.range = worksheet.UsedRange;

            Dictionary<string, int> mipi_dic = new Dictionary<string, int>();
            Dictionary<string, int> mipi_dic_rx = new Dictionary<string, int>();
            
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

            MIPIOnlyDic = tcfManager.MakeIndexDictionary(parsemipi_xlsx);
            
            //MIPI Combination
            NewMakeMIPICommand(parsemipi_xlsx, MIPIOnlyDic, out Dictionary<string, int> mipi_dict, out Dictionary<string, int> HPME_dict);
            CopyDCRegister(parsemipi_xlsx, MIPIOnlyDic, mipi_dict, HPME_dict); // HPME Value Copy to DC 
            CopyDCMIPI(parsemipi_xlsx, MIPIOnlyDic);
            CopyVCC2(parsemipi_xlsx, MIPIOnlyDic);
            NewDCMakeMIPICommand(parsemipi_xlsx, MIPIOnlyDic);

            string mipiokpath = filepath.Substring(0,filepath.LastIndexOf('.'));
            workbook.SaveAs(mipiokpath + "_mipi_completed" + ".xlsx");
            UpdateProgress(100);
        }        
        public void NewParseMIPIcmd(XMLParmameter xmlparam, string filepath, Struct_xlsx mipi_xlsx, Dictionary<string, int> IndexDic)
        {            
            Workbook workbook = mipi_xlsx.workbook;
            Worksheet worksheet = workbook.Worksheets.get_Item("Condition_PA");

            Dictionary<string, int> mipi_dic = new Dictionary<string, int>();
            Dictionary<string, int> mipi_dic_rx = new Dictionary<string, int>();

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

            //MIPI Combination only RnD Part. Not DC, TIMING, RF Fixed Pin(Not C_Prior)
            NewMakeMIPICommand(mipi_xlsx, IndexDic,  out Dictionary<string, int> mipi_dict, out Dictionary<string, int> HPME_dict);            
            CopyDCRegister(mipi_xlsx, IndexDic, mipi_dict, HPME_dict); // HPME Value Copy to DC 
            CopyDCMIPI(mipi_xlsx, IndexDic);
            CopyVCC2(mipi_xlsx, IndexDic);
            NewDCMakeMIPICommand(mipi_xlsx, IndexDic);

            //Filter added. 
            int mergedcolumncnt = mipi_xlsx.range.Columns.Count;
            Microsoft.Office.Interop.Excel.Range filterrange = mipi_xlsx.worksheet.Range[mipi_xlsx.worksheet.Cells[2, 1], mipi_xlsx.worksheet.Cells[2, mergedcolumncnt]];
            filterrange.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

            string mipiokpath = filepath.Substring(0, filepath.LastIndexOf('.'));
            workbook.SaveAs(mipiokpath + "_mipi_completed" + ".xlsx");
            UpdateProgress(100);
        }        
        public void NewMakeMIPICommand(Struct_xlsx mipi_xlsx, Dictionary<string, int> MIPIIndexDic , out Dictionary<string, int> MIPI_Dict, out Dictionary<string, int> HPME_Dict)
        {            
            Worksheet worksheet = mipi_xlsx.worksheet;

            List<Range> FindDCRangeList = new List<Range>();
            List<Range> TxMipiRangeList = new List<Range>();
            List<Range> RxMipiRangeList = new List<Range>();
            List<Range> ComplianceRangeList = new List<Range>();

            List<object[,]> FindDCObjectList = new List<object[,]>();
            List<object[,]> TxMipiObjectList = new List<object[,]>();            
            List<object[,]> RxMipiObjectList = new List<object[,]>();
            List<object[,]> ComplianceObjectList = new List<object[,]>();

            List<int> Find_DC_List = new List<int>();

            MIPI_Dict = new Dictionary<string, int>();
            HPME_Dict = new Dictionary<string, int>();

            //Load Index From TCF File
            int ind_comp = MIPIIndexDic["Mipi_Compliance"];
            int ind_MIPImap = MIPIIndexDic["Mipi_Mapped"];
            int index_extractfolder = MIPIIndexDic["Extract folder"];

            int ind_MIPICommands = MIPIIndexDic["MIPI Commands"];
            //Find for DC 
            int ind_powermode = MIPIIndexDic["Power_Mode"];
            int ind_band = MIPIIndexDic["BAND"];
            int ind_switch_tx = MIPIIndexDic["Switch_TX"];
            int ind_switch_ant = MIPIIndexDic["Switch_ANT"];
            int ind_switch_rx = MIPIIndexDic["Switch_RX"];
            int ind_testmode = MIPIIndexDic["Test Mode"];
            int ind_trx = MIPIIndexDic["TRX"];           

            //Tx Register Range Setting 
            foreach (Tuple<string,string,string> tuple in xmltuplelistTx)
            {
                Range temp_tx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                TxMipiRangeList.Add(temp_tx_mipi_range);                
                TxMipiObjectList.Add(temp_tx_mipi_range.get_Value());
            }
            //Rx Register Range Setting 
            foreach (Tuple<string, string, string> tuple in xmltuplelistRx)
            {
                Range temp_rx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                RxMipiRangeList.Add(temp_rx_mipi_range);
                RxMipiObjectList.Add(temp_rx_mipi_range.get_Value());
            }

            //Compliance Range Setting
            Range extract_range = worksheet.Range[worksheet.Cells[2, index_extractfolder], worksheet.Cells[mipi_xlsx.range.Rows.Count, index_extractfolder]];
            Range mipimap_range = worksheet.Range[worksheet.Cells[2, ind_MIPImap], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_MIPImap]];
            Range compliance_range = worksheet.Range[worksheet.Cells[2, ind_comp], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_comp]];           
            
            ComplianceRangeList.Add(compliance_range);
            ComplianceRangeList.Add(mipimap_range);
            ComplianceRangeList.Add(extract_range);

            ComplianceObjectList.Add(compliance_range.get_Value());
            ComplianceObjectList.Add(mipimap_range.get_Value());
            ComplianceObjectList.Add(extract_range.get_Value());
            
            //Band Range Setting. To Find Condition on DC Test. 
            Range band_range = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_powermode]];
            object[,] band_rng_obj = band_range.get_Value();            
          
            //MIPI CMD Range Setting 
            Range mipi_cmd_range = worksheet.Range[worksheet.Cells[2, ind_MIPICommands], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_MIPICommands]];
            object[,] mipi_cmd_obj = mipi_cmd_range.get_Value();
            

            #region MIPI Command Combiation
            for (int r = 2; r < mipi_xlsx.range.Rows.Count; r++)
            {             
                StringBuilder sb2 = new StringBuilder();
                StringBuilder dic_sb = new StringBuilder();
                StringBuilder HPME_sb = new StringBuilder();
                if (ComplianceObjectList[0][r, 1] != null && ComplianceObjectList[2][r, 1] != null)
                {
                    if (ComplianceObjectList[2][r, 1].ToString() != "C_Prior" && ComplianceObjectList[2][r, 1].ToString() != "C_Post")  // Only RnD
                    {
                        if (ComplianceObjectList[0][r, 1].ToString() == "R_PT_N")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistTx.Count; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {                                   
                                    sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistRx.Count; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {                                   
                                    sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_PT_M")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistTx.Count; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistTx[c-1].Item3 == "1")
                                    {                                        
                                        sb2.Append("(" + "0x@" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {                                        
                                        sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistRx.Count; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistRx[c - 1].Item3 == "1")
                                    {                                        
                                        sb2.Append("(" + "0x@" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {                                      
                                        sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }                                    
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_TM_N")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            sb2.Append(txtriggermask);
                            for (int c = 1; c <= xmltuplelistTx.Count - 2; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {                                   
                                    sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            sb2.Append(rxtriggermask);
                            for (int c = 1; c <= xmltuplelistRx.Count - 2; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {                                   
                                    sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }                                                                   
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_TM_M")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            sb2.Append(txtriggermask);
                            for (int c = 1; c <= xmltuplelistTx.Count - 2; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistTx[c - 1].Item3 == "1")
                                    {                                       
                                        sb2.Append("(" + "0x@" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {                                        
                                        sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            sb2.Append(rxtriggermask);
                            for (int c = 1; c <= xmltuplelistRx.Count - 2; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistRx[c - 1].Item3 == "1")
                                    {                                        
                                        sb2.Append("(" + "0x@" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {                                       
                                        sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }                                    
                            }
                        }                         
                        dic_sb.Append(band_rng_obj[r, ind_band]?.ToString());
                        dic_sb.Append(band_rng_obj[r, ind_switch_tx]?.ToString());
                        dic_sb.Append(band_rng_obj[r, ind_switch_ant]?.ToString());
                        dic_sb.Append(band_rng_obj[r, ind_switch_rx]?.ToString());
                        dic_sb.Append(band_rng_obj[r, ind_trx]?.ToString());
                        dic_sb.Append(band_rng_obj[r, ind_powermode]?.ToString());

                    if(band_rng_obj[r, ind_powermode]?.ToString() == "HPME")
                        {
                            HPME_sb.Append(band_rng_obj[r, ind_band]?.ToString());
                            HPME_sb.Append(band_rng_obj[r, ind_powermode]?.ToString());
                        }                        
                    }                    
                }
                if (!MIPI_Dict.ContainsKey(dic_sb.ToString()))
                {
                    MIPI_Dict.Add(dic_sb.ToString(), r);
                }
                if (!HPME_Dict.ContainsKey(HPME_sb.ToString()))
                {
                    HPME_Dict.Add(HPME_sb.ToString(), r);
                }
                if (sb2.ToString() != "")
                {
                    mipi_cmd_obj[r, 1] = sb2.ToString();
                }
                int totalProgress = (int)((double)r / mipi_xlsx.range.Rows.Count * 100);
                UpdateProgress(totalProgress);
            }
            #endregion

            //MIPI Command Copy to MIPI Command Column
            mipi_cmd_range.Value = mipi_cmd_obj;            
        }
        public void CopyDCMIPI(Struct_xlsx mipi_xlsx, Dictionary<string, int> CopyDic)
        {
            Worksheet worksheet = mipi_xlsx.worksheet;

            List<Range> TxDAQRangeDestList = new List<Range>();
            List<object[,]> TxDAQObjectList = new List<object[,]>();
            
            List<Range> RxLNARangeDestList = new List<Range>();
            List<object[,]> RxLNAObjectList = new List<object[,]>();
            

            foreach (Tuple<string, string> tuple in xmltuplelistTxDAQ)
            {
                Range temp_tx_daq_range = worksheet.Range[worksheet.Cells[3, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                Range temp_tx_daq_range_dest = worksheet.Range[worksheet.Cells[3, CopyDic[tuple.Item2]], worksheet.Cells[mipi_xlsx.range.Rows.Count, CopyDic[tuple.Item2]]];

                TxDAQObjectList.Add(temp_tx_daq_range.get_Value());
                TxDAQRangeDestList.Add(temp_tx_daq_range_dest);
            }
            foreach (Tuple<string, string> tuple in xmltuplelistRxLNA)
            {
                Range temp_rx_lna_range = worksheet.Range[worksheet.Cells[3, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                Range temp_rx_lna_range_dest = worksheet.Range[worksheet.Cells[3, CopyDic[tuple.Item2]], worksheet.Cells[mipi_xlsx.range.Rows.Count, CopyDic[tuple.Item2]]];
                               
                RxLNAObjectList.Add(temp_rx_lna_range.get_Value());
                RxLNARangeDestList.Add(temp_rx_lna_range_dest);
            }

            for(int i = 0; i < xmltuplelistTxDAQ.Count; i++)
            {
                TxDAQRangeDestList[i].Value = TxDAQObjectList[i];
            }
            for (int i = 0; i < xmltuplelistRxLNA.Count; i++)
            {
                RxLNARangeDestList[i].Value = RxLNAObjectList[i];
            }
        }
        public void CopyDCRegister(Struct_xlsx mipi_xlsx, Dictionary<string, int> IndexDic ,  Dictionary<string, int> dc_dic, Dictionary<string, int> hpme_dic)
        {

            Worksheet worksheet = mipi_xlsx.worksheet;

            List<Range> TxDCRangeList = new List<Range>();
            List<Range> RxDCRangeList = new List<Range>();
            List<object[,]> TxDCObjectList = new List<object[,]>();
            List<object[,]> RxDCObjectList = new List<object[,]>();
            
            //Load Index From TCF File
            int ind_comp = IndexDic["Mipi_Compliance"];
            int ind_MIPImap = IndexDic["Mipi_Mapped"];
            int ind_MIPICommands = IndexDic["MIPI Commands"];
            int index_extractfolder = IndexDic["Extract folder"];
            int ind_powermode = IndexDic["Power_Mode"];
            int ind_band = IndexDic["BAND"];
            int ind_switch_tx = IndexDic["Switch_TX"];
            int ind_switch_ant = IndexDic["Switch_ANT"];
            int ind_switch_rx = IndexDic["Switch_RX"];
            int ind_testmode = IndexDic["Test Mode"];
            int ind_trx = IndexDic["TRX"];

            foreach (Tuple<string, string, string> tuple in xmltuplelistTx)
            {
                Range temp_tx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                TxDCRangeList.Add(temp_tx_mipi_range);
                TxDCObjectList.Add(temp_tx_mipi_range.get_Value());
            }
            //Rx Register Range Setting 
            foreach (Tuple<string, string, string> tuple in xmltuplelistRx)
            {
                Range temp_rx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                RxDCRangeList.Add(temp_rx_mipi_range);
                RxDCObjectList.Add(temp_rx_mipi_range.get_Value());
            }


            Range dc_range = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_powermode]];
            //Range tx_mipi_range = worksheet.Range[worksheet.Cells[2, xmltuplelistTx[0].Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, xmltuplelistTx[xmltuplelistTx.Count - 1].Item1]];
            //Range rx_mipi_range = worksheet.Range[worksheet.Cells[2, xmltuplelistRx[0].Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, xmltuplelistRx[xmltuplelistRx.Count - 1].Item1]];

            //Range tx_daq_range = worksheet.Range[worksheet.Cells[2, TCFManager.ColumnIndexDic[xmltuplelistTxDAQ[0].Item2]], worksheet.Cells[mipi_xlsx.range.Rows.Count, TCFManager.ColumnIndexDic[xmltuplelistTxDAQ[xmltuplelistTxDAQ.Count - 1].Item2]]];
            //Range tx_daq_range = worksheet.Range[worksheet.Cells[2, xmltuplelistTxDAQ[0].Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, xmltuplelistTxDAQ[xmltuplelistTxDAQ.Count - 1].Item1]];

            object[,] dc_rng_obj = dc_range.get_Value();
            //object[,] tx_mipi_obj = tx_mipi_range.get_Value();
            //object[,] rx_mipi_obj = rx_mipi_range.get_Value();
            //object[,] tx_daq_obj = tx_daq_range.get_Value();

            for (int r = 2; r < mipi_xlsx.range.Rows.Count; r++)
            {
                StringBuilder dc_sb = new StringBuilder();
                StringBuilder hpme_sb = new StringBuilder();
                if (dc_rng_obj[r, ind_testmode]?.ToString() == "DC" && dc_rng_obj[r, ind_trx]?.ToString() == "RX") //ADD DC Area Condition 
                {
                    dc_sb.Append(dc_rng_obj[r, ind_band]?.ToString());
                    dc_sb.Append(dc_rng_obj[r, ind_switch_tx]?.ToString());
                    dc_sb.Append(dc_rng_obj[r, ind_switch_ant]?.ToString());
                    dc_sb.Append(dc_rng_obj[r, ind_switch_rx]?.ToString());
                    dc_sb.Append(dc_rng_obj[r, ind_trx]?.ToString());
                    dc_sb.Append(dc_rng_obj[r, ind_powermode]?.ToString());

                    //hpme_sb.Append(dc_rng_obj[r, ind_band]?.ToString());
                    //hpme_sb.Append(dc_rng_obj[r, ind_powermode]?.ToString());
                    
                    if (dc_dic.ContainsKey(dc_sb.ToString())) // When DC Condition meets RnD Condition
                    {
                        for (int c = 1; c <= xmltuplelistTx.Count; c++)
                        {
                            TxDCObjectList[c - 1][r, 1] = TxDCObjectList[c - 1][dc_dic[dc_sb.ToString()], 1].ToString();
                            //tx_mipi_obj[r, c] = tx_mipi_obj[dc_dic[dc_sb.ToString()], c]?.ToString();
                        }
                        for (int c = 1; c <= xmltuplelistRx.Count; c++)
                        {
                            RxDCObjectList[c - 1][r, 1] = RxDCObjectList[c - 1][dc_dic[dc_sb.ToString()], 1].ToString();
                            //rx_mipi_obj[r, c] = rx_mipi_obj[dc_dic[dc_sb.ToString()], c]?.ToString();
                        }
                    }
                    //if (hpme_dic.ContainsKey(hpme_sb.ToString()))
                    //{
                    //    for (int c = 1; c <= xmltuplelistTxDAQ.Count; c++)
                    //    {
                    //        tx_daq_obj[r, c] = tx_daq_obj[hpme_dic[hpme_sb.ToString()], c]?.ToString();
                    //    }                        
                    //}
                }                                
            }
            for (int c = 1; c <= xmltuplelistTx.Count; c++)
            {
                TxDCRangeList[c-1].Value =  TxDCObjectList[c - 1];
            }
            for (int c = 1; c <= xmltuplelistRx.Count; c++)
            {
                RxDCRangeList[c - 1].Value = RxDCObjectList[c - 1];
            }
            //rx_mipi_range.Value = rx_mipi_obj;
            //tx_mipi_range.Value = tx_mipi_obj;
            //tx_daq_range.Value = tx_daq_obj;
        }
        public void CopyVCC2(Struct_xlsx mipi_xlsx, Dictionary<string, int> IndexDic)
        {
            int ind_VCC = IndexDic["V.Vcc"];
            int ind_VCC2 = IndexDic["V.Vcc2"];
            Worksheet worksheet = mipi_xlsx.worksheet;

            Range vcc_range = worksheet.Range[worksheet.Cells[3, ind_VCC], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_VCC]];           

            Range vcc2_range = worksheet.Range[worksheet.Cells[3, ind_VCC2], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_VCC2]];
            vcc2_range.Value = vcc_range.get_Value();

        }
        public void NewDCMakeMIPICommand(Struct_xlsx mipi_xlsx, Dictionary<string, int> MIPIIndexDic)
        {
            Worksheet worksheet = mipi_xlsx.worksheet;

            List<Range> TxMipiRangeList = new List<Range>();
            List<Range> RxMipiRangeList = new List<Range>();
            List<Range> ComplianceRangeList = new List<Range>();

            List<object[,]> TxMipiObjectList = new List<object[,]>();
            List<object[,]> RxMipiObjectList = new List<object[,]>();
            List<object[,]> ComplianceObjectList = new List<object[,]>();          

            //Load Index From TCF File
            int ind_comp = MIPIIndexDic["Mipi_Compliance"];
            int ind_MIPImap = MIPIIndexDic["Mipi_Mapped"];
            int ind_MIPICommands = MIPIIndexDic["MIPI Commands"];
            int index_extractfolder = MIPIIndexDic["Extract folder"];
            int ind_powermode = MIPIIndexDic["Power_Mode"];
            int ind_band = MIPIIndexDic["BAND"];
            int ind_switch_tx = MIPIIndexDic["Switch_TX"];
            int ind_switch_ant = MIPIIndexDic["Switch_ANT"];
            int ind_switch_rx = MIPIIndexDic["Switch_RX"];
            int ind_testmode = MIPIIndexDic["Test Mode"];
            int ind_trx = MIPIIndexDic["TRX"];

            //Tx Register Range Setting 
            foreach (Tuple<string, string, string> tuple in xmltuplelistTx)
            {
                Range temp_tx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                TxMipiRangeList.Add(temp_tx_mipi_range);
                TxMipiObjectList.Add(temp_tx_mipi_range.get_Value());
            }
            //Rx Register Range Setting 
            foreach (Tuple<string, string, string> tuple in xmltuplelistRx)
            {
                Range temp_rx_mipi_range = worksheet.Range[worksheet.Cells[2, tuple.Item1], worksheet.Cells[mipi_xlsx.range.Rows.Count, tuple.Item1]];
                RxMipiRangeList.Add(temp_rx_mipi_range);
                RxMipiObjectList.Add(temp_rx_mipi_range.get_Value());
            }

            //Compliance Range Setting
            Range extract_range = worksheet.Range[worksheet.Cells[2, index_extractfolder], worksheet.Cells[mipi_xlsx.range.Rows.Count, index_extractfolder]];
            Range mipimap_range = worksheet.Range[worksheet.Cells[2, ind_MIPImap], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_MIPImap]];
            Range compliance_range = worksheet.Range[worksheet.Cells[2, ind_comp], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_comp]];

            ComplianceRangeList.Add(compliance_range);
            ComplianceRangeList.Add(mipimap_range);
            ComplianceRangeList.Add(extract_range);

            ComplianceObjectList.Add(compliance_range.get_Value());
            ComplianceObjectList.Add(mipimap_range.get_Value());
            ComplianceObjectList.Add(extract_range.get_Value());

            //Band Range Setting. To Find Condition on DC Test. 
            Range band_range = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_powermode]];
            object[,] band_rng_obj = band_range.get_Value();

            //MIPI CMD Range Setting 
            Range mipi_cmd_range = worksheet.Range[worksheet.Cells[2, ind_MIPICommands], worksheet.Cells[mipi_xlsx.range.Rows.Count, ind_MIPICommands]];
            object[,] mipi_cmd_obj = mipi_cmd_range.get_Value();


            #region MIPI Command Combiation
            for (int r = 2; r < mipi_xlsx.range.Rows.Count; r++)
            {
                StringBuilder sb2 = new StringBuilder();              
                if (ComplianceObjectList[0][r, 1] != null && ComplianceObjectList[2][r, 1] != null)
                {
                    if (ComplianceObjectList[2][r, 1].ToString() == "C_Prior" && band_rng_obj[r, ind_trx].ToString() == "RX")  // Only DC
                    {
                        if (ComplianceObjectList[0][r, 1].ToString() == "R_PT_N")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistTx.Count; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistRx.Count; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_PT_M")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistTx.Count; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistTx[c - 1].Item3 == "1")
                                    {
                                        sb2.Append("(" + "0x@" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {
                                        sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            for (int c = 1; c <= xmltuplelistRx.Count; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistRx[c - 1].Item3 == "1")
                                    {
                                        sb2.Append("(" + "0x@" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {
                                        sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_TM_N")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            sb2.Append(txtriggermask);
                            for (int c = 1; c <= xmltuplelistTx.Count - 2; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            sb2.Append(rxtriggermask);
                            for (int c = 1; c <= xmltuplelistRx.Count - 2; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                }
                            }
                        }
                        else if (ComplianceObjectList[0][r, 1].ToString() == "R_TM_M")
                        {
                            sb2.Append(txusid);
                            sb2.Append(prefix);
                            sb2.Append(txtriggermask);
                            for (int c = 1; c <= xmltuplelistTx.Count - 2; c++)
                            {
                                if (TxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistTx[c - 1].Item3 == "1")
                                    {
                                        sb2.Append("(" + "0x@" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {
                                        sb2.Append("(" + "0x" + TxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + TxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                            sb2.Append(rxusid);
                            sb2.Append(prefix);
                            sb2.Append(rxtriggermask);
                            for (int c = 1; c <= xmltuplelistRx.Count - 2; c++)
                            {
                                if (RxMipiObjectList[c - 1][r, 1] != null)
                                {
                                    if (xmltuplelistRx[c - 1].Item3 == "1")
                                    {
                                        sb2.Append("(" + "0x@" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                    else
                                    {
                                        sb2.Append("(" + "0x" + RxMipiObjectList[c - 1][1, 1].ToString() + "," + "0x" + RxMipiObjectList[c - 1][r, 1].ToString() + ")");
                                    }
                                }
                            }
                        }
                       
                       
                    }
                }                
                if (sb2.ToString() != "")
                {
                    mipi_cmd_obj[r, 1] = sb2.ToString();
                }              
            }
            #endregion

            //MIPI Command Copy to MIPI Command Column
            mipi_cmd_range.Value = mipi_cmd_obj;
        }
        public List<Tuple<string, string, string>> ReadFromXML3(string nodename, List<string> xmlnode)
        {
            List<Tuple<string, string, string>> xmltuplelist = new List<Tuple<string, string, string>>();

            for (int i = 0; i < xmlnode.Count; i++)
            {
                string splitcol = xmlnode[i].Split('_')[0];
                string[] split_str = xmlnode[i].Split('_')[1].Split(',');
                xmltuplelist.Add(new Tuple<string, string, string>(splitcol, split_str[0], split_str[1]));
            }

            return xmltuplelist;
        }
        public List<Tuple<string, string>> ReadFromXML2(string nodename, List<string> xmlnode)
        {
            List<Tuple<string, string>> xmltuplelist = new List<Tuple<string, string>>();
            for (int i = 0; i < xmlnode.Count; i++)
            {
                xmltuplelist.Add(new Tuple<string, string>(xmlnode[i].Split(',')[0], xmlnode[i].Split(',')[1]));
            }
            return xmltuplelist;
        }
    }
}
