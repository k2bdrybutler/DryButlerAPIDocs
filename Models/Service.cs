using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class Service : K2Facade.EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class Service
    {
        public static List<Service> SelectAll(string searchString)
        {
            try
            {
                return (!string.IsNullOrEmpty(searchString))
                    ? K2Facade.Facade.Search<Service>(0, new K2Facade.SqlFilter("Name", searchString, K2Facade.FilterOperators.Like))
                    : K2Facade.Facade.SelectAll<Service>(0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Service - SelectAll", ex);
                return null;
            }
        }

        public static Service SelectByID(int id)
        {
            try
            {
                return K2Facade.Facade.SelectByID<Service>(id, 0);
            }
            catch (Exception ex)
            {
                K2Facade.Tools.WriteLog("Service - SelectByID", ex);
                return null;
            }
        }

        private List<Method> _Methods;

        public List<Method> Methods
        {
            get
            {
                try
                {
                    if (_Methods == null) _Methods = K2Facade.Facade.Search<Method>(0, new K2Facade.SqlFilter("ServiceID", ID));
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