using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class DBAPIModelParameter : K2Facade.EntityBase
    {
        public int DBAPIModelID { get; set; }
        public string ModelUniqID { get; set; }
        public string ParameterName { get; set; }
        public int ParameterType { get; set; }
        public int ObjectType { get; set; }
        public string ValueRange { get; set; }
        public string SampleValue { get; set; }
        public string ObjectModelID { get; set; }
        public bool Nullable { get; set; }
        public string Description { get; set; }

        public virtual bool Required { get { return Nullable == false; } }

        private DBAPIModel _ObjectModel;

        public virtual DBAPIModel ObjectModel
        {
            get
            {
                try
                {
                    if (_ObjectModel == null && !string.IsNullOrEmpty(ObjectModelID))
                        _ObjectModel = K2Facade.Facade.GetFirst<DBAPIModel>(0, new K2Facade.SqlFilter("UniqID", ObjectModelID));
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("ModelParameter - ObjectModel", ex);
                }
                return _ObjectModel;
            }
        }

        public virtual string ObjectModelName
        {
            get
            {
                return (ObjectModel != null) ? ObjectModel.ModelName : null;
            }
        }

        public virtual string ParameterTypeDesc
        {
            get
            {
                return ((ObjectType == 20) ? "Array of " : "") + K2Facade.EnumComboClass.SelectByID(K2Facade.ProjectEnums.PropertyTypes, ParameterType, 0).DESC;
            }
        }
    }
}