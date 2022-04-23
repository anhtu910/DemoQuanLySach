using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAppDemo
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute(
				name: "AdminLogin",
				url: "Admin/login",
				defaults: new { controller = "Auth", action = "Login", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "AdminRegister",
				url: "Admin/Register",
				defaults: new { controller = "Auth", action = "Register", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}
