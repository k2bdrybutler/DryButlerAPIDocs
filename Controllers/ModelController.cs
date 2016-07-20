using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryButlerAPIDocs.Controllers
{
    public class ModelController : Controller
    {
        public ActionResult Index()
        {
            var dataModel = Models.Model.SelectAll();
            return View(dataModel);
        }

        public ActionResult Detail(int id)
        {
            var dataModel = Models.Model.SelectByID(id);
            return View(dataModel);
        }
    }
}