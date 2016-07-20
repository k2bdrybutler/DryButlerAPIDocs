using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Facade
{
    /*
    public class User : EntityBaseWithUID
    {
        public string AdSoyad { get; set; }
        public string CepTelefonu { get; set; }
        public string Email { get; set; }
        public int RolID { get; set; }
        public virtual Rol Rol
        {
            get
            {
                return Facade.SelectByID<Rol>(this.RolID, 0);
            }
            set
            {
                this.RolID = value.ID;
            }
        }
        public string UserAdi { get; set; }
        public string Sifre { get; set; }
        public string PinKodu { get; set; }
        public bool isAdmin { get; set; }
        public string ResetPassKey { get; set; }
        public DateTime? ResetPassEnd { get; set; }

        public override string ToString()
        {
            return this.AdSoyad;
        }
    }

    public partial class Rol : EntityBase
    {
        public string RolAdi { get; set; }
        public int? MainMenuID { get; set; }
        public virtual MainMenu MainMenu
        {
            get
            {
                if (!this.MainMenuID.HasValue)
                    return null;
                else
                    return Facade.SelectByID<MainMenu>(this.MainMenuID.Value, 0);
            }
            set
            {
                if (value == null)
                    this.MainMenuID = null;
                else
                    this.MainMenuID = value.ID;
            }
        }

        public override string ToString()
        {
            return this.RolAdi;
        }
    }

    public partial class RolYetki : EntityBase
    {
        public int RolID { get; set; }
        public int YetkiTipi { get; set; }
        public int Yetki { get; set; }
        public string Deger { get; set; }
    }
    */
}
