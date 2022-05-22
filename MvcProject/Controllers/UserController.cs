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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult GetAllUsers()
        {
            var result = UserManager.Instance.GetAllUsers();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public bool DeleteUser(User userId)
        {
            var result = UserManager.Instance.DeleteUser(userId);
            return result;
        }


        public ActionResult CreateUser(User users)
        {
            users.UserKey = PageManager.Instance.RandomString(29);
            users.Password = SecurityManager.Instance.GetPasswordHash(users);


            var result = UserManager.Instance.CreateUser(users);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetUser(int id)
        {
            var result = UserManager.Instance.GetUser(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }
        public ActionResult UpdateUser(User entity)
        {
            if (entity.Password != null)
            {
                entity.Password = SecurityManager.Instance.GetPasswordHash(entity);
            }
         

            var result = UserManager.Instance.UpdateUser(entity);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }


        public ActionResult GetUserForEdit()
        {
            int id;
            var users = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            id = users.Data[0].Id;
            var result = UserManager.Instance.GetUser(id);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetAllUsersRoleGallery()
        {
            var result = UserManager.Instance.GetAllUsersRoleGallery();
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetOnlineUserRole()
        {
           
            var result = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }


    }
}