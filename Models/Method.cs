using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class Method : K2Facade.EntityBase
    {
        public int ServiceID { get; set; }
        public int MethodCode { get; set; }
        public string MethodName { get; set; }
        public string Description { get; set; }
        public bool NullAuthorizationResult { get; set; }
        public int? RequestModelID { get; set; }
        public int? ResponseModelID { get; set; }
    }

    public partial class Method
    {
        public static Method SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<Method>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Method - SelectByID", ex);
                return null;
            }
        }

        private Model _RequestModel;

        public Model RequestModel
        {
            get
            {
                try
                {
                    if (_RequestModel == null && RequestModelID.HasValue)
                        _RequestModel = K2Facade.Facade.SelectByID<Model>(RequestModelID.Value, 0);
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("Method - RequestModel", ex);
                }
                return _RequestModel;
            }
        }
    }
}