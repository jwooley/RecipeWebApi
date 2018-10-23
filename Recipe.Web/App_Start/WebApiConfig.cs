using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Recipe.Web.Services;
using System.Web.Http.Tracing;
using RecipeDal;
using Microsoft.AspNet.OData.Extensions;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json.Serialization;

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

            GlobalConfiguration.Configuration.EnableDependencyInjection();
            config.AddODataQueryFilter();
            config.Count().Filter().OrderBy().Select().MaxTop(null);


            config.Services.Replace(typeof(System.Web.Http.Tracing.ITraceWriter), new ServiceTracer());
            config.Formatters.Add(new SyndicationFeedFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            config.EnsureInitialized();

            var logConfig = new LogConfiguration();
        }
    }
}
