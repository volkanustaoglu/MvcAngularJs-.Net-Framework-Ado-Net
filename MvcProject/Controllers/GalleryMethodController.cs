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
    public class GalleryMethodController : Controller
    {
        // GET: GalleryMethod
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllGalleries()
        {
            var result = GalleryManager.Instance.GetAllGalleries();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public bool DeleteGallery(int Id)
        {
            var result = GalleryManager.Instance.DeleteGallery(Id);
            return result;
        }


        public ActionResult CreateGallery(Gallery galleries)
        {
            var result = GalleryManager.Instance.CreateGallery(galleries);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetGallery(int id)
        {
            var result = GalleryManager.Instance.GetGallery(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
        //public ActionResult UpdateGallery(Gallery entity)
        //{
        //    var result = GalleryManager.Instance.UpdateGallery(entity);
        //    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        //}

        public ActionResult GetAllGalleriesWithUsername()
        {
            var result = GalleryManager.Instance.GetAllGalleriesWithUsername();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetGalleryWithUsername(int id)
        {
            var result = GalleryManager.Instance.GetGalleryWithUsername(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
        public ActionResult GetGalleryWithUsernameForUserId()
        {
            int id;
            var users = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            id = users.Data[0].Id;
            var result = GalleryManager.Instance.GetGalleryWithUsernameForUserId(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }


        [HttpPost]
        public ActionResult UpdateGalleryByUser()

        {

            string folderUrl = "";
            string folderUrlDb = "";

            Gallery gallery = JsonHelper.JsonConvert<Gallery>(Request.Form["data"]);


            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];
                var folder = "/Data/ImgGallery/";
                {

                    string fileGuId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    folderUrlDb = Path.Combine(folder, fileGuId);

                    folderUrl = Path.Combine(Server.MapPath(folder + fileGuId));

                    var fileName = gallery.Img.Split('/');

                    FileHelper.PlanFileDelete(Server.MapPath(folder), fileName[fileName.Length - 1]);

                    gallery.Img = folderUrlDb;

                    file.SaveAs(folderUrl);

                    Image oldImage = Image.FromFile(folderUrl);
                    Image tempImage = FileHelper.RefactorImage(oldImage, 150);

                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    oldImage.Dispose();
                    tempImage.Save(folderUrl, FileHelper.GetEncoder(ImageFormat.Jpeg), encoderParameters);

                    var result = GalleryManager.Instance.UpdateGalleryByUser(gallery);
                    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));


                }


            }
            else
            {
                var result = GalleryManager.Instance.UpdateGalleryByUser(gallery);
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }


        }

        public ActionResult GetGalleryByName(string id)
        {
            var result = GalleryManager.Instance.GetGalleryByName(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        [HttpPost]
        public ActionResult UpdateGallery()

        {

            string folderUrl = "";
            string folderUrlDb = "";

            Gallery gallery = JsonHelper.JsonConvert<Gallery>(Request.Form["data"]);


            if (Request.Files.Count > 0)

            {



                var file = Request.Files[0];
                var folder = "/Data/ImgGallery/";
                {

                    string fileGuId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    folderUrlDb = Path.Combine(folder, fileGuId);

                    folderUrl = Path.Combine(Server.MapPath(folder + fileGuId));

                    var fileName = gallery.Img.Split('/');

                    FileHelper.PlanFileDelete(Server.MapPath(folder), fileName[fileName.Length - 1]);

                    gallery.Img = folderUrlDb;

                    file.SaveAs(folderUrl);

                    Image oldImage = Image.FromFile(folderUrl);
                    Image tempImage = FileHelper.RefactorImage(oldImage, 250);

                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    oldImage.Dispose();
                    tempImage.Save(folderUrl, FileHelper.GetEncoder(ImageFormat.Jpeg), encoderParameters);

                    var result = GalleryManager.Instance.UpdateGallery(gallery);
                    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));


                }


            }
            else
            {
                var result = GalleryManager.Instance.UpdateGallery(gallery);
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }


        }
    }
}