using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryButlerAPIDocs.Controllers
{
    public class MethodController : Controller
    {
        public ActionResult Index(int id)
        {
            var dataModel = Models.Method.SelectByID(id);
            return View(dataModel);
        }
    }
}
