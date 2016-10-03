using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class DBResponse : K2Facade.EntityBase
    {
        public int ResponseCategory { get; set; }
        public int ResponseCode { get; set; }
        public int ResponseSubCode { get; set; }
        public string ResponseText { get; set; }
        public bool IncludeException { get; set; }

        public virtual string ResponseDisplay
        {
            get
            {
                return ResponseCode + "." + ResponseSubCode;
            }
        }
    }

}