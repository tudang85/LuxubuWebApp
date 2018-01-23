using Luxubu.Types;
using Luxubu.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Luxubu.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Controller objController = context.Controller as Controller;
            string strActionName = objController.ControllerContext.RouteData.Values["action"].ToString();
            if(!strActionName.Equals("login", StringComparison.InvariantCultureIgnoreCase))
            {
                SessionData objSSData = Session.GetSession(context.HttpContext);
                if (objSSData == null)
                {
                    context.Result = objController.RedirectToAction("Login", "Home");
                    return;
                }
            }            
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public void OnException(ExceptionContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
