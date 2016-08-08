using System.Web.Mvc;
using PagedList;

namespace DryButlerAPIDocs.Controllers
{
    public class ModelController : Controller
    {
        public ActionResult Index(string searchString, int? page)
        {

            var dataModel = Models.Model.SelectAll(searchString);
            return View(dataModel.ToPagedList(page ?? 1, GeneralItems.PageRowCount));
        }

        public ActionResult Details(int id)
        {
            var dataModel = Models.Model.SelectByID(id);
            return View(dataModel);
        }
    }
}