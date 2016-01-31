using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Model;
using TechnicalTest.Domain.Repositories;
using TechnicalTest.Domain.Services;
using TechnicalTest.Mvc.Controllers;

namespace TechnicalTest.Mvc
{

    public class PoorManControllerFactory : DefaultControllerFactory
    {
        IServiceAccount _serviceAccount;
        public PoorManControllerFactory(IServiceAccount serviceAccount)
        {
            if (serviceAccount == null) throw new Exception("serviceAccount cannot be null");
            _serviceAccount = serviceAccount;
        }
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            switch (controllerName)
            {
                case "CharacterController":
                    return new CharacterController(_serviceAccount);
                default:
                    return base.CreateController(requestContext, controllerName);
            }
        }
    }
        public class MvcApplication : System.Web.HttpApplication
    {
        

        protected void Application_Start()
        {
            

            Action<Account> saveUserToSession = (account) =>
            {
                HttpContext.Current.Session.Remove("account");
                HttpContext.Current.Session.Add("account", account);
            };
            Func<Account> getFromSession = () => (Account)HttpContext.Current.Session["account"];

            var memoryDataSourceAccount = new Dictionary<string, Account>();
            
            //sample initialization
            memoryDataSourceAccount.Add("wow", new Account() { Name = "wow", Password = "wow" });

            var repoAccount = new SimpleRepoAccount(memoryDataSourceAccount);
            var serviceAccount = new ServiceAccount(repoAccount, saveUserToSession,getFromSession);

            var filterConfig = new FilterConfig(serviceAccount);

            var controllerFactory = new PoorManControllerFactory(serviceAccount);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            filterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
