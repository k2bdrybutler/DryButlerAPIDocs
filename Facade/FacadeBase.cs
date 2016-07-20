using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace K2Facade
{
    public partial class FacadeBase : IK2FacadeBase
    {
        #region Base Items

        private IConnectionSettings Settings;

        public FacadeBase(IConnectionSettings settings)
        {
            Settings = settings;
        }

        private SqlConnection GetConnection()
        {
            var cnn = new Connection(Settings);
            return cnn.Con;
        }

        private SqlConnection GetConnection(IConnectionSettings cnnSettings)
        {
            var cnn = new Connection(cnnSettings);
            return cnn.Con;
        }

        private T GenerateObject<T>(SqlDataReader dr)
        {
            try
            {
                var kayit = Activator.CreateInstance<T>();
                foreach (var prp in kayit.GetType().GetProperties())
                {
                    var getMethod = prp.GetGetMethod();
                    if (!getMethod.IsFinal && getMethod.IsVirtual) continue;
                    object deger = null;
                    try
                    {
                        deger = dr[prp.Name];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Tools.WriteLog("K2Facade - GenerateObject", "Field Not Found! Object: " + typeof(T).Name + " FieldName: " + prp.Name);
                        deger = null;
                    }
                    if (deger != null && deger != DBNull.Value)
                        kayit.GetType().GetProperty(prp.Name).SetValue(kayit, deger, null);
                }
                return kayit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Procedure Methods

        public int ExecuteSqlProc(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("dbo." + procName, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (con.State != System.Data.ConnectionState.Open) con.Open();
                    var sonuc = cmd.ExecuteNonQuery();
                    con.Close();
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteSqlProcScalarObj(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("dbo." + procName, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (con.State != System.Data.ConnectionState.Open) con.Open();
                    var sonuc = cmd.ExecuteScalar();
                    con.Close();
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ExecuteSqlProc<T>(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                List<T> donecek = null;
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(((procName.ToLower().StartsWith("dbo.")) ? "" : "dbo.") + procName, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (con.State != System.Data.ConnectionState.Open) con.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        donecek = new List<T>();
                        while (dr.Read())
                            donecek.Add(GenerateObject<T>(dr));
                    }
                    con.Close();
                }
                return donecek;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSqlProcData(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlDataAdapter cmd = new SqlDataAdapter("dbo." + procName, con);
                    cmd.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.SelectCommand.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.SelectCommand.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    var dt = new DataTable();
                    cmd.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ExecuteSql<T>(int userID, string sqlCommand, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                List<T> donecek = new List<T>();
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sqlCommand);
                    cmd.Connection = cnn;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                                donecek.Add(GenerateObject<T>(dr));
                        dr.Close();
                    }
                }
                return donecek;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteSql(int userID, string sqlCommand, params SqlFilter[] parameters)
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sqlCommand);
                    cmd.Connection = cnn;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T ExecuteScalarSql<T>(int userID, string sqlCommand, params SqlFilter[] parameters)
        {
            try
            {
                var donecek = default(T);
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sqlCommand);
                    cmd.Connection = cnn;
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    donecek = (T)cmd.ExecuteScalar();
                }
                return donecek;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Insert & Update & Delete

        public bool Insert(IEntityBase record, int userID)
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    var haricAlanlar = new List<string>() { "ID", "UUser", "UDate", "DUser", "DDate" };
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("insert into [");
                    sb.Append(record.GetType().Name);
                    sb.Append("](");
                    int sayac = 0;
                    foreach (var prp in record.GetType().GetProperties())
                    {
                        if (haricAlanlar.Contains(prp.Name) || (!prp.GetGetMethod().IsFinal && prp.GetGetMethod().IsVirtual)) continue;
                        if (sayac > 0) sb.Append(", ");
                        sb.Append("[");
                        sb.Append(prp.Name);
                        sb.Append("]");
                        sayac++;
                    }
                    sb.Append(") values(");
                    sayac = 0;
                    foreach (var prp in record.GetType().GetProperties())
                    {
                        if (haricAlanlar.Contains(prp.Name) || (!prp.GetGetMethod().IsFinal && prp.GetGetMethod().IsVirtual)) continue;
                        if (sayac > 0) sb.Append(", ");
                        sb.Append("@");
                        sb.Append(prp.Name);
                        sayac++;
                    }
                    sb.Append("); select SCOPE_IDENTITY();");
                    cmd.CommandText = sb.ToString();
                    foreach (var prp in record.GetType().GetProperties())
                    {
                        if (haricAlanlar.Contains(prp.Name) || (!prp.GetGetMethod().IsFinal && prp.GetGetMethod().IsVirtual)) continue;
                        if (prp.Name == "Status") cmd.Parameters.AddWithValue("@" + prp.Name, true);
                        else if (prp.Name == "CUser") cmd.Parameters.AddWithValue("@" + prp.Name, userID);
                        else if (prp.Name == "CDate") cmd.Parameters.AddWithValue("@" + prp.Name, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        else
                        {
                            var deger = record.GetType().GetProperty(prp.Name).GetValue(record, null);
                            if (deger == null && prp.PropertyType == typeof(byte[]))
                            {
                                cmd.Parameters.Add("@" + prp.Name, System.Data.SqlDbType.VarBinary, -1);
                                cmd.Parameters["@" + prp.Name].Value = DBNull.Value;
                            }
                            else
                            {
                                if (deger == null) deger = DBNull.Value;
                                cmd.Parameters.AddWithValue("@" + prp.Name, deger);
                            }
                        }
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                    if (id > 0)
                    {
                        record.GetType().GetProperty("ID").SetValue(record, id, null);
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(IEntityBase record, int userID)
        {
            try
            {
                using (var cnn = GetConnection())
                {
                    var haricAlanlar = new List<string>() { "ID", "CUser", "CDate", "DUser", "DDate" };
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update [");
                    sb.Append(record.GetType().Name);
                    sb.Append("] set ");
                    int sayac = 0;
                    foreach (var prp in record.GetType().GetProperties())
                    {
                        if (haricAlanlar.Contains(prp.Name) || (!prp.GetGetMethod().IsFinal && prp.GetGetMethod().IsVirtual)) continue;
                        if (sayac > 0) sb.Append(", ");
                        sb.Append("[");
                        sb.Append(prp.Name);
                        sb.Append("]");
                        sb.Append(" = @");
                        sb.Append(prp.Name);
                        sayac++;
                    }
                    sb.Append(" where ID = @id");
                    cmd.CommandText = sb.ToString();
                    cmd.CommandTimeout = 0;
                    foreach (var prp in record.GetType().GetProperties())
                    {
                        if (haricAlanlar.Contains(prp.Name) || (!prp.GetGetMethod().IsFinal && prp.GetGetMethod().IsVirtual)) continue;
                        if (prp.Name == "Status") cmd.Parameters.AddWithValue("@" + prp.Name, true);
                        else if (prp.Name == "UUser") cmd.Parameters.AddWithValue("@" + prp.Name, userID);
                        else if (prp.Name == "UDate") cmd.Parameters.AddWithValue("@" + prp.Name, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        else
                        {
                            var deger = record.GetType().GetProperty(prp.Name).GetValue(record, null);
                            if (deger == null && prp.PropertyType == typeof(byte[]))
                            {
                                cmd.Parameters.Add("@" + prp.Name, System.Data.SqlDbType.VarBinary, -1);
                                cmd.Parameters["@" + prp.Name].Value = DBNull.Value;
                            }
                            else
                            {
                                if (deger == null) deger = DBNull.Value;
                                cmd.Parameters.AddWithValue("@" + prp.Name, deger);
                            }
                        }

                    }
                    cmd.Parameters.AddWithValue("@id", record.GetType().GetProperty("ID").GetValue(record, null));
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(IEntityBase record, int userID)
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update [");
                    sb.Append(record.GetType().Name);
                    sb.Append("] set [Status] = @Status, [DUser] = @DUser, [DDate] = @DDate where ID = @ID");
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@Status", false);
                    cmd.Parameters.AddWithValue("@DUser", userID);
                    cmd.Parameters.AddWithValue("@DDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@ID", record.GetType().GetProperty("ID").GetValue(record, null));
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteAll<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update [");
                    sb.Append((typeof(T)).Name);
                    sb.Append("] set [Status] = @Status, [DUser] = @DUser, [DDate] = @DDate where 1 = 1");
                    var lastGroup = -1;
                    var prmList = parameters.OrderBy(x => x.Group).ToList();
                    for (int i = 0; i < prmList.Count; i++)
                    {
                        var current = prmList[i];
                        if (current.Group == lastGroup)
                            sb.Append(" or ");
                        else sb.Append(" and (");
                        if (!current.Field.Contains("(")) sb.Append("[");
                        sb.Append(current.Field);
                        if (!current.Field.Contains("(")) sb.Append("]");
                        sb.Append(current.GetOperator());
                        if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                            sb.AppendLine(")");
                        if (current.Group > 0) lastGroup = current.Group;
                    }
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@Status", false);
                    cmd.Parameters.AddWithValue("@DUser", userID);
                    cmd.Parameters.AddWithValue("@DDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Select Methods

        public List<T> SelectAll<T>(int userID, int startIndex = -1, int take = -1, params SqlOrder[] sorters) where T : IEntityBase
        {
            try
            {
                List<T> donecek = new List<T>();
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    if (startIndex != -1 && take != -1)
                    {
                        sb.AppendLine("select * from (");
                        sb.Append("select row_number() over (");
                        if (sorters != null && sorters.Count() > 0) sb.Append(SqlOrder.GetOrderString(sorters));
                        else sb.Append("order by ID");
                        sb.AppendLine(") as ix, * from(");
                        sb.Append("select * from [");
                        sb.Append((typeof(T)).Name);
                        sb.AppendLine("] where Status = 1");
                        sb.Append(")ds1 )ds2 where ix between ");
                        sb.Append(startIndex);
                        sb.Append(" and ");
                        sb.Append(startIndex + take);
                    }
                    else
                    {
                        sb.Append("select * from [");
                        sb.Append((typeof(T)).Name);
                        sb.Append("] where Status = 1");
                        if (sorters != null && sorters.Count() > 0) sb.Append(SqlOrder.GetOrderString(sorters));
                    }
                    cmd.CommandText = sb.ToString();
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                                donecek.Add(GenerateObject<T>(dr));
                        dr.Close();
                    }
                }
                return donecek;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T SelectByID<T>(int id, int userID) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select * from [");
                    sb.Append((typeof(T)).Name);
                    sb.Append("] where ID = @ID");
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@ID", id);
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                                return GenerateObject<T>(dr);
                        dr.Close();
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetFirst<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select top 1 * from [");
                    sb.Append((typeof(T)).Name);
                    sb.Append("] where [Status] = 1");
                    var lastGroup = -1;
                    var prmList = parameters.OrderBy(x => x.Group).ToList();
                    for (int i = 0; i < prmList.Count; i++)
                    {
                        var current = prmList[i];
                        if (current.Group == lastGroup)
                            sb.Append(" or ");
                        else sb.Append(" and (");
                        if (!current.Field.Contains("(")) sb.Append("[");
                        sb.Append(current.Field);
                        if (!current.Field.Contains("(")) sb.Append("]");
                        sb.Append(current.GetOperator());
                        if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                            sb.AppendLine(")");
                        if (current.Group > 0) lastGroup = current.Group;
                    }
                    if (sorters != null && sorters.Count() > 0) sb.Append(SqlOrder.GetOrderString(sorters));
                    cmd.CommandText = sb.ToString();
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                                return GenerateObject<T>(dr);
                        dr.Close();
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetFirst<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetFirst<T>(userID, new List<SqlOrder>() { sorter }, parameters);
        }

        public T GetFirst<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetFirst<T>(userID, new List<SqlOrder>(), parameters);
        }

        public int GetCount<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select count(*) from [");
                    sb.Append((typeof(T)).Name);
                    sb.Append("] where [Status] = 1");
                    var lastGroup = -1;
                    var prmList = parameters.OrderBy(x => x.Group).ToList();
                    for (int i = 0; i < prmList.Count; i++)
                    {
                        var current = prmList[i];
                        if (current.Group == lastGroup)
                            sb.Append(" or ");
                        else sb.Append(" and (");
                        if (!current.Field.Contains("(")) sb.Append("[");
                        sb.Append(current.Field);
                        if (!current.Field.Contains("(")) sb.Append("]");
                        sb.Append(current.GetOperator());
                        if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                            sb.AppendLine(")");
                        if (current.Group > 0) lastGroup = current.Group;
                    }
                    cmd.CommandText = sb.ToString();
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isInsertable<T>(int userID, int id, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select count(*) from [");
                    sb.Append((typeof(T)).Name);
                    sb.Append("] where [Status] = 1 and ID <> @id");
                    var lastGroup = -1;
                    var prmList = parameters.OrderBy(x => x.Group).ToList();
                    for (int i = 0; i < prmList.Count; i++)
                    {
                        var current = prmList[i];
                        if (current.Group == lastGroup)
                            sb.Append(" or ");
                        else sb.Append(" and (");
                        if (!current.Field.Contains("(")) sb.Append("[");
                        sb.Append(current.Field);
                        if (!current.Field.Contains("(")) sb.Append("]");
                        sb.Append(current.GetOperator());
                        if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                            sb.AppendLine(")");
                        if (current.Group > 0) lastGroup = current.Group;
                    }
                    cmd.CommandText = sb.ToString();
                    cmd.Parameters.AddWithValue("@id", id);
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Search Methods

        public List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                List<T> donecek = new List<T>();
                using (SqlConnection cnn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    StringBuilder sb = new StringBuilder();
                    if (startIndex != -1 && take != -1)
                    {
                        sb.AppendLine("select * from (");
                        sb.Append("select row_number() over (");
                        if (sorters != null && sorters.Count() > 0) sb.Append(SqlOrder.GetOrderString(sorters));
                        else sb.Append("order by ID");
                        sb.AppendLine(") as ix, * from(");
                        sb.Append("select * from [");
                        sb.Append((typeof(T)).Name);
                        if (deletedRecords) sb.Append("] where 1 = 1");
                        else sb.Append("] where [Status] = 1");
                        var lastGroup = -1;
                        var prmList = parameters.OrderBy(x => x.Group).ToList();
                        for (int i = 0; i < prmList.Count; i++)
                        {
                            var current = prmList[i];
                            if (current.Group == lastGroup)
                                sb.Append(" or ");
                            else sb.Append(" and (");
                            if (!current.Field.Contains("(")) sb.Append("[");
                            sb.Append(current.Field);
                            if (!current.Field.Contains("(")) sb.Append("]");
                            sb.Append(current.GetOperator());
                            if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                                sb.AppendLine(")");
                            if (current.Group > 0) lastGroup = current.Group;
                        }
                        sb.Append(")ds1 )ds2 where ix between ");
                        sb.Append(startIndex);
                        sb.Append(" and ");
                        sb.Append(startIndex + take);
                    }
                    else
                    {
                        sb.Append("select * from [");
                        sb.Append((typeof(T)).Name);
                        if (deletedRecords) sb.Append("] where 1 = 1");
                        else sb.Append("] where [Status] = 1");
                        var lastGroup = -1;
                        var prmList = parameters.OrderBy(x => x.Group).ToList();
                        for (int i = 0; i < prmList.Count; i++)
                        {
                            var current = prmList[i];
                            if (current.Group == lastGroup)
                                sb.Append(" or ");
                            else sb.Append(" and (");
                            if (!current.Field.Contains("(")) sb.Append("[");
                            sb.Append(current.Field);
                            if (!current.Field.Contains("(")) sb.Append("]");
                            sb.Append(current.GetOperator());
                            if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                                sb.AppendLine(")");
                            if (current.Group > 0) lastGroup = current.Group;
                        }
                        if (sorters != null && sorters.Count() > 0) sb.Append(SqlOrder.GetOrderString(sorters));
                    }
                    cmd.CommandText = sb.ToString();
                    foreach (var item in parameters)
                    {
                        if (item.Values.Count == 1)
                            cmd.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                        else
                            for (int i = 0; i < item.Values.Count; i++)
                                cmd.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                    }
                    if (cnn.State != System.Data.ConnectionState.Open) cnn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                                donecek.Add(GenerateObject<T>(dr));
                        dr.Close();
                    }
                }
                return donecek;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, startIndex, take, sorters, false, parameters);
        }

        public List<T> Search<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, sorters, parameters);
        }

        public List<T> Search<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, new List<SqlOrder>() { sorter }, parameters);
        }

        public List<T> Search<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, null, parameters);
        }

        public List<T> Search<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, null, deletedRecords, parameters);
        }

        #endregion

        #region GetDataTable

        public DataTable GetDataTable(int userID, string query, params SqlFilter[] parameters)
        {
            return GetDataTable(userID, query, Settings, parameters);
        }

        public DataTable GetDataTable(int userID, string query, IConnectionSettings connection, params SqlFilter[] parameters)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, GetConnection(connection)))
                {
                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var item in parameters)
                        {
                            if (item.Values.Count == 1)
                                da.SelectCommand.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                            else
                                for (int i = 0; i < item.Values.Count; i++)
                                    da.SelectCommand.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                        }
                    }
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                using (SqlConnection cnn = GetConnection())
                {
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand.Connection = cnn;
                        StringBuilder sb = new StringBuilder();
                        if (startIndex != -1 && take != -1)
                        {
                            sb.AppendLine("select * from (");
                            sb.Append("select row_number() over (order by ");
                            if (!string.IsNullOrEmpty(sorting)) sb.Append(sorting);
                            else sb.Append("ID");
                            sb.AppendLine(") as ix, * from(");
                            sb.Append("select * from [");
                            sb.Append((typeof(T)).Name);
                            if (deletedRecords) sb.Append("] where 1 = 1");
                            else sb.Append("] where [Status] = 1");
                            var lastGroup = -1;
                            var prmList = parameters.OrderBy(x => x.Group).ToList();
                            for (int i = 0; i < prmList.Count; i++)
                            {
                                var current = prmList[i];
                                if (current.Group == lastGroup)
                                    sb.Append(" or ");
                                else sb.Append(" and (");
                                if (!current.Field.Contains("(")) sb.Append("[");
                                sb.Append(current.Field);
                                if (!current.Field.Contains("(")) sb.Append("]");
                                sb.Append(current.GetOperator());
                                if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                                    sb.AppendLine(")");
                                if (current.Group > 0) lastGroup = current.Group;
                            }
                            sb.Append(")ds1 )ds2 where ix between ");
                            sb.Append(startIndex);
                            sb.Append(" and ");
                            sb.Append(startIndex + take);
                        }
                        else
                        {
                            sb.Append("select * from [");
                            sb.Append((typeof(T)).Name);
                            if (deletedRecords) sb.Append("] where 1 = 1");
                            else sb.Append("] where [Status] = 1");
                            var lastGroup = -1;
                            var prmList = parameters.OrderBy(x => x.Group).ToList();
                            for (int i = 0; i < prmList.Count; i++)
                            {
                                var current = prmList[i];
                                if (current.Group == lastGroup)
                                    sb.Append(" or ");
                                else sb.Append(" and (");
                                if (!current.Field.Contains("(")) sb.Append("[");
                                sb.Append(current.Field);
                                if (!current.Field.Contains("(")) sb.Append("]");
                                sb.Append(current.GetOperator());
                                if (current.Group == 0 || i + 1 == prmList.Count || prmList[i + 1].Group != current.Group)
                                    sb.AppendLine(")");
                                if (current.Group > 0) lastGroup = current.Group;
                            }
                            if (!string.IsNullOrEmpty(sorting))
                            {
                                sb.Append(" order by ");
                                sb.Append(sorting);
                            }
                        }
                        da.SelectCommand.CommandText = sb.ToString();
                        foreach (var item in parameters)
                        {
                            if (item.Values.Count == 1)
                                da.SelectCommand.Parameters.AddWithValue(item.VariableName, item.Values[0]);
                            else
                                for (int i = 0; i < item.Values.Count; i++)
                                    da.SelectCommand.Parameters.AddWithValue(item.VariableName + "_" + i, item.Values[i]);
                        }
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, startIndex, take, sorting, false, parameters);
        }

        public DataTable GetDataTable<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, -1, -1, null, parameters);
        }

        public DataTable GetDataTable<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, -1, -1, null, deletedRecords, parameters);
        }

        #endregion
    }

    //A static copy of K2Facade.Facade
    public partial class Facade
    {
        #region Base Items

        private static FacadeBase facade;

        public Facade()
        {
            CreateInstance();
        }

        private static void CreateInstance()
        {
            if (facade == null) facade = new FacadeBase(Tools.ReadSettings());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Procedure Methods

        public static int ExecuteSqlProc(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.ExecuteSqlProc(userID, procName, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteSqlProc", ex, userID);
                throw ex;
            }
        }

        public static object ExecuteSqlProcScalarObj(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.ExecuteSqlProcScalarObj(userID, procName, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteSqlProcScalarObj", ex, userID);
                throw ex;
            }
        }

        public static List<T> ExecuteSqlProc<T>(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.ExecuteSqlProc<T>(userID, procName, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteSqlProc", ex, userID);
                throw ex;
            }
        }

        public static DataTable GetSqlProcData(int userID, string procName, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.GetSqlProcData(userID, procName, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetSqlProcData", ex, userID);
                throw ex;
            }
        }

        public static List<T> ExecuteSql<T>(int userID, string sqlCommand, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.ExecuteSql<T>(userID, sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteSql", ex, userID);
                throw ex;
            }
        }

        public static void ExecuteSql(int userID, string sqlCommand, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                facade.ExecuteSql(userID, sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteSql", ex, userID);
                throw ex;
            }
        }

        public static T ExecuteScalarSql<T>(int userID, string sqlCommand, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.ExecuteScalarSql<T>(userID, sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - ExecuteScalarSql", ex, userID);
                throw ex;
            }
        }

        #endregion

        #region Insert & Update & Delete

        public static bool Insert(IEntityBase record, int userID)
        {
            try
            {
                CreateInstance();
                return facade.Insert(record, userID);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - Insert", ex, userID);
                throw ex;
            }
        }

        public static bool Update(IEntityBase record, int userID)
        {
            try
            {
                CreateInstance();
                return facade.Update(record, userID);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - Update", ex, userID);
                throw ex;
            }
        }

        public static bool Delete(IEntityBase record, int userID)
        {
            try
            {
                CreateInstance();
                return facade.Delete(record, userID);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - Delete", ex, userID);
                throw ex;
            }
        }

        public static bool DeleteAll<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.DeleteAll<T>(userID, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - DeleteAll", ex, userID);
                throw ex;
            }
        }

        #endregion

        #region Select Methods

        public static List<T> SelectAll<T>(int userID, int startIndex = -1, int take = -1, params SqlOrder[] sorters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.SelectAll<T>(userID, startIndex, take, sorters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - SelectAll", ex, userID);
                throw ex;
            }
        }

        public static T SelectByID<T>(int id, int userID) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.SelectByID<T>(id, userID);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - SelectByID (" + typeof(T).Name + ")", ex, userID);
                throw ex;
            }
        }

        public static T GetFirst<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.GetFirst<T>(userID, sorters, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetFirst (" + typeof(T).Name + ")", ex, userID);
                throw ex;
            }
        }

        public static T GetFirst<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.GetFirst<T>(userID, sorter, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetFirst", ex, userID);
                throw ex;
            }
        }

        public static T GetFirst<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetFirst<T>(userID, new List<SqlOrder>(), parameters);
        }

        public static int GetCount<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.GetCount<T>(userID, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetCount", ex, userID);
                throw ex;
            }
        }

        public static bool isInsertable<T>(int userID, int id, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.isInsertable<T>(userID, id, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - isInsertable", ex, userID);
                throw ex;
            }
        }

        #endregion

        #region Search Methods

        public static List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.Search<T>(userID, startIndex, take, sorters, deletedRecords, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - Search (" + typeof(T).Name + ")", ex, userID);
                throw ex;
            }
        }

        public static List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, startIndex, take, sorters, false, parameters);
        }

        public static List<T> Search<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, sorters, parameters);
        }

        public static List<T> Search<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, new List<SqlOrder>() { sorter }, parameters);
        }

        public static List<T> Search<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, null, parameters);
        }

        public static List<T> Search<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            return Search<T>(userID, -1, -1, null, deletedRecords, parameters);
        }

        #endregion

        #region GetDataTable

        public static DataTable GetDataTable(int userID, string query, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.GetDataTable(userID, query, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetDataTable", ex, userID);
                throw ex;
            }
        }

        public static DataTable GetDataTable(int userID, string query, IConnectionSettings connection, params SqlFilter[] parameters)
        {
            try
            {
                CreateInstance();
                return facade.GetDataTable(userID, query, connection, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetDataTable", ex, userID);
                throw ex;
            }
        }

        public static DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            try
            {
                CreateInstance();
                return facade.GetDataTable<T>(userID, startIndex, take, sorting, deletedRecords, parameters);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("K2Facade.Facade - GetDataTable", ex, userID);
                throw ex;
            }
        }

        public static DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, startIndex, take, sorting, false, parameters);
        }

        public static DataTable GetDataTable<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, -1, -1, null, parameters);
        }

        public static DataTable GetDataTable<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase
        {
            return GetDataTable<T>(userID, -1, -1, null, deletedRecords, parameters);
        }

        #endregion
    }
}