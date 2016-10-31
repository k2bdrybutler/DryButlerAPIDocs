using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace K2Facade
{
    public partial class Tools
    {
        /// <summary>
        /// Returns Current Date and Time in UTC
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static DateTime Today
        {
            get
            {
                return Now.Date;
            }
        }

        public static string NowString
        {
            get
            {
                return Now.ToString("dd.MM.yyyy hh:mm");
            }
        }

        public static string LocalTimeZoneID
        {
            get
            {
                return TimeZoneInfo.Local.Id;
            }
        }

        public static DateTime ToLocalTimeZone(DateTime dateTime, string timeZoneID)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Utc.Id, timeZoneID);
        }

        public static DateTime? ToLocalTimeZone(DateTime? dateTime, string timeZoneID)
        {
            if (!dateTime.HasValue) return null;
            else return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.Value, TimeZoneInfo.Utc.Id, timeZoneID);
        }
    }

    public partial class Tools
    {
        public static bool WriteLog(string modul, string mesaj)
        {
            try
            {
                using (SqlConnection con = Tools.GetMDConnection())
                {
                    SqlCommand cmd = new SqlCommand("insert into errorlog(module,message,errortime) values(@mdl,@msg,@etm)", con);
                    cmd.Parameters.AddWithValue("@mdl", modul);
                    cmd.Parameters.AddWithValue("@msg", mesaj);
                    cmd.Parameters.AddWithValue("@etm", DateTime.Now);
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool WriteLog(string modul, string mesaj, int userid)
        {
            try
            {
                using (SqlConnection con = Tools.GetMDConnection())
                {
                    SqlCommand cmd = new SqlCommand("insert into errorlog(module,message,errortime,userid) values(@mdl,@msg,@etm,@uid)", con);
                    cmd.Parameters.AddWithValue("@mdl", modul);
                    cmd.Parameters.AddWithValue("@msg", mesaj);
                    cmd.Parameters.AddWithValue("@etm", DateTime.Now);
                    cmd.Parameters.AddWithValue("@uid", userid);
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool WriteLog(string modul, Exception Hata)
        {
            try
            {
                var mesaj = (Hata.InnerException != null) ? Hata.InnerException.Message : Hata.Message;
                using (SqlConnection con = Tools.GetMDConnection())
                {
                    SqlCommand cmd = new SqlCommand("insert into errorlog(module,message,errortime) values(@mdl,@msg,@etm)", con);
                    cmd.Parameters.AddWithValue("@mdl", modul);
                    cmd.Parameters.AddWithValue("@msg", mesaj);
                    cmd.Parameters.AddWithValue("@etm", DateTime.Now);
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool WriteLog(string modul, Exception Hata, int userid)
        {
            try
            {
                var mesaj = (Hata.InnerException != null) ? Hata.InnerException.Message : Hata.Message;
                using (SqlConnection con = Tools.GetMDConnection())
                {
                    SqlCommand cmd = new SqlCommand("insert into errorlog(module,message,errortime,userID) values(@mdl,@msg,@etm,@uid)", con);
                    cmd.Parameters.AddWithValue("@mdl", modul);
                    cmd.Parameters.AddWithValue("@msg", mesaj);
                    cmd.Parameters.AddWithValue("@etm", DateTime.Now);
                    cmd.Parameters.AddWithValue("@uid", userid);
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return (cmd.ExecuteNonQuery() > 0) ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public partial class Tools
    {
        public static string RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string CompanyName = "SevkTakip";
        public static string FileName = "Company.xml";

        public static SqlConnection GetMDConnection()
        {
            return new SqlConnection(Tools.GetConnectionString());
        }

        private static string GetConnectionString()
        {
            var ayar = Tools.ReadSettings();
            return GetSqlConnectionString(ayar.Server, ayar.Database, ayar.UseSSPI, ayar.UserID, ayar.Password);
        }

        public static string GetSqlConnectionString(string server, string database, bool sspi, string uid = null, string pwd = null, bool multipleActiveResultSets = true, int timeout = 10)
        {
            try
            {
                if (!sspi && (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pwd))) throw new MissingMemberException("User ID And Password Required!");
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
                sb.DataSource = server;
                sb.InitialCatalog = database;
                sb.IntegratedSecurity = sspi;
                if (!sspi && !string.IsNullOrEmpty(uid)) sb.UserID = uid;
                if (!sspi && !string.IsNullOrEmpty(pwd)) sb.Password = pwd;
                sb.MultipleActiveResultSets = multipleActiveResultSets;
                sb.ConnectTimeout = timeout;
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlConnection GetSqlConnection(string server, string database, bool sspi, string uid = null, string pwd = null, bool multipleActiveResultSets = true, int timeout = 10)
        {
            try
            {
                return new SqlConnection(GetSqlConnectionString(server, database, sspi, uid, pwd, multipleActiveResultSets, timeout));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public partial class Tools
    {
        private static bool sifrele = false;
        private static string SessionXmlFile = RootFolder + "\\" + CompanyName + "\\Session.xml";

        internal static string Sifrele(string sifrelenecek)
        {
            if (string.IsNullOrEmpty(sifrelenecek)) return sifrelenecek;
            byte[] Anahtar = { 16, 78, 103, 4, 155, 96, 74, 193, 56, 210, 11, 63, 13, 123, 30, 46, 17, 18, 54, 120, 21, 212, 144, 87 };
            byte[] Vektor = { 81, 202, 31, 104, 56, 53, 6, 181 };
            byte[] sfr = Encoding.GetEncoding(1254).GetBytes(sifrelenecek);
            TripleDES des = TripleDES.Create();
            ICryptoTransform donusturucu = des.CreateEncryptor(Anahtar, Vektor);
            var donenbyte = donusturucu.TransformFinalBlock(sfr, 0, sfr.Length);
            return System.Convert.ToBase64String(donenbyte);
        }

        internal static string SifreCoz(string cozulecek)
        {
            try
            {
                if (string.IsNullOrEmpty(cozulecek)) return cozulecek;
                var SifreliMetin = Convert.FromBase64String(cozulecek);
                byte[] Anahtar = { 16, 78, 103, 4, 155, 96, 74, 193, 56, 210, 11, 63, 13, 123, 30, 46, 17, 18, 54, 120, 21, 212, 144, 87 };
                byte[] Vektor = { 81, 202, 31, 104, 56, 53, 6, 181 };
                TripleDES des = TripleDES.Create();
                ICryptoTransform donusturucu = des.CreateDecryptor(Anahtar, Vektor);
                byte[] Cozulmus = donusturucu.TransformFinalBlock(SifreliMetin, 0, SifreliMetin.Length);
                return Encoding.GetEncoding(1254).GetString(Cozulmus);
            }
            catch (Exception)
            {
                return null;
            }
        }

        static bool staticSettings = true;

        public static Company ReadSettings(int sessionID = 0)
        {
            try
            {
                if (staticSettings)
                {
                    return new Company()
                    {
                        ID = 0,
                        CompanyName = "DryButler",
                        UseSSPI = false,
                        Server = "10.10.10.7",
                        //Server = ".",
                        Database = "DryButler",
                        UserID = "sa",
                        Password = "123456"
                    };
                }
                else
                {
                    if (!File.Exists(SessionXmlFile)) return null;
                    var donecek = new Company();
                    donecek.ID = (sessionID == 0) ? System.Diagnostics.Process.GetCurrentProcess().Id : sessionID;
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(SessionXmlFile);
                    var root = xdoc.SelectSingleNode("Sessions");
                    foreach (XmlNode item in root.SelectNodes("Session"))
                    {
                        if (item.SelectSingleNode("ID").InnerText == donecek.ID.ToString())
                        {
                            foreach (var property in typeof(Company).GetProperties())
                            {
                                var xF = item.SelectSingleNode(property.Name);
                                var xD = (xF != null) ? xF.InnerText : string.Empty;
                                if (!string.IsNullOrEmpty(xD))
                                    donecek.GetType().GetProperty(property.Name).SetValue(donecek, Convert.ChangeType(xD, property.PropertyType), null);
                            }

                        }
                    }
                    return new Company()
                    {
                        ID = donecek.ID,
                        CompanyName = SifreCoz(donecek.CompanyName),
                        Server = SifreCoz(donecek.Server),
                        Database = SifreCoz(donecek.Database),
                        UseSSPI = donecek.UseSSPI,
                        UserID = SifreCoz(donecek.UserID),
                        Password = SifreCoz(donecek.Password)
                    };
                }
            }
            catch (Exception ex)
            {
                WriteLog("Tools - ReadSettings", ex);
                return null;
            }
        }

        public static bool CreateSettings(Company company, int processID = 0)
        {
            try
            {
                var s = new Company();
                s.ID = (processID == 0) ? System.Diagnostics.Process.GetCurrentProcess().Id : processID;
                s.CompanyName = Sifrele(company.CompanyName);
                s.Server = Sifrele(company.Server);
                s.Database = Sifrele(company.Database);
                s.UseSSPI = company.UseSSPI;
                s.UserID = Sifrele(company.UserID);
                s.Password = Sifrele(company.Password);
                return XmlTools.Insert(SessionXmlFile, "Sessions", "Session", s, sifrele);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CopySettings(int oldID)
        {
            try
            {
                if (!File.Exists(SessionXmlFile)) return false;
                var donecek = new Company();
                donecek.ID = System.Diagnostics.Process.GetCurrentProcess().Id;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(SessionXmlFile);
                var root = xdoc.SelectSingleNode("Sessions");
                foreach (XmlNode item in root.SelectNodes("Session"))
                {
                    if (item.SelectSingleNode("ID").InnerText == oldID.ToString())
                    {
                        foreach (var property in typeof(Company).GetProperties())
                        {
                            donecek.GetType().GetProperty(property.Name).SetValue(donecek,
                                Convert.ChangeType(item.SelectSingleNode(property.Name).InnerText, property.PropertyType)
                                , null);
                        }

                    }
                }
                return XmlTools.Insert(SessionXmlFile, "Sessions", "Session", donecek, sifrele);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DisposeSettings()
        {
            try
            {
                if (!File.Exists(SessionXmlFile)) return;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(SessionXmlFile);
                var root = xdoc.SelectSingleNode("Sessions");
                if (root.SelectNodes("Session").Count > 1) return;
                else XmlTools.DeleteXmlFile(SessionXmlFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteTxtLog(string module, string message)
        {
            try
            {
                var xFile = RootFolder + "\\" + CompanyName + "\\SysLog.txt";
                using (var fs = new FileStream(xFile, FileMode.OpenOrCreate))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("----- Log Time: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm") + " -----");
                        sw.WriteLine("Modul: " + module);
                        sw.WriteLine("Message: " + message);
                        sw.WriteLine("--------------------------------------");
                        sw.Flush();
                        sw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public partial class Tools
    {
        public static string GetMultiValue(List<string> values)
        {
            try
            {
                var donecek = "";
                for (int i = 0; i < values.Count; i++)
                {
                    if (i == 0)
                    {
                        donecek = donecek + values[i];
                    }
                    else
                    {
                        donecek = donecek + ", " + values[i];
                    }
                }
                return donecek;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetMultiValue(string[] values, bool noSpace = false, string tirnak = "")
        {
            try
            {
                var donecek = "";
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!string.IsNullOrEmpty(tirnak)) donecek += tirnak;
                        donecek = donecek + values[i];
                        if (!string.IsNullOrEmpty(tirnak)) donecek += tirnak;
                    }
                    else
                    {
                        donecek = donecek + ((noSpace) ? "," : ", ");
                        if (!string.IsNullOrEmpty(tirnak)) donecek += tirnak;
                        donecek = donecek + values[i];
                        if (!string.IsNullOrEmpty(tirnak)) donecek += tirnak;
                    }
                }
                return donecek;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string ImageToBase64(string imageLocation, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var image = System.Drawing.Image.FromFile(imageLocation);
                image.Save(ms, format);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static System.Drawing.Image ImageFromBase64(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
    }
}
