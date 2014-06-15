using System.Web.Routing;
using System.Configuration;
using Soho.Utility.Web.Mvc;

namespace SohoWeb.WebMgt
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteConfigurationSection section = (RouteConfigurationSection)ConfigurationManager.GetSection("routeConfig");
            routes.MapRoute(section);
        }
    }
}