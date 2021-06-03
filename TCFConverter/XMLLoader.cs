using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCFConverter
{
    class XMLLoader
    {
        public XmlDocument LoadingXml(string path)
        {
            XmlDocument configxml = new XmlDocument();
            configxml.Load(path);
            return configxml;
        }
        
    }
}
