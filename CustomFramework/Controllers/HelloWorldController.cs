using System;

namespace CustomFramework.Controllers
{
    public class HelloWorldController : Controller
    {
        protected override void GetList()
        {
            Console.WriteLine($"GetList Action - {GetType()}");
        }

        protected override void Get()
        {
            throw new System.NotImplementedException();
        }

        protected override void Create()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }

        protected override void Delete()
        {
            throw new System.NotImplementedException();
        }

        protected override void Options()
        {
            throw new System.NotImplementedException();
        }
    }
}