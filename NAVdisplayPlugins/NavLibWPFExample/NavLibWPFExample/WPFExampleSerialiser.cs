﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NavLibWPFExample
{
    class WPFExampleSerialiser : IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Example", "WPFPLuginExample");
        }
    }
}
