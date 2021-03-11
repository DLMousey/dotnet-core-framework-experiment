using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CustomFramework.Controllers;
using CustomFramework.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomFramework
{
    public class Startup
    {
        public HttpListener listener;

        private Dictionary<
            Route.RequestMethod, 
            List<Route>
        > routeMap = new Dictionary<Route.RequestMethod, List<Route>>();
        
        public Startup()
        {
            Console.WriteLine("Starting application...");
            Console.WriteLine();
            Console.WriteLine("Building routes...");
            
            BuildRoutes();
            BuildServer();
        }
        
        private void BuildRoutes()
        {
            Console.WriteLine("Preparing routeMap...");
            routeMap.Add(Route.RequestMethod.GET, new List<Route>());
            routeMap.Add(Route.RequestMethod.POST, new List<Route>());
            routeMap.Add(Route.RequestMethod.PUT, new List<Route>());
            routeMap.Add(Route.RequestMethod.DELETE, new List<Route>());
            routeMap.Add(Route.RequestMethod.OPTIONS, new List<Route>());

            Console.WriteLine("Loading config...");
            using (StreamReader config = File.OpenText(@"appconfig.json"))
            using (JsonTextReader reader = new JsonTextReader(config))
            {
                JObject appConfiguration = (JObject) JToken.ReadFrom(reader);
                appConfiguration.TryGetValue("routes", out var routeConfiguration);

                if (routeConfiguration == null)
                {
                    Console.WriteLine("No routes defined in config!");
                    Environment.Exit(1);
                }

                int currentRoute = 1;
                JEnumerable<JToken> fileRoutes = routeConfiguration.Children();
                Console.WriteLine($"Found {fileRoutes.Count()} routes in config");

                foreach (var token in routeConfiguration.Children())
                {
                    Console.WriteLine($"Building route {currentRoute} of {fileRoutes.Count()}...");
                    Route r = RouteBuilder.BuildRouteFromConfig(token);

                    List<Route> targetList;
                    routeMap.TryGetValue(r.Method, out targetList);

                    if (targetList == null)
                    {
                        Console.WriteLine("Unexpected method found!");
                        Environment.Exit(1);
                    }
                    
                    targetList.Add(r);
                    currentRoute++;
                }
                
                Console.WriteLine("Route building complete");
                Console.WriteLine();
            }
        }

        private void BuildServer()
        {
            // Based off define-private-public's C# HTTP Server implementation
            // @see https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7
            
            Console.WriteLine("Starting HTTP Listener...");
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.Start();

            Console.WriteLine("Listening for traffic on localhost:8000");
            Console.WriteLine();

            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }

        private async Task HandleIncomingConnections()
        {
            while (true)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse res = ctx.Response;

                Console.WriteLine(
                    $"INCOMING REQUEST: {req.Url.AbsolutePath} | " +
                    $"{req.HttpMethod} | " +
                    $"{req.UserHostName} | " +
                    $"{req.UserAgent}"
                );

                List<Route> routeCollection = new List<Route>();
                switch (req.HttpMethod.ToUpper())
                {
                    case "GET":
                        routeMap.TryGetValue(Route.RequestMethod.GET, out routeCollection);
                        break;
                    case "POST":
                        routeMap.TryGetValue(Route.RequestMethod.POST, out routeCollection);
                        break;
                    case "PUT":
                        routeMap.TryGetValue(Route.RequestMethod.PUT, out routeCollection);
                        break;
                    case "DELETE":
                        routeMap.TryGetValue(Route.RequestMethod.DELETE, out routeCollection);
                        break;
                    case "OPTIONS":
                        routeMap.TryGetValue(Route.RequestMethod.OPTIONS, out routeCollection);
                        break;
                    default:
                        Console.WriteLine("Unrecognised request method!");
                        Environment.Exit(1);
                        break;
                }

                Route dispatchRoute = routeCollection.SingleOrDefault(r => r.Path.Equals(req.Url.AbsolutePath));
                if (dispatchRoute == null)
                {
                    Console.WriteLine("Unknown route requested!");
                    Environment.Exit(1);
                }

                Controller controller = (Controller) Activator.CreateInstance(dispatchRoute.Controller);
                controller.Dispatch(dispatchRoute);

                res.Close();
            }
        }
        
    }
}