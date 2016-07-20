﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DryButlerAPIDocs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            K2Facade.Tools.WriteLog("GlobalError", exception);
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;

            //if (httpException != null)
            //{
            //    Response.StatusCode = httpException.GetHttpCode();

            //    switch (Response.StatusCode)
            //    {
            //        case 403: //Eğer 403 hatası meydana gelmiş ise Http403 Action'ı devreye girecek.
            //            routeData.Values["action"] = "Http403";
            //            break;

            //        case 404: //Eğer 404 hatası meydana gelmiş ise Http404 Action'ı devreye girecek.
            //            routeData.Values["action"] = "Http404";
            //            break;
            //    }
            //}

            IController errorsController = new Controllers.ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }
    }
}
