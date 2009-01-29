﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSharp.MvcExtensions.Handlers
{
	public class BlogMvcHandler : System.Web.Mvc.MvcHandler
	{

		public BlogMvcHandler(RequestContext requestContext)
			: base(requestContext)
		{
		}
		protected override void ProcessRequest(System.Web.HttpContext httpContext)
		{
			HttpContextBase iHttpContext = new HttpContextWrapper(httpContext);
			ProcessRequest(iHttpContext);
		}

		protected override void ProcessRequest(System.Web.HttpContextBase httpContext)
		{
			IExtendedControllerFactory factory = ControllerBuilder.Current.GetControllerFactory() as IExtendedControllerFactory;
			var controller = factory.CreateController(RequestContext, (Type)RequestContext.RouteData.Values["controller"]);
			try
			{
				controller.Execute(RequestContext);
			}
			finally
			{
				factory.ReleaseController(controller);
			}
		}

	}
	
}
