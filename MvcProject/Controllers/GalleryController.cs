using MvcProject.Methods;
using MvcProject.Models;
using MvcProject.ResponseModel;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        [Authorize(Roles = "Gallery")]
        public ActionResult Index()
        {
            return View();
        }

       
    }
}