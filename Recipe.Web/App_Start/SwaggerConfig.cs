using System.Web.Http;
using WebActivatorEx;
using Recipe.Web;
using Swashbuckle.Application;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Recipe.Web
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Recipe.Web");
                        c.IncludeXmlComments(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App_Data\\RecipeDal.XML");
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}