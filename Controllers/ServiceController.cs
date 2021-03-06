﻿using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace DryButlerAPIDocs.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Index(string searchString, int? page)
        {
            var dataModel = Models.DBService.SelectAll(searchString);
            return View(dataModel.ToPagedList(page ?? 1, GeneralItems.PageRowCount));
        }

        public ActionResult Details(int id, string searchString, int? page)
        {
            var dataModel = Models.DBService.SelectByID(id);
            var data = (dataModel != null) ? dataModel.Methods : null;
            if (data == null || data.Count == 0) return View();
            if (!string.IsNullOrEmpty(searchString)) data = data.Where(x => x.MethodCode.ToString() == searchString
            || x.MethodName.ToLower().Contains(searchString.ToLower())).ToList();
            var returnModel = new ServiceDetailModel()
            {
                ServiceID = dataModel.ID,
                ServiceName = dataModel.Name,
                ServiceTitle = dataModel.ID.ToString() + " - " + dataModel.Name,
                Methods = data.ToPagedList(page ?? 1, GeneralItems.PageRowCount)
            };
            return View(returnModel);
        }
    }

    public class ServiceDetailModel
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTitle { get; set; }
        public IPagedList<Models.DBMethod> Methods { get; set; }
    }
}
