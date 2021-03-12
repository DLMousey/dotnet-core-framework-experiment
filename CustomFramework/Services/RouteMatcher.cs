using System;
using System.Collections.Generic;

namespace CustomFramework.Services
{
    public static class RouteMatcher
    {
        public static Route MatchRoute(string path, List<Route> routeMap)
        {
            Route match = null;
            
            string[] pathParts = path.Split('/');
            for (int i = 0; i < routeMap.Count; i++)
            {
                // If this route is a literal match for the path, return it and immediately break out of the loop 
                if (routeMap[i].Path == path)
                {
                    match = routeMap[i];
                    match.Parameterised = false;
                    break;
                }

                // If there's no parameters defined for the route path, skip it
                if (!routeMap[i].Path.Contains('{') && !routeMap[i].Path.Contains('}'))
                {
                    continue;
                }
                
                // If the route path doesn't have the same number of parts as the request path, discard it
                string[] routeParts = routeMap[i].Path.Split('/');
                if (routeParts.Length != pathParts.Length)
                {
                    continue;
                }

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                for (int j = 0; j < routeParts.Length; j++)
                {
                    if (routeParts[j].StartsWith('{') && routeParts[j].EndsWith('}'))
                    {
                        string sanitisedName = routeParts[j].Replace("{", "").Replace("}", "");
                        parameters.Add(sanitisedName, pathParts[j]);
                    }
                    
                    // Substitute each part of the route part with the path part
                    routeParts[j] = pathParts[j];
                }
                
                if (string.Join("/", routeParts) == path)
                {
                    match = routeMap[i];
                    match.Parameters = parameters;
                    match.Parameterised = true;
                    break;
                }
            }

            if (match == null)
            {
                throw new Exception($"No matching route found for path {path}");                
            }

            return match;
        }
    }
}