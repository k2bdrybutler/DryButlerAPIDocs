using System;
using System.Collections.Generic;

namespace DryButlerAPIDocs.Models
{
    public partial class DBMethod : K2Facade.EntityBase
    {
        public int DBServiceID { get; set; }
        public int MethodCode { get; set; }
        public string MethodName { get; set; }
        public string Description { get; set; }
        public bool NullAuthorizationResult { get; set; }
        public string RequestModelID { get; set; }
        public string ResponseModelID { get; set; }
        public int? ResponseObjectType { get; set; }
        public bool ShowInAPIDocs { get; set; }

        public static DBMethod SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<DBMethod>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Method - SelectByID", ex);
                return null;
            }
        }

        private DBAPIModel _RequestModel;

        public virtual DBAPIModel RequestModel
        {
            get
            {
                try
                {
                    if (_RequestModel == null && !string.IsNullOrEmpty(RequestModelID))
                        _RequestModel = K2Facade.Facade.GetFirst<DBAPIModel>(0, new K2Facade.SqlFilter("UniqID", RequestModelID));
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("Method - RequestModel", ex);
                }
                return _RequestModel;
            }
        }

        private DBAPIModel _ResponseModel;

        public virtual DBAPIModel ResponseModel
        {
            get
            {
                try
                {
                    if (_ResponseModel == null && !string.IsNullOrEmpty( ResponseModelID))
                        _ResponseModel = K2Facade.Facade.GetFirst<DBAPIModel>(0, new K2Facade.SqlFilter("UniqID", ResponseModelID));
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("Method - Response", ex);
                }
                return _ResponseModel;
            }
        }

        private List<DBMethodResponse> _Responses;

        public virtual List<DBMethodResponse> Responses
        {
            get
            {
                if (_Responses == null || _Responses.Count == 0)
                {
                    _Responses = K2Facade.Facade.Search<DBMethodResponse>(0, new K2Facade.SqlOrder("Level"), new K2Facade.SqlFilter("Service", DBServiceID), new K2Facade.SqlFilter("Method", MethodCode));
                }
                return _Responses;
            }
        }
    }
}