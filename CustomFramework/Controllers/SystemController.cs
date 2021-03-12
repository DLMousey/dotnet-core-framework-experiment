using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async void JsonResponse(JsonResponse responseData, HttpListenerRequest request, HttpListenerResponse response)
        {
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.StatusCode = responseData.Status;

            string output = JsonConvert.SerializeObject(responseData);
            byte[] outputData = Encoding.UTF8.GetBytes(output);

            await response.OutputStream.WriteAsync(outputData, 0, outputData.Length);
            
            Console.WriteLine(
                $"[{response.StatusCode}]: {request.Url.AbsolutePath} | " +
                $"{request.HttpMethod} | " +
                $"{request.UserHostName} | " +
                $"{request.UserAgent}"
            );
        }

        protected override Task<JsonResponse> GetList()
        {
            throw new NotImplementedException();
        }

        protected override Task<JsonResponse> Get()
        {
            throw new NotImplementedException();
        }

        protected override Task<JsonResponse> Create()
        {
            throw new NotImplementedException();
        }

        protected override Task<JsonResponse> Update()
        {
            throw new NotImplementedException();
        }

        protected override Task<JsonResponse> Delete()
        {
            throw new NotImplementedException();
        }

        protected override Task<JsonResponse> Options()
        {
            throw new NotImplementedException();
        }
    }
}