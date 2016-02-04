using System;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.Domain.Services;
using TechnicalTest.Mvc.ActionFilters;

namespace TechnicalTest.Mvc
{
    public class FilterConfig
    {
        private IServiceUser _serviceUser;

        public FilterConfig(IServiceUser serviceUser)
        {
            if (serviceUser == null) throw new ArgumentNullException("serviceUser cannot be null");

            _serviceUser = serviceUser;
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BasicAuthenticationFilter(_serviceUser));
        }
    }
}
