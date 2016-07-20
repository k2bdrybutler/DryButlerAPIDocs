using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace K2Facade
{
    public partial class Company : EntityBase, K2Facade.IConnectionSettings
    {
        public string UniqID { get; set; }
        public string CompanyName { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public bool UseSSPI { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return this.CompanyName;
        }
    }

    public partial class Company
    {
        private static string XmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Tools.CompanyName + "\\" + Tools.FileName;
        private static bool sifrele = true;

        public static bool Insert(Company eklenecek)
        {
            try
            {
                return XmlTools.Insert(XmlFile, "Companys", "Company", eklenecek, sifrele);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Company guncellenecek)
        {
            try
            {
                return Delete(guncellenecek) && Insert(guncellenecek);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(Company silinecek)
        {
            return Delete(silinecek.UniqID);
        }

        public static bool Delete(string uid)
        {
            try
            {
                return XmlTools.Delete(XmlFile, "Companys", "Company", "CompanyName", uid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isInsertable(string name)
        {
            try
            {
                if (!File.Exists(XmlFile)) return true;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode("Companys");
                foreach (XmlNode item in root.SelectNodes("Company"))
                {
                    if (item.SelectSingleNode("CompanyName").Value == Tools.Sifrele(name))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Company SelectByID(string uid)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode("Companys");
                foreach (XmlNode item in root.SelectNodes("Company"))
                {
                    var sAd = Tools.SifreCoz(item.SelectSingleNode("UniqID").InnerText);
                    if (sAd == uid)
                    {
                        var donecek = new Company();
                        foreach (var property in typeof(Company).GetProperties())
                        {
                            var deger = (!string.IsNullOrEmpty(item.SelectSingleNode(property.Name).InnerText))
                                ? (sifrele) ? Tools.SifreCoz(item.SelectSingleNode(property.Name).InnerText) : item.SelectSingleNode(property.Name).InnerText
                                : null;
                            if (!string.IsNullOrEmpty(deger))
                            {
                                donecek.GetType().GetProperty(property.Name).SetValue(donecek,
                                    Convert.ChangeType(deger, property.PropertyType)
                                    , null);
                            }
                        }
                        return donecek;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Company SelectByName(string name)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode("Companys");
                foreach (XmlNode item in root.SelectNodes("Company"))
                {
                    var sAd = Tools.SifreCoz(item.SelectSingleNode("CompanyName").InnerText);
                    if (sAd == name)
                    {
                        var donecek = new Company();
                        foreach (var property in typeof(Company).GetProperties())
                        {
                            var deger = item.SelectSingleNode(property.Name).InnerText;
                            if (!string.IsNullOrEmpty(deger))
                            {
                                donecek.GetType().GetProperty(property.Name).SetValue(donecek,
                                    Convert.ChangeType(((sifrele) ? Tools.SifreCoz(deger) : deger), property.PropertyType)
                                    , null);
                            }
                        }
                        return donecek;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<Company> SelectAll()
        {
            try
            {
                if (!File.Exists(XmlFile)) return null;
                List<Company> Donecekler = new List<Company>();
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(XmlFile);
                var root = xdoc.SelectSingleNode("Companys");
                foreach (XmlNode item in root.SelectNodes("Company"))
                {
                    var donecek = new Company();
                    foreach (var property in typeof(Company).GetProperties())
                    {
                        var xmlF = item.SelectSingleNode(property.Name);
                        var xmlDeger = (xmlF != null)
                            ? ((sifrele) ? Tools.SifreCoz(xmlF.InnerText) : xmlF.InnerText)
                            : string.Empty;
                        if (!string.IsNullOrEmpty(xmlDeger))
                            donecek.GetType().GetProperty(property.Name)
                                .SetValue(donecek, Convert.ChangeType(xmlDeger, property.PropertyType), null);
                    }
                    Donecekler.Add(donecek);
                }
                return Donecekler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
