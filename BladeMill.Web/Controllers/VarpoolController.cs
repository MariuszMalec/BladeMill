using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.Web.Controllers
{
    public class VarpoolController : Controller
    {
        private XMLVarpoolService _xmlVarpoolService;
        public VarpoolController()
        {
            _xmlVarpoolService = new XMLVarpoolService();
        }
        // GET: VarpoolController
        public ActionResult Index()
        {
            var model = _xmlVarpoolService.GetAllDataFromVarpoolFile();
            return View(model);
        }

        // GET: VarpoolController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VarpoolController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VarpoolController/Create
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

        // GET: VarpoolController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VarpoolController/Edit/5
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

        // GET: VarpoolController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VarpoolController/Delete/5
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
