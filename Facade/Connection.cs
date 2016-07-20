using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace K2Facade
{
    public class Connection
    {
        private IConnectionSettings Setting;

        public Connection(IConnectionSettings settings)
        {
            Setting = settings;
        }

        public SqlConnection Con
        {
            get
            {
                if (Setting == null) throw new KeyNotFoundException("Connection Settings Not Found!");
                return GetSqlConnection(Setting.Server, Setting.Database, Setting.UseSSPI, Setting.UserID, Setting.Password);
            }
        }

        public string GetSqlConnectionString(string server, string database, bool sspi, string uid = null, string pwd = null, bool multipleActiveResultSets = true, int timeout = 10)
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

        public SqlConnection GetSqlConnection(string server, string database, bool sspi, string uid = null, string pwd = null, bool multipleActiveResultSets = true, int timeout = 10)
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
}
