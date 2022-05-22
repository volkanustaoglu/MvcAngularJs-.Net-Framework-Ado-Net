using MvcProject.Methods;
using MvcProject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditorEditLogs()
        {
            return View();
        }

        public ActionResult GetAllEditorEditLogs()
        {
            var result = LogManager.Instance.GetAllEditorEditLogs();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
    }
}