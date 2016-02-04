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
        IServiceUser _serviceUser;

        public PoorManControllerFactory(IServiceUser serviceUser, IServiceAccount serviceAccount)
        {
            if (serviceUser == null) throw new ArgumentNullException("serviceUser cannot be null");
            if (serviceAccount == null) throw new ArgumentNullException("serviceAccount cannot be null");

            _serviceAccount = serviceAccount;
            _serviceUser = serviceUser;
        }
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            switch (controllerName)
            {
                case "Account":
                    return new AccountController(_serviceUser, _serviceAccount);
                default:
                    return base.CreateController(requestContext, controllerName);
            }
        }
    }
        public class MvcApplication : System.Web.HttpApplication
    {
        

        protected void Application_Start()
        {
            

            Action<User> saveUserToSession = (account) =>
            {
                HttpContext.Current.Session.Remove("User");
                HttpContext.Current.Session.Add("User", account);
            };
            Func<User> getFromSession = () => (User)HttpContext.Current.Session["User"];

            var memoryDataSourceUser = new Dictionary<string, User>();

            //sample initialization
            memoryDataSourceUser.Add("wow", new User() { Id = Guid.Parse("537c8609-ddbe-434f-9e74-b2f55b9a35da"),  Name = "wow", Password = "wow" });
            memoryDataSourceUser.Add("wah", new User() { Id = Guid.Parse("2939c3e6-0ee6-4167-a8aa-f5e95e58edee"),  Name = "wah", Password = "wah" }); 
            

            //var repoAccount = new MemoryRepoAccount(new Dictionary<Guid, Account>());
            var repoAccount = new FileSystemRepoAccount();
            var serviceAccount = new ServiceAccount(repoAccount);

            var repoUser = new MemoryRepoUser(memoryDataSourceUser);
            var serviceUser = new ServiceUser(repoUser, saveUserToSession, getFromSession);

            var filterConfig = new FilterConfig(serviceUser);

            var controllerFactory = new PoorManControllerFactory(serviceUser,serviceAccount);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            AreaRegistration.RegisterAllAreas();
            filterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
