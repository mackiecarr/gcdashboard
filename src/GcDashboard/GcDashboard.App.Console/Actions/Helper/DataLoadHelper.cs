using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System.Xml;
using GcDashboard.Core.Entities;

namespace GcDashboard.App.AdminConsole.Actions.Helper
{

    internal static class DataLoadHelper
    {

        #region [ Fields (2) ]

        private static XmlDocument _xmlDocument = null;
        private static XmlNamespaceManager _namespaceManager = null;

        #endregion [ Fields ]

        #region [ Private Static Methods (5) ]

        internal static void Extract(string filename)
        {
            using (Stream s = new GZipInputStream(File.OpenRead(filename)))
            using (FileStream fs = File.Create(filename + ".work"))
            {
                StreamUtils.Copy(s, fs, new byte[4096]);
            }
        }

        internal static XmlNodeList GetAccountNodes()
        {
            return GetNodes("gnc-v2/gnc:book/gnc:account");
        }

        internal static XmlNodeList GetTransactionNodes()
        {
            return GetNodes("gnc-v2/gnc:book/gnc:transaction");
        }

        internal static XmlNodeList GetNodes(string xpath)
        {
            return _xmlDocument.SelectNodes(xpath, _namespaceManager);
        }

        internal static XmlDocument ReadXml(string filename)
        {
            StreamReader rdr = new StreamReader(filename);
            string xml = rdr.ReadToEnd();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            _namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (XmlAttribute xmlAttrib in xmlDoc.DocumentElement.Attributes)
            {
                _namespaceManager.AddNamespace(xmlAttrib.LocalName, xmlAttrib.Value);
            }

            _xmlDocument = xmlDoc;

            return xmlDoc;
        }

        #endregion [ Private Static Methods ]

    }
}


