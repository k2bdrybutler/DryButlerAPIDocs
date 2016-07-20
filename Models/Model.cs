using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class Model : K2Facade.EntityBase
    {
        public string ModelName { get; set; }
    }

    public partial class Model
    {
        public static List<Model> SelectAll()
        {
            try
            {
                return K2Facade.Facade.SelectAll<Model>(0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Model - SelectAll", ex);
                return null;
            }
        }

        public static Model SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<Model>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Model - SelectByID", ex);
                return null;
            }
        }

        private List<ModelParameter> _ModelParameters;

        public List<ModelParameter> ModelParameters
        {
            get
            {
                try
                {
                    if (_ModelParameters == null) _ModelParameters = K2Facade.Facade.Search<ModelParameter>(0, new K2Facade.SqlFilter("ModelID", ID));
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