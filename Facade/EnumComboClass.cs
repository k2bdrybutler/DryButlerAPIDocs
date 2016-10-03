using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K2Facade
{
    public class EnumComboClass : IDisposable
    {
        #region BaseItems

        public int ID { get; set; }
        public string DESC { get; set; }

        public EnumComboClass()
        {

        }

        public EnumComboClass(int id, string desc)
        {
            ID = id;
            DESC = desc;
        }

        public override string ToString()
        {
            return this.DESC;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        public static List<EnumComboClass> SelectAll(ProjectEnums enm, int lang = 0)
        {
            try
            {
                List<EnumComboClass> donecek = new List<EnumComboClass>();
                switch (enm)
                {
                    case ProjectEnums.PropertyTypes:
                        donecek.Add(new EnumComboClass(10, "String"));
                        donecek.Add(new EnumComboClass(20, "Integer"));
                        donecek.Add(new EnumComboClass(30, "Double"));
                        donecek.Add(new EnumComboClass(40, "Boolean"));
                        donecek.Add(new EnumComboClass(50, "Date"));
                        donecek.Add(new EnumComboClass(60, "Time"));
                        donecek.Add(new EnumComboClass(70, "Datetime"));
                        donecek.Add(new EnumComboClass(80, "Decimal"));
                        donecek.Add(new EnumComboClass(100, "JSONObject"));
                        break;
                    case ProjectEnums.DisplayTypes:
                        donecek.Add(new EnumComboClass(0, "None"));
                        donecek.Add(new EnumComboClass(10, "StatusBar"));
                        donecek.Add(new EnumComboClass(20, "Modal"));
                        donecek.Add(new EnumComboClass(30, "Field"));
                        break;
                    case ProjectEnums.ModelTypes:
                        donecek.Add(new EnumComboClass(0, "None"));
                        donecek.Add(new EnumComboClass(10, "Request"));
                        donecek.Add(new EnumComboClass(20, "Response"));
                        donecek.Add(new EnumComboClass(30, "Request / Response"));
                        break;
                }
                return donecek.OrderBy(x => x.DESC).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static EnumComboClass SelectByID(ProjectEnums enm, int id, int lang = 0)
        {
            try
            {
                return SelectAll(enm).Where(x => x.ID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum ProjectEnums
    {
        PropertyTypes,
        DisplayTypes,
        ModelTypes
    }
}