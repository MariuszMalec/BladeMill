using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace BladeMill.Web.Controllers
{
    public class ProgramExeController : Controller
    {
        // GET: HelpProgramsController
        private ProgramExeService _programExeService;
        private readonly ILogger<ProgramExeController> _logger;
        public ProgramExeController(ILogger<ProgramExeController> logger)
        {
            _programExeService = new ProgramExeService();
            _logger = logger;
        }
        public ActionResult Index()
        {
            var model = _programExeService.GetAll();
            return View(model);
        }
        public ActionResult StartProgramExe(int id)
        {
            var fullName = _programExeService.GetProgramExeById(id).FullName;
            if (System.IO.File.Exists(fullName))
            {
                _programExeService.StartNewProcess(fullName);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //return Content($"Programu nie znaleziono ! {program} || {dir}");
                _logger.LogWarning("Get({Id}) cannot find program", id);
                return RedirectToAction("EmptyList", new { program = fullName });
            }

            return View();
        }
        public ActionResult EmptyList(string program)//TODO to musi byc!!
        {
            ViewBag.Program = program;
            return View();
        }

        // GET: HelpProgramsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HelpProgramsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HelpProgramsController/Create
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

        // GET: HelpProgramsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HelpProgramsController/Edit/5
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

        // GET: HelpProgramsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HelpProgramsController/Delete/5
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
