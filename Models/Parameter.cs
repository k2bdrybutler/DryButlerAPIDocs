using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryButlerAPIDocs.Models
{
    public partial class Parameter : K2Facade.EntityBase
    {
        public string ParameterName { get; set; }
        public int ParameterType { get; set; }
        public string ValueRange { get; set; }
        public string SampleValue { get; set; }
    }
}