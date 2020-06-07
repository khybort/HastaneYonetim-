using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HastaneYonetim
{
    public static class WebApiConfig
    {
        public static void Kayit(HttpConfiguration config)
        {
            // formate with camelcase
            var ayar = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            ayar.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ayar.Formatting = Formatting.Indented;

            //parent child serialization
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);




            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
