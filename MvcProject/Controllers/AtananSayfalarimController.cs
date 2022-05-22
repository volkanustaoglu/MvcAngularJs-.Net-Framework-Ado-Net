using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class AtananSayfalarimController : Controller
    {
        // GET: AtananSayfalarim
        [Authorize(Roles = "Editor,Gallery")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Editor,Gallery")]
        public ActionResult Duzenleme()
        {
            return View();
        }
    }
}