using System;
using System.Collections.Generic;
using CustomFramework.Controllers;

namespace CustomFramework
{
    public class Route
    {
        public Route(string name, RequestMethod method, string path, Type controller)
        {
            Name = name;
            Method = method;
            Path = path;
            Controller = controller;
        }

        public Route()
        {
        }

        public string Name { get; }

        public RequestMethod Method { get; }

        public string Path { get; }

        public bool Parameterised { get; set; }

        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        public Type Controller { get; set; }

        public enum RequestMethod
        {
            GET,
            POST,
            PUT,
            DELETE,
            OPTIONS
        }
    }
}