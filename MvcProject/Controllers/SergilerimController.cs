using MvcProject.Methods;
using MvcProject.Models;
using MvcProject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class SergilerimController : Controller
    {
        // GET: Sergilerim
        [Authorize(Roles = "Gallery")]
        public ActionResult Index()
        {
            return View();
        }


     
    }
}