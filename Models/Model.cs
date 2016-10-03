using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class DBAPIModel : K2Facade.EntityBase
    {
        public string UniqID { get; set; }
        public string ModelName { get; set; }
        public int ModelType { get; set; }

        public static List<DBAPIModel> SelectAll(string searchString)
        {
            try
            {
                return (!string.IsNullOrEmpty(searchString))
                    ? K2Facade.Facade.Search<DBAPIModel>(0, new K2Facade.SqlFilter("ModelName", searchString, K2Facade.FilterOperators.Like))
                    : K2Facade.Facade.SelectAll<DBAPIModel>(0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Model - SelectAll", ex);
                return null;
            }
        }

        public static DBAPIModel SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<DBAPIModel>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Model - SelectByID", ex);
                return null;
            }
        }

        private List<DBAPIModelParameter> _ModelParameters;

        public virtual List<DBAPIModelParameter> ModelParameters
        {
            get
            {
                try
                {
                    if (_ModelParameters == null) _ModelParameters = K2Facade.Facade.Search<DBAPIModelParameter>(0, new K2Facade.SqlFilter("DBAPIModelID", ID));
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("Model - ModelParameters", ex);
                }
                return _ModelParameters;
            }
        }

    }
}