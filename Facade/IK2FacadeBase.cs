using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace K2Facade
{
    public interface IK2FacadeBase : IDisposable
    {
        #region Procedure Methods

        int ExecuteSqlProc(int userID, string procName, params SqlFilter[] parameters);

        DataTable GetSqlProcData(int userID, string procName, params SqlFilter[] parameters);

        List<T> ExecuteSql<T>(int userID, string sqlCommand, params SqlFilter[] parameters) where T : IEntityBase;

        #endregion

        #region Insert & Update & Delete

        bool Insert(IEntityBase record, int userID);

        bool Update(IEntityBase record, int userID);

        bool Delete(IEntityBase record, int userID);

        #endregion

        #region Select Methods

        List<T> SelectAll<T>(int userID, int startIndex = -1, int take = -1, params SqlOrder[] sorters) where T : IEntityBase;

        T SelectByID<T>(int id, int userID) where T : IEntityBase;

        T GetFirst<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase;

        T GetFirst<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase;

        T GetFirst<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase;

        int GetCount<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase;

        bool isInsertable<T>(int userID, int id, params SqlFilter[] parameters) where T : IEntityBase;

        #endregion

        #region Search Methods

        List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase;

        List<T> Search<T>(int userID, int startIndex = -1, int take = -1, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase;

        List<T> Search<T>(int userID, List<SqlOrder> sorters = null, params SqlFilter[] parameters) where T : IEntityBase;

        List<T> Search<T>(int userID, SqlOrder sorter = null, params SqlFilter[] parameters) where T : IEntityBase;

        List<T> Search<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase;

        List<T> Search<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase;

        #endregion

        #region GetDataTable

        DataTable GetDataTable(int userID, string query, params SqlFilter[] parameters);

        DataTable GetDataTable(int userID, string query, IConnectionSettings connection, params SqlFilter[] parameters);

        DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase;

        DataTable GetDataTable<T>(int userID, int startIndex = -1, int take = -1, string sorting = null, params SqlFilter[] parameters) where T : IEntityBase;

        DataTable GetDataTable<T>(int userID, params SqlFilter[] parameters) where T : IEntityBase;

        DataTable GetDataTable<T>(int userID, bool deletedRecords = false, params SqlFilter[] parameters) where T : IEntityBase;

        #endregion
    }
}
