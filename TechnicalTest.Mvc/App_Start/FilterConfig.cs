using System;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.Domain.Services;
using TechnicalTest.Mvc.ActionFilters;

namespace TechnicalTest.Mvc
{
    public class FilterConfig
    {
        private IServiceAccount _serviceAccount;

        public FilterConfig(IServiceAccount serviceAccount)
        {
            if (serviceAccount == null) throw new Exception("serviceAccount cannot be null");

            _serviceAccount = serviceAccount;
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BasicAuthenticationFilter(_serviceAccount));
        }
    }
}
