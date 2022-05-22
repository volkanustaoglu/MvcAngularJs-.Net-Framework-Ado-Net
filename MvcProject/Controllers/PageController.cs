using MvcProject.Methods;
using MvcProject.Models;
using MvcProject.ResponseModel;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllPages()
        {
            var result = PageManager.Instance.GetAllPages();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public bool DeletePage(int Id)
        {
            var result = PageManager.Instance.DeletePage(Id);
            return result;
        }


        public ActionResult CreatePage(Page pages)
        {
            pages.PageKey = PageManager.Instance.RandomString(29);
            var result = PageManager.Instance.CreatePage(pages);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetPage(int id)
        {
            var result = PageManager.Instance.GetPage(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
      
        public ActionResult GetPagesByUserId(Page entity)
        {
            var users = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            entity.UserId = users.Data[0].Id;
            var result = PageManager.Instance.GetPagesByUserId(entity);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
        [LogAction]
        public ActionResult GetPagesByPageKey(string id)
        {
            var result = PageManager.Instance.GetPagesByPageKey(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetAllPagesWithUsername()
        {
            var result = PageManager.Instance.GetAllPagesWithUsername();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetPageWithUsername(int id)
        {
            var result = PageManager.Instance.GetPageWithUsername(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }


        [HttpPost]
        public ActionResult UpdatePageByUser()

        {

            string folderUrl = "";
            string folderUrlDb = "";

            Page page = JsonHelper.JsonConvert<Page>(Request.Form["data"]);
           

                if (Request.Files.Count > 0)

                {



                    var file = Request.Files[0];
                    var folder = "/Data/ImgPage/";
                    {

                        string fileGuId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    folderUrlDb = Path.Combine(folder, fileGuId);

                    folderUrl = Path.Combine(Server.MapPath(folder + fileGuId));

                    var fileName = page.Img.Split('/');

                    FileHelper.PlanFileDelete(Server.MapPath(folder), fileName[fileName.Length - 1]);

                    page.Img = folderUrlDb;

                        file.SaveAs(folderUrl);

                    Image oldImage = Image.FromFile(folderUrl);
                    Image tempImage = FileHelper.RefactorImage(oldImage, 250);
                  
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    oldImage.Dispose();
                    tempImage.Save(folderUrl, FileHelper.GetEncoder(ImageFormat.Jpeg), encoderParameters);

                    var result = PageManager.Instance.UpdatePageByUser(page);
                    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));


                }


            }
            else
            {
                var result = PageManager.Instance.UpdatePageByUser(page);
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }
            

        }

        [HttpPost]
        public ActionResult UpdatePage()

        {

            string folderUrl = "";
            string folderUrlDb = "";

            Page page = JsonHelper.JsonConvert<Page>(Request.Form["data"]);


            if (Request.Files.Count > 0)

            {



                var file = Request.Files[0];
                var folder = "/Data/ImgPage/";
                {

                    string fileGuId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    folderUrlDb = Path.Combine(folder, fileGuId);

                    folderUrl = Path.Combine(Server.MapPath(folder + fileGuId));

                    var fileName = page.Img.Split('/');

                    FileHelper.PlanFileDelete(Server.MapPath(folder), fileName[fileName.Length - 1]);

                    page.Img = folderUrlDb;

                    file.SaveAs(folderUrl);

                    Image oldImage = Image.FromFile(folderUrl);
                    Image tempImage = FileHelper.RefactorImage(oldImage, 250);

                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    oldImage.Dispose();
                    tempImage.Save(folderUrl, FileHelper.GetEncoder(ImageFormat.Jpeg), encoderParameters);

                    var result = PageManager.Instance.UpdatePage(page);
                    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));


                }


            }
            else
            {
                var result = PageManager.Instance.UpdatePage(page);
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }


        }

        public ActionResult GetPageWithExhibition(int id)
        {
            var result = PageManager.Instance.GetPageWithExhibition(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
    }
}