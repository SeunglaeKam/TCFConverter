using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;

namespace TCFConverter
{
    public class XMLParmameter
    {
        private string project, product, revision, txusid, rxusid, prefix, txtriggermask, rxtriggermask;
        private XmlNode txregister, rxregister, band, txdaq, rxlna;
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
        public XmlNode TxRegister
        {
            get { return txregister; }
            set { txregister = value; }
        }
        public XmlNode RxRegister
        {
            get { return rxregister; }
            set { rxregister = value; }
        }
        public XmlNode Band
        {
            get { return band; }
            set { band = value; }
        }
        public XmlNode TxDAQ
        {
            get { return txdaq; }
            set { txdaq = value; }
        }
        public XmlNode RxLNA
        {
            get { return rxlna; }
            set { rxlna = value; }
        }

    }
    class XMLLoader
    {

        [Category("Project")]
        [DisplayNameAttribute("Project Name")]
        public string Name { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("Project Product")]
        public string Product { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("Revision")]
        public string Revision { get; set; }

        [Category("TxUSID")]
        [DisplayNameAttribute("TxUSID")]
        public string TxUSID { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("RxUSID")]
        public string RxUSID { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("Prefix")]
        public string Prefix { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("TxTriggerMask")]
        public string TriggerMask { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("RxTriggerMask")]
        public string RxTriggerMask { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("TxRegister")]
        public string TxRegister { get; set; }

        [Category("Project")]
        [DisplayNameAttribute("RxRegister")]
        public string RxRegister { get; set; }


        public XmlDocument LoadingXml(string path)
        {
            XmlDocument configxml = new XmlDocument();
            configxml.Load(path);
            return configxml;
        }

        public XMLParmameter ParsingXML(string path)
        {
            XmlDocument configxml = new XmlDocument();           
            configxml.Load(path);
            XMLParmameter xmlpara = new XMLParmameter();
            foreach (XmlNode node in configxml.GetElementsByTagName("Project").Item(0))
            {
                if (node.Name == "name")
                {
                    Name = node.InnerText;
                    xmlpara.Project = node.InnerText;
                }
                else if (node.Name == "product")
                {
                    Product = node.InnerText;
                    xmlpara.Product = node.InnerText;
                }
                else if (node.Name == "revision")
                {
                    Revision = node.InnerText;
                    xmlpara.Revision = node.InnerText;
                }
                else if (node.Name == "TxUSID")
                {
                    TxUSID = node.InnerText;
                    xmlpara.TXUSID = node.InnerText;
                }
                else if (node.Name == "RxUSID")
                {
                    RxUSID = node.InnerText;
                    xmlpara.RXUSID = node.InnerText;
                }
                else if (node.Name == "Prefix")
                {
                    Prefix = node.InnerText;
                    xmlpara.PreFix = node.InnerText;
                }
                else if (node.Name == "TxTriggerMask")
                {
                    TriggerMask = node.InnerText;
                    xmlpara.TxTriggerMask = node.InnerText;
                }
                else if (node.Name == "RxTriggerMask")
                {
                    RxTriggerMask = node.InnerText;
                    xmlpara.RxTriggerMask = node.InnerText;
                }
                else if (node.Name == "TxRegister")
                {
                    TxRegister = node.InnerXml;
                    xmlpara.TxRegister = node;
                }
                else if (node.Name == "RxRegister")
                {
                    RxRegister = node.InnerXml;
                    xmlpara.RxRegister = node;
                }
                else if (node.Name == "Band")
                {                    
                    xmlpara.Band = node;
                }
                else if (node.Name == "TxDAQ")
                {
                    xmlpara.TxDAQ = node;
                }
                else if (node.Name == "RxLNA")
                {
                    xmlpara.RxLNA = node;
                }
            }
            return xmlpara;
        }
        
    }
}
