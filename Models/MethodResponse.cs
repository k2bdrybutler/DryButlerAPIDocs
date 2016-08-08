using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class MethodResponse : K2Facade.EntityBase
    {
        public int Service { get; set; }
        public int Method { get; set; }
        public int Level { get; set; }
        public int ResponseID { get; set; }
        public virtual Response Response
        {
            get
            {
                return K2Facade.Facade.SelectByID<Response>(this.ResponseID, 0);
            }
            set
            {
                ResponseID = value.ID;
            }
        }
        public int DisplayType { get; set; }
    }
}