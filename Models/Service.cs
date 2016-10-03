using System;
using System.Collections.Generic;

namespace DryButlerAPIDocs.Models
{
    public partial class DBService : K2Facade.EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowInAPIDocs { get; set; }

        public static List<DBService> SelectAll(string searchString)
        {
            try
            {
                return (!string.IsNullOrEmpty(searchString))
                    ? K2Facade.Facade.Search<DBService>(0, new K2Facade.SqlOrder("ID"), new K2Facade.SqlFilter("Name", searchString, K2Facade.FilterOperators.Like))
                    : K2Facade.Facade.Search<DBService>(0, new K2Facade.SqlOrder("ID"), new K2Facade.SqlFilter("ShowInAPIDocs", true));
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Service - SelectAll", ex);
                return null;
            }
        }

        public static DBService SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<DBService>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Service - SelectByID", ex);
                return null;
            }
        }

        private List<DBMethod> _Methods;

        public virtual List<DBMethod> Methods
        {
            get
            {
                try
                {
                    if (_Methods == null) _Methods = K2Facade.Facade.Search<DBMethod>(0, new K2Facade.SqlOrder("MethodCode"), 
                        new K2Facade.SqlFilter("DBServiceID", ID), new K2Facade.SqlFilter("ShowInAPIDocs", true));
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("Service - Methods", ex);
                }
                return _Methods;
            }
        }
    }
}