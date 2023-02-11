using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.Web.Controllers
{
    public class InputDataXmlController : Controller
    {
        private InputDataXmlService _inputDataXmlService;
        // GET: InputDataController

        public InputDataXmlController()
        {
            _inputDataXmlService = new InputDataXmlService();
        }

        public ActionResult Index()
        {
            var model = _inputDataXmlService.GetAllDataFromInputDataXml();
            return View(model);
        }

        // GET: InputDataController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InputDataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InputDataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InputDataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InputDataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InputDataController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InputDataController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
