using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace K2Facade
{
    public interface IEntityBase : IDisposable
    {
        int ID { get; set; }
        bool? Status { get; set; }
        int? CUser { get; set; }
        string CDate { get; set; }
        int? UUser { get; set; }
        string UDate { get; set; }
        int? DUser { get; set; }
        string DDate { get; set; }
    }

    [Serializable]
    public abstract class EntityBase : IEntityBase
    {
        public int ID { get; set; }
        public bool? Status { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? CUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string CDate { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? UUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string UDate { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? DUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string DDate { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    [Serializable]
    public abstract class EntityBaseWithUID : IEntityBase
    {
        public int ID { get; set; }
        public string UniqID { get; set; }
        public bool? Status { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? CUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string CDate { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? UUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string UDate { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public int? DUser { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        [JsonIgnore]
        public string DDate { get; set; }

        public static string GenerateUniqID()
        {
            return Guid.NewGuid().ToString("N").ToUpper().Substring(0, 10);
        }

        public string GenerateUID()
        {
            return Guid.NewGuid().ToString("N").ToUpper().Substring(0, 10);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
