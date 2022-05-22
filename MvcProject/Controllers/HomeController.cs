using MvcProject.Methods;
using MvcProject.ResponseModel;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[LogAction]
    
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Page()
        {
            return View();
        }


    }
}