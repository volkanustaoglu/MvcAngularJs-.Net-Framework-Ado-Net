using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class GaleriAyarlariController : Controller
    {
        // GET: GaleriAyarlari
        [Authorize(Roles ="Gallery")]
        public ActionResult Index()
        {
            return View();
        }
    }
}