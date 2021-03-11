using System;
using Newtonsoft.Json.Linq;

namespace CustomFramework.Services
{
    public static class RouteBuilder
    {
        public static Route BuildRouteFromConfig(JToken routeConfig)
        {
            string name = routeConfig.Value<string>("name");
            string path = routeConfig.Value<string>("path");
            string stringMethod = routeConfig.Value<string>("method");
            string controller = routeConfig.Value<string>("controller");
            Route.RequestMethod method = Route.RequestMethod.GET;

            if (stringMethod == null)
            {
                Console.WriteLine("Request method not specified!");
                Environment.Exit(1);
            }
            
            switch (stringMethod.ToUpper())
            {
                case "GET":
                    method = Route.RequestMethod.GET;
                    break;
                case "POST":
                    method = Route.RequestMethod.POST;
                    break;
                case "PUT":
                    method = Route.RequestMethod.PUT;
                    break;
                case "DELETE":
                    method = Route.RequestMethod.DELETE;
                    break;
                case "OPTIONS":
                    method = Route.RequestMethod.OPTIONS;
                    break;
                default:
                    Console.WriteLine("Unrecognised request method!");
                    Environment.Exit(1);
                    break;
            }

            Type controllerType = Type.GetType($"CustomFramework.Controllers.{controller}");
            return new Route(name, method, path, controllerType);
        }
    }
}