using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace BladeMill.Web.Controllers
{
    public class ApplicationXmlConfController : Controller
    {
        private AppXmlConfService _appXmlConfService;
        private static string _applicationXmlFile = Path.Combine(@"C:\Users",
                                 Environment.UserName,
                                 @"source\repos\BladeMill\UnitTests\SourceData", "Application.xml.conf");

        public ApplicationXmlConfController(AppXmlConfService appXmlConfService)
        {
            _appXmlConfService = appXmlConfService;
        }

        public IActionResult Index()
        {
            var model = new AppXmlConfFiles(_applicationXmlFile);

            if (model == null)
                return Content("AppconfFile model pusty!");

            return View(model);
        }
    }
}
