using MvcProject.EmailServices;
using MvcProject.Methods;
using MvcProject.Models;
using MvcProject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcProject.Controllers
{
    public class SecurityController : Controller
    {

        Random rnd = new Random();
        string confirmPhoneCode;
        
        // GET: Security

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendPhoneMessage(User entity)
        {
            confirmPhoneCode = rnd.Next(10).ToString() + rnd.Next(10).ToString() + rnd.Next(10).ToString() + rnd.Next(10).ToString() + rnd.Next(10).ToString() + rnd.Next(10).ToString();

            SecurityManager.Instance.SendPhoneMessage(entity.PhoneNumber, confirmPhoneCode + " www.curatortag.com için doğrulama kodunuz.");

            return null;
        }

        public ActionResult Login(User users)
        {
            users.Password = SecurityManager.Instance.GetPasswordHash(users);
            var result = SecurityManager.Instance.GetLoginUser(users);
            if (result.Data.Count ==0 || result.Data==null)
            {
                return Json(false);
            }
            else
            {
                //if (result.Data[0].ConfirmEmail ==false)
                //{
                //    result.Message = "Lütfen Email Adresinizi onaylayınız.";
                //    return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
                //}
                FormsAuthentication.SetAuthCookie(result.Data[0].Username, users.RememberMe);
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }
           

        }
       
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register(User users)
        {

            users.UserKey = PageManager.Instance.RandomString(29);
            users.Password = SecurityManager.Instance.GetPasswordHash(users);
            if (users.PhoneConfirmCode == users.PhoneConfirmCode)
            {
                var result = UserManager.Instance.CreateUser(users);


                var url = Url.Action("ConfirmEmail", "Security", new
                {

                    userId = result.ReturnId,
                    token = users.UserKey
                });
                if (result.IsSuccess == true)
                {

                   SmtpEmailSender.MailSender(users.Email, "Hesabınızı Onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://goofyduck.online{url}'>tıklayınız.</a>");
                }
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }
            return null;
           
   
            
        }

      
      
        public ActionResult ConfirmEmail(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Unauthorized", "Security");
            }
            
            var user = UserManager.Instance.GetUser(Convert.ToInt32(userId));

            if (user.Data.Count == 0)
            {
                return RedirectToAction("Unauthorized", "Security");
            }
            else
            {
                if (user.Data[0].UserKey == token)
                {
                    var result = UserManager.Instance.UpdateUserEmailConfirm(userId,token);

                    return View();
                }
                return RedirectToAction("Unauthorized", "Security");
            }
            
        }

        public ActionResult GetUserByEmail(User entity)
        {
            var result = SecurityManager.Instance.GetUserByEmail(entity);

            if (result.Data.Count == 0)
            {
                result.Message = "Mail Adresiniz Sistemimizde Kayıtlı Değildir.";
                result.IsSuccess = false;
                return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
            }
            var url = Url.Action("ResetPassword", "Security", new
            {

                userId = result.Data[0].Id,
                token = result.Data[0].UserKey
            });

            SmtpEmailSender.MailSender(entity.Email, "Şifrenizi Değiştiriniz.", $"Lütfen şifrenizi değiştirmek için linke <a href='https://goofyduck.online{url}'>tıklayınız.</a>");
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult ResetPassword(string userId, string token)
        {

            if (userId == null || token == null)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            var user = UserManager.Instance.GetUser(Convert.ToInt32(userId));

            if (user.Data.Count == 0)
            {
                return RedirectToAction("Unauthorized", "Security");
            }
            else
            {
                if (user.Data[0].UserKey == token)
                {

                    return View();
                }
                return RedirectToAction("Unauthorized", "Security");
            }
          
        }

        public ActionResult ResetPasswordNow(User entity)
        {
            entity.Password = SecurityManager.Instance.GetPasswordHash(entity);
            var result = SecurityManager.Instance.UpdateUserPasswordByUserIdToken(entity);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult GetUserByEmailForCheck(User entity)
        {
            var result = SecurityManager.Instance.GetUserByEmail(entity);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

        public ActionResult GetUserByNameForCheck(User entity)
        {
            string username = entity.Username;
            var result = UserManager.Instance.GetUserByName(username);
            return Content(ResultData.Get(result.IsSuccess, result.Message, result.Data));
        }

    }
}