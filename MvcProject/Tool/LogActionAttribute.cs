using MvcProject.Methods;
using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Tool
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)

        {



            var Controller = filterContext.RequestContext.RouteData.Values["Controller"];

            var Action = filterContext.RequestContext.RouteData.Values["Action"];

            var users = UserManager.Instance.GetUserByName(System.Web.HttpContext.Current.User.Identity.Name);

            var UserId = users.Data[0].Id;
            var Role = users.Data[0].Role;
            DateTime time = DateTime.Now;
            var CreateDate = time;

            var setResult = new Log() {
                Controller = Controller.ToString(),
                Action = Action.ToString(),
                UserId = UserId,
                Role = Role,
                CreateDate = CreateDate


            };

            LogManager.Instance.CreateLog(setResult);



            base.OnActionExecuting(filterContext);

        }
    }
}