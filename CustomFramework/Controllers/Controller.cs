using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomFramework.Controllers
{
    public abstract class Controller
    {
        protected Route Route;
        
        public async Task<JsonResponse> Dispatch(Route route)
        {
            Route = route;
            switch (route.Method)
            {
                case Route.RequestMethod.GET:
                    if (route.Parameterised)
                    {
                        return await Get();
                    }
                    else
                    {
                        return await GetList();
                    }
                default:
                    Console.WriteLine("Unsupported method");
                    Environment.Exit(1);
                    break;
            }

            return new JsonResponse();
        }

        public JsonResponse Respond(int status, string message, object data)
        {
            JsonResponse response = new JsonResponse();
            response.Status = status;
            response.Message = message;
            response.Data = data;

            return response;
        }

        protected abstract Task<JsonResponse> GetList();
        protected abstract Task<JsonResponse> Get();
        protected abstract Task<JsonResponse> Create();
        protected abstract Task<JsonResponse> Update();
        protected abstract Task<JsonResponse> Delete();
        protected abstract Task<JsonResponse> Options();
    }
}