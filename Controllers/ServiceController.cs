using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace DryButlerAPIDocs.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Index(string searchString, int? page)
        {
            var dataModel = Models.Service.SelectAll(searchString);
            return View(dataModel.ToPagedList(page ?? 1, GeneralItems.PageRowCount));
        }

        public ActionResult Details(int id, string searchString, int? page)
        {
            var dataModel = Models.Service.SelectByID(id);
            var data = (dataModel != null) ? dataModel.Methods : null;
            if (!string.IsNullOrEmpty(searchString)) data = data.Where(x => x.MethodCode.ToString() == searchString
            || x.MethodName.ToLower().Contains(searchString.ToLower())).ToList();
            return View(data.ToPagedList(page ?? 1, GeneralItems.PageRowCount));
        }
    }
}
