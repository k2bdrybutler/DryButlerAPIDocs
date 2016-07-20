using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryButlerAPIDocs.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            var dataModel = Models.Service.SelectAll();
            return View(dataModel);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            var dataModel = Models.Service.SelectByID(id);
            if (dataModel != null) return View(dataModel);
            else return Index();
        }
    }
}
