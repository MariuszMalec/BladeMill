using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BladeMill.Web.Controllers
{
    public class NcMainProgramController : Controller
    {
        // GET: MainProgramController
        private NcMainProgramService _ncMainProgramService;
        public string MainProgrames = "";
        public NcMainProgramController()
        {
            _ncMainProgramService = new NcMainProgramService();
        }
        public ActionResult Index()
        {
            var model = _ncMainProgramService.GetAll();
            return View(model);
        }

        // GET: MainProgramController/Details/5
        public ActionResult Details(int id)//TODO ID dziala ale ncProgram juz nie!!!
        {
            var model = _ncMainProgramService.GetById(id);

            if (model == null)
                return BadRequest("Brak przekazania modela!");

            return View(model);
        }


        // GET: MainProgramController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MainProgramController/Create
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

        // GET: MainProgramController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MainProgramController/Edit/5
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

        // GET: MainProgramController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MainProgramController/Delete/5
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
