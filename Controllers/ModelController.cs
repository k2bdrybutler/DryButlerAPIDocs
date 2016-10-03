using System.Web.Mvc;
using PagedList;

namespace DryButlerAPIDocs.Controllers
{
    public class ModelController : Controller
    {
        public ActionResult Index(string searchString, int? page)
        {

            var dataModel = Models.DBAPIModel.SelectAll(searchString);
            return View(dataModel.ToPagedList(page ?? 1, GeneralItems.PageRowCount));
        }

        public ActionResult Details(string id)
        {
            var dataModel = Models.DBAPIModel.SelectByID(id);
            return View(dataModel);
        }
    }
}