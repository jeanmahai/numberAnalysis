﻿using System.Web;
using System.Web.Mvc;
using Soho.Utility.Web.Framework;

namespace SohoWeb.WebMgt
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}