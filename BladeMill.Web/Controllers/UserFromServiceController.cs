using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BladeMill.Web.Controllers
{
    public class UserFromServiceController : Controller
    {
        // GET: UserFromServiceController
        private readonly ILogger<UserFromServiceController> _logger;
        private UserServiceWithoutDatabase _userServiceWithoutBase;
        private string _sso = "";

        public UserFromServiceController(ILogger<UserFromServiceController> logger,
               UserServiceWithoutDatabase userServiceWithoutBase)
        {
            _logger = logger;
            _userServiceWithoutBase = userServiceWithoutBase;
        }
        public ActionResult Index()
        {
            var model = new List<User>() { };
            _logger.LogInformation("Sciagam dane z modelu z Service ...");
            model = _userServiceWithoutBase.GetAll();
            return View(model);//tutaj tylko user model wchodzi!!
        }

        // GET: UserFromServiceController/Details/5
        public ActionResult Details(int id)
        {
            var model = _userServiceWithoutBase.GetById(id);

            if (model == null)
                return BadRequest($"Brak uzytkownika!");

            return View(model);
        }

        // GET: UserFromServiceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserFromServiceController/Create
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

        // GET: UserFromServiceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserFromServiceController/Edit/5
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

        // GET: UserFromServiceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserFromServiceController/Delete/5
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
