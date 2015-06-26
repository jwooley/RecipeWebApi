using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
//using Newtonsoft.Json.Serialization;
using Recipe.Web.Services;
using System.Web.Http.Tracing;
using RecipeDal;
using System.Web.OData.Extensions;

namespace Recipe.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API routes
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "services/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.AddODataQueryFilter();
            
            config.Services.Replace(typeof(ITraceWriter), new ServiceTracer());
            config.Formatters.Add(new SyndicationFeedFormatter());

            var logConfig = new LogConfiguration();
        }
    }
}
