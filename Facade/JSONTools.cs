using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace K2Facade
{
    public class JSONParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ValueType { get; set; }

        public JSONParameter()
        {

        }

        public JSONParameter(string name, string value)
        {
            Name = name;
            Value = value;
            ValueType = 1;
        }

        public JSONParameter(string name, string value, int valueType)
        {
            Name = name;
            Value = value;
            ValueType = valueType;
        }

        public static List<JSONParameter> GetParameters(string prmString)
        {
            try
            {
                var prmList = JSONTools.DeserializeObjectList<JSONParameter>(prmString);
                if (prmList != null && prmList.Count > 0)
                {
                    var donecek = new List<JSONParameter>();
                    int i; double d; DateTime dt; bool b;
                    foreach (var item in prmList)
                    {
                        switch ((JSONParameterValueTypes)item.ValueType)
                        {
                            case JSONParameterValueTypes.Integer:
                                if (!int.TryParse(item.Value, out i)) throw new InvalidCastException("'" + item.Name + "' Parameters Value Is Invalid!");
                                break;
                            case JSONParameterValueTypes.Double:
                                if (!double.TryParse(item.Value, out d)) throw new InvalidCastException("'" + item.Name + "' Parameters Value Is Invalid!");
                                break;
                            case JSONParameterValueTypes.DateTime:
                                if (!DateTime.TryParse(item.Value, out dt)) throw new InvalidCastException("'" + item.Name + "' Parameters Value Is Invalid!");
                                break;
                            case JSONParameterValueTypes.Boolean:
                                if (!bool.TryParse(item.Value, out b)) throw new InvalidCastException("'" + item.Name + "' Parameters Value Is Invalid!");
                                break;
                        }
                    }
                    return prmList;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Tools.WriteLog("JSONParameter - GetParameters", ex);
                return null;
            }
        }
    }

    public enum JSONParameterValueTypes : int
    {
        String = 1,
        Integer = 2,
        Double = 3,
        DateTime = 4,
        Boolean = 5
    }

    public class JSONTools
    {
        public static string SerializeSingleObject(object record)
        {
            try
            {
                return JsonConvert.SerializeObject(record, Formatting.None);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string SerializeObjectList<T>(IEnumerable<T> recordList)
        {
            try
            {
                return JsonConvert.SerializeObject(recordList, Formatting.Indented);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T DeserializeSingleObject<T>(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T DeserializeSingleObject<T>(Stream jsonStream)
        {
            try
            {
                var sr = new StreamReader(jsonStream);
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> DeserializeObjectList<T>(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> DeserializeObjectList<T>(Stream jsonStream)
        {
            try
            {
                var sr = new StreamReader(jsonStream);
                return JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
