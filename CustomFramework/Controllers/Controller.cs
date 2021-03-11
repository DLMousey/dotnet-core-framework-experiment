using System;

namespace CustomFramework.Controllers
{
    public abstract class Controller
    {
        public void Dispatch(Route route)
        {
            switch (route.Method)
            {
                case Route.RequestMethod.GET:
                    GetList();
                    break;
                default:
                    Console.WriteLine("Unsupported method");
                    Environment.Exit(1);
                    break;
            }
        }

        protected abstract void GetList();
        protected abstract void Get();
        protected abstract void Create();
        protected abstract void Update();
        protected abstract void Delete();
        protected abstract void Options();
    }
}