using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class ModelParameter : K2Facade.EntityBase
    {
        public int ModelID { get; set; }
        public int ParameterID { get; set; }
        public int ObjectType { get; set; }
        public int? ObjectModelID { get; set; }
        public bool Nullable { get; set; }

        public virtual bool Required { get { return Nullable == false; } }
    }

    public partial class ModelParameter
    {
        private Parameter _Parameter;

        public Parameter Parameter
        {
            get
            {
                try
                {
                    if (_Parameter == null) _Parameter = K2Facade.Facade.SelectByID<Parameter>(ParameterID, 0);
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("ModelParameter - Parameter", ex);
                }
                return _Parameter;
            }
        }

        private Model _ObjectModel;

        public Model ObjectModel
        {
            get
            {
                try
                {
                    if (_ObjectModel == null && ObjectModelID.HasValue) _ObjectModel = K2Facade.Facade.SelectByID<Model>(ObjectModelID.Value, 0);
                }
                catch (Exception ex)
                {
                    K2Facade.Tools.WriteLog("ModelParameter - ObjectModel", ex);
                }
                return _ObjectModel;
            }
        }

        public string ObjectModelName
        {
            get
            {
                return (ObjectModel != null) ? ObjectModel.ModelName : null;
            }
        }
    }

}