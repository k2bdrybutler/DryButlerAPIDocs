using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Facade
{
    public interface IConnectionSettings : IDisposable
    {
        string Server { get; set; }
        string Database { get; set; }
        bool UseSSPI { get; set; }
        string UserID { get; set; }
        string Password { get; set; }
    }
}
