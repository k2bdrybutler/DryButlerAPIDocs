using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace K2Facade
{
    public class XmlTools
    {
        public static bool CreateXml(string XmlFile, string RootElement)
        {
            try
            {
                var klasor = Tools.RootFolder + "\\" + Tools.CompanyName;
                if (!Directory.Exists(klasor)) Directory.CreateDirectory(klasor);
                if (!System.IO.File.Exists(XmlFile))
                {
                    var xw = new XmlTextWriter(XmlFile, Encoding.UTF8);
                    xw.Formatting = Formatting.Indented;
                    xw.WriteStartDocument();
                    xw.WriteStartElement(RootElement);
                    xw.WriteEndElement();
                    xw.WriteEndDocument();
                    xw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(string XmlFile, string RootElement, string ElementName, object eklenecek, bool sifrele)
        {
            try
            {
                CreateXml(XmlFile, RootElement);
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode(RootElement);
                var sfr = xdoc.CreateAttribute("PasswordProtected");
                sfr.Value = sifrele.ToString();
                root.Attributes.Append(sfr);
                XmlNode yeni = xdoc.CreateElement(ElementName);
                foreach (var item in eklenecek.GetType().GetProperties())
                {
                    var s1 = xdoc.CreateNode(XmlNodeType.Element, item.Name, null);
                    var s11 = xdoc.CreateNode(XmlNodeType.CDATA, null, null);
                    var d1 = eklenecek.GetType().InvokeMember(item.Name, (BindingFlags.GetProperty | BindingFlags.GetField), null, eklenecek, null);
                    var nodeDeger = (d1 != null) ? d1.ToString() : "";
                    s11.Value = (sifrele) ? Sifrele(nodeDeger) : nodeDeger;
                    s1.AppendChild(s11);
                    yeni.AppendChild(s1);
                }
                root.AppendChild(yeni);
                xdoc.Save(XmlFile);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(string XmlFile, string RootElement, string ElementName, string AttributeName, string Value)
        {
            try
            {
                if (!File.Exists(XmlFile)) return true;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode(RootElement);
                var sifreAttr = root.Attributes["PasswordProtected"];
                if (sifreAttr != null && !string.IsNullOrEmpty(sifreAttr.Value) && Convert.ToBoolean(sifreAttr.Value)) Value = Sifrele(Value);
                foreach (XmlNode item in root.SelectNodes(ElementName))
                {
                    if (item.SelectSingleNode(AttributeName).InnerText == Value)
                    {
                        root.RemoveChild(item);
                        xdoc.Save(XmlFile);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ReCreateXmlFile(string XmlFile, string RootElement)
        {
            try
            {
                if (File.Exists(XmlFile)) File.Delete(XmlFile);
                return CreateXml(XmlFile, RootElement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteXmlFile(string XmlFile)
        {
            try
            {
                if (File.Exists(XmlFile)) File.Delete(XmlFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string Sifrele(string sifrelenecek)
        {
            byte[] Anahtar = { 16, 78, 103, 4, 155, 96, 74, 193, 56, 210, 11, 63, 13, 123, 30, 46, 17, 18, 54, 120, 21, 212, 144, 87 };
            byte[] Vektor = { 81, 202, 31, 104, 56, 53, 6, 181 };
            byte[] sfr = Encoding.GetEncoding(1254).GetBytes(sifrelenecek);
            System.Security.Cryptography.TripleDES des = System.Security.Cryptography.TripleDES.Create();
            System.Security.Cryptography.ICryptoTransform donusturucu = des.CreateEncryptor(Anahtar, Vektor);
            var donenbyte = donusturucu.TransformFinalBlock(sfr, 0, sfr.Length);
            return System.Convert.ToBase64String(donenbyte);
        }
    }
}
