using System;
using System.Threading.Tasks;

namespace CustomFramework.Controllers
{
    public class HelloWorldController : Controller
    {
        protected override async Task<JsonResponse> GetList()
        {
            Console.WriteLine($"GetList Action - {GetType()}");
            return Respond(200, "Get List Action Successful!", new {success = true});
        }

        protected override async Task<JsonResponse> Get()
        {
            Console.WriteLine($"Get Action - {GetType()}");

            string outputName;
            Route.Parameters.TryGetValue("name", out outputName);
            return Respond(200, "Get Action Successful!", new {success = true, name = outputName});
        }

        protected override Task<JsonResponse> Create()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<JsonResponse> Update()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<JsonResponse> Delete()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<JsonResponse> Options()
        {
            throw new System.NotImplementedException();
        }
    }
}