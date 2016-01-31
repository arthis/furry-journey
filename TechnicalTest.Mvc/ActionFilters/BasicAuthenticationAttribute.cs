using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.Domain.Services;

namespace TechnicalTest.Mvc.ActionFilters
{
    public class BasicAuthenticationFilter :IActionFilter
    {
        IServiceAccount _serviceAccount;
        public string BasicRealm { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public BasicAuthenticationFilter(IServiceAccount serviceAccount)
        {
            if (serviceAccount == null) throw new Exception("serviceAccount cannot be null");

            _serviceAccount = serviceAccount;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!String.IsNullOrEmpty(auth))
            {
                var cred = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                if (_serviceAccount.authenticate(cred[0], cred[1])) return;
            }
            var res = filterContext.HttpContext.Response;
            res.StatusCode = 401;
            res.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Ryadel"));
            res.End();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }

    public class BasicAuthenticationAttribute : Attribute   { }
}