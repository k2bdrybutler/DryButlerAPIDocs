using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Facade
{
    public partial class SqlOrder : IDisposable
    {
        public string Field { get; set; }
        public OrderTypes OrderType { get; set; }
        public int OrderIndex { get; set; }

        public SqlOrder()
        {

        }

        public SqlOrder(string field)
        {
            Field = field;
        }

        public SqlOrder(string field, OrderTypes orderType)
        {
            Field = field;
            OrderType = orderType;
        }

        public SqlOrder(string field, OrderTypes orderType, int index)
        {
            Field = field;
            OrderType = orderType;
            OrderIndex = index;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public partial class SqlOrder
    {
        public string GetOrderString()
        {
            try
            {
                return GetOrderString(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetOrderString(SqlOrder record)
        {
            try
            {
                return " order by " + record.Field + ((record.OrderType == OrderTypes.Descending) ? " desc" : "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetOrderString(List<SqlOrder> records)
        {
            try
            {
                if (records == null || records.Count == 0) return "";
                records = records.OrderBy(x => x.OrderIndex).ToList();
                StringBuilder sb = new StringBuilder();
                sb.Append(" order by ");
                for (int i = 0; i < records.Count; i++)
                {
                    if (i != 0) sb.Append(", ");
                    sb.Append(records[i].Field);
                    if (records[i].OrderType == OrderTypes.Descending) sb.Append(" desc");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetOrderString(params SqlOrder[] records)
        {
            try
            {
                if (records == null || records.Count() == 0) return "";
                else return GetOrderString(records.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
