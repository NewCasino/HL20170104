using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCore.Controls;
namespace JN.Web.Areas.UserCenter.Controllers
{
    public class helloController : BaseController
    {
        //
        // GET: /UserCenter/hello/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult add(int id) {
            ReturnResult result = new ReturnResult();
            result.Message = id+"ok";
            return Json(result);
        }
	}
}