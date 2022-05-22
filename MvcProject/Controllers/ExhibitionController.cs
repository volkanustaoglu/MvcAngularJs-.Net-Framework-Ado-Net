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
    public class ExhibitionController : Controller
    {
        // GET: Exhibition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllExhibitions()
        {
            var result = ExhibitionManager.Instance.GetAllExhibitions();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public bool DeleteExhibition(int Id)
        {
            var result = ExhibitionManager.Instance.DeleteExhibition(Id);
            return result;
        }


        public ActionResult CreateExhibition(Exhibition exhibition)
        {
            var users = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            exhibition.UserId = users.Data[0].Id;
            var result = ExhibitionManager.Instance.CreateExhibition(exhibition);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetExhibition(int id)
        {
            var result = ExhibitionManager.Instance.GetExhibition(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
        public ActionResult GetAllExhibitionsForGalleryId(int id)
        {
            var result = ExhibitionManager.Instance.GetAllExhibitionsForGalleryId(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
    }
}