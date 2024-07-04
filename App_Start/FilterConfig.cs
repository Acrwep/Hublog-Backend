using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EMP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new System.Web.Http.AuthorizeAttribute());
        }
    }
}
