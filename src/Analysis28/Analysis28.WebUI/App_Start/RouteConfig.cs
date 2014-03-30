using System.Configuration;
using System.Web.Routing;
using Common.Utility.Web.Mvc;

namespace Analysis28.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteConfigurationSection section =
                (RouteConfigurationSection)ConfigurationManager.GetSection("routeConfig");

            routes.MapRoute(section);   
        }
    }
}