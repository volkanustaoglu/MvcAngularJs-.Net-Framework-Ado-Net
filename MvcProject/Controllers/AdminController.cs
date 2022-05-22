using System.Web.Mvc;

namespace MvcProject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
      
        [Authorize(Roles = "Admin")]
        public ActionResult UserAdmin()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        public ActionResult PageAdmin()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CreatePage()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditPage()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditPageByUser()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GalleryAdmin()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult CreateGallery()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditGallery()
        {
            return View();
        }
        public ActionResult ExhibitionAdmin()
        {
            return View();
        }
        
    }
}