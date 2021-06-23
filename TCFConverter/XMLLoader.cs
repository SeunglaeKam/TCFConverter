using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;
using System.Collections;

namespace TCFConverter
{  
    public class XMLParmameter
    {        
        private string project, product, revision, txusid, rxusid, prefix, txtriggermask, rxtriggermask;        
        private List<string> rxregister, txregister, band, txdaq, rxlna;
        public string Project
        {
            get { return project; }           
            set{ project = value; }            
        }
        public string Product
        {
            get { return product; }
            set { product = value; }
        }
        public string Revision
        {
            get { return revision; }
            set { revision = value; }            
        }
        public string TXUSID
        {
            get { return txusid; }
            set { txusid = value; }           
        }       
        public string RXUSID
        {
            get { return rxusid; }
            set { rxusid = value; }
        }
        public string PreFix
        {
            get { return prefix; }
            set { prefix = value; }
        }
        public string TxTriggerMask
        {
            get { return txtriggermask; }
            set { txtriggermask = value; }
        }
        public string RxTriggerMask
        {
            get { return rxtriggermask; }
            set { rxtriggermask = value; }
        }
        public List<string> TxRegister
        {
            get { return txregister; }
            set { txregister = value; }
        }
        public List<string> RxRegister
        {
            get { return rxregister; }
            set { rxregister = value; }
        }
        public List<string> Band
        {
            get { return band; }
            set { band = value; }
        }
        public List<string> TxDAQ
        {
            get { return txdaq; }
            set { txdaq = value; }
        }
        public List<string> RxLNA
        {
            get { return rxlna; }
            set { rxlna = value; }
        }
    }
    public class XMLLoader : StringConverter
    {      
        internal static List<string> list = new List<string>();
        internal static List<string> rxlist = new List<string>();
        private string _Txreg = "";
        private string _Rxreg = "";

        [Category("Project Name")]
        [DisplayNameAttribute("Project Name")]
        public string Name { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("Project Product")]
        public string Product { get; set; }

        [Category("Revision")]
        [DisplayNameAttribute("Revision")]
        public string Revision { get; set; }

        [Category("TxUSID")]
        [DisplayNameAttribute("TxUSID")]
        public string TxUSID { get; set; }

        [Category("RxUSID")]
        [DisplayNameAttribute("RxUSID")]
        public string RxUSID { get; set; }

        [Category("Prefix")]
        [DisplayNameAttribute("Prefix")]
        public string Prefix { get; set; }

        [Category("TxTriggerMask")]
        [DisplayNameAttribute("TxTriggerMask")]
        public string TriggerMask { get; set; }

        [Category("RxTriggerMask")]
        [DisplayNameAttribute("RxTriggerMask")]
        public string RxTriggerMask { get; set; }

        [Category("TxRegister")]        
        [DisplayNameAttribute("TxRegister")]
        [TypeConverter(typeof(TxConverter))]
        public string TxRegister
        {
            get
            {
                string txreg = "";
                if (_Txreg != null)
                {
                    txreg = _Txreg;
                }
                else
                {
                    if (list.Count > 0)
                    {
                        txreg = list[0];
                    }
                }
                return txreg;
            }
            set { _Txreg = value; }
        } 

        [Category("RxRegister")]       
        [DisplayNameAttribute("RxRegister")]
        [TypeConverter(typeof(RxConverter))]
        public string RxRegister
        {
            get
            {
                string rxreg = "";
                if (_Rxreg != null)
                {
                    rxreg = _Rxreg;
                }
                else
                {
                    if (rxlist.Count > 0)
                    {
                        rxreg = rxlist[0];
                    }
                }
                return rxreg;
            }
            set { _Rxreg = value; }
        }
        public XmlDocument LoadingXml(string path)
        {
            XmlDocument configxml = new XmlDocument();            
            configxml.Load(path);
            return configxml;
        }
        public XMLParmameter XmlDocumentParse(string path)
        {
            XmlDocument configxml = new XmlDocument();
            configxml.Load(path);            
            
            List<string> bandxmllist = new List<string>();
            List<string> daqxmllist = new List<string>();
            List<string> lnaxmllist = new List<string>();

            XmlNodeList xmlList = configxml.SelectNodes("Configs");
            XMLParmameter xmlpara = new XMLParmameter();

            foreach (XmlNode nodes in xmlList[0].ChildNodes)
            {
                if (nodes.Attributes[0].InnerText == "Project")
                {
                    xmlpara.Project = nodes.Attributes[1].InnerText;
                    Name = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "revision")
                {
                    xmlpara.Revision = nodes.Attributes[1].InnerText;
                    Revision = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "TxUSID")
                {
                    xmlpara.TXUSID = nodes.Attributes[1].InnerText;
                    TxUSID = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "RxUSID")
                {
                    xmlpara.RXUSID = nodes.Attributes[1].InnerText;
                    RxUSID = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "Prefix")
                {
                    xmlpara.PreFix = nodes.Attributes[1].InnerText;
                    Prefix = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "TxTriggerMask")
                {
                    xmlpara.TxTriggerMask = nodes.Attributes[1].InnerText;
                    TriggerMask = nodes.Attributes[1].InnerText;
                }
                else if (nodes.Attributes[0].InnerText == "RxTriggerMask")
                {
                    xmlpara.RxTriggerMask = nodes.Attributes[1].InnerText;
                    RxTriggerMask = nodes.Attributes[1].InnerText;                    
                }
                else if (nodes.Name == "TxRegister")
                {
                    list.Add(nodes.Attributes[0].InnerText + "_" + nodes.Attributes[1].InnerText + "," + nodes.Attributes[2].InnerText);
                    xmlpara.TxRegister = list;
                }
                else if (nodes.Name == "RxRegister")
                {
                    rxlist.Add(nodes.Attributes[0].InnerText + "_" + nodes.Attributes[1].InnerText + "," + nodes.Attributes[2].InnerText);
                    xmlpara.RxRegister = rxlist;
                }
                else if (nodes.Name == "BAND")
                {
                    bandxmllist.Add(nodes.Attributes[1].InnerText);
                    xmlpara.Band = bandxmllist;
                }
                else if (nodes.Name == "TxDAQ")
                {
                    daqxmllist.Add(nodes.Attributes[0].InnerText + "," + nodes.Attributes[1].InnerText);
                    xmlpara.TxDAQ = daqxmllist;
                }
                else if (nodes.Name == "RxLNA")
                {
                    lnaxmllist.Add(nodes.Attributes[0].InnerText + "," + nodes.Attributes[1].InnerText);
                    xmlpara.RxLNA = lnaxmllist;
                }
            }
            return xmlpara;
        }       
    }
    public class TxConverter : StringConverter
    {
        XMLLoader xloader = new XMLLoader();
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return false; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {            
            return new StandardValuesCollection(XMLLoader.list);
        }
    }
    public class RxConverter : StringConverter
    {
        XMLLoader xloader = new XMLLoader();
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return false; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {

            return new StandardValuesCollection(XMLLoader.rxlist);
        }
    }
}
