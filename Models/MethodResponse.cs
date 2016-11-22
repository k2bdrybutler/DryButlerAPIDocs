namespace DryButlerAPIDocs.Models
{
    public partial class DBMethodResponse : K2Facade.EntityBase
    {
        public int Service { get; set; }
        public int Method { get; set; }
        public int Level { get; set; }
        public string Control { get; set; }
        public int DBResponseID { get; set; }
        public virtual DBResponse Response
        {
            get
            {
                return K2Facade.Facade.SelectByID<DBResponse>(this.DBResponseID, 0);
            }
            set
            {
                DBResponseID = value.ID;
            }
        }
        public int? ResponseType { get; set; }
        public int DisplayTypeAnd { get; set; }
        public int DisplayTypeIOS { get; set; }
        public int DisplayTypeWeb { get; set; }
    }
}