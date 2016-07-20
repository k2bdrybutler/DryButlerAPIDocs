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
                donecek.Add(new EnumComboClass(0, "Belirsiz"));
                switch (enm)
                {
                    case ProjectEnums.PropertyTypes:
                        donecek.Add(new EnumComboClass(1, "String"));
                        donecek.Add(new EnumComboClass(2, "Int32"));
                        donecek.Add(new EnumComboClass(3, "Datetime"));
                        donecek.Add(new EnumComboClass(4, "Boolean"));
                        donecek.Add(new EnumComboClass(5, "JSONObject"));
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
        PropertyTypes
    }
}