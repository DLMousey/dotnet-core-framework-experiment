using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CustomFramework.Controllers
{
    public class SystemController : Controller
    {
        public async void NotFound(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.StatusCode = (int) HttpStatusCode.NotFound;

            JObject data = new JObject();
            data["status"] = 404;
            data["message"] = "Unknown route requested";

            byte[] output = Encoding.UTF8.GetBytes(data.ToString());
            await response.OutputStream.WriteAsync(output, 0, output.Length);
                    
            Console.WriteLine(
                $"[404]: {request.Url.AbsolutePath} | " +
                $"{request.HttpMethod} | " +
                $"{request.UserHostName} | " +
                $"{request.UserAgent}"
            );
        }

        protected override void GetList()
        {
            throw new NotImplementedException();
        }

        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Create()
        {
            throw new NotImplementedException();
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override void Options()
        {
            throw new NotImplementedException();
        }
    }
}