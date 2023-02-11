using BladeMill.BLL.DAL;
using BladeMill.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BladeMill.Web.Controllers
{
    public class BMUserController : Controller
    {
        private readonly IBMUserService _bmUserService;

        public BMUserController(IBMUserService bmUserService)
        {
            _bmUserService = bmUserService;
        }
        // GET: BMUserController
        public async Task<IActionResult> Index()
        {
            var users = await _bmUserService.GetAll();
            return View(users);
        }

        // GET: BMUserController/Details/5
        public async Task<ActionResult<BMUserDto>> Details(int id)
        {
            var user = await _bmUserService.GetById(id);
            return View(user);
        }

        // GET: BMUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BMUserController/Create
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

        // GET: BMUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BMUserController/Edit/5
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

        // GET: BMUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BMUserController/Delete/5
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
