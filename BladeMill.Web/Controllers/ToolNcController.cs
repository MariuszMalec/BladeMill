using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BladeMill.Web.Controllers
{
    public class ToolNcController : Controller
    {
        // GET: ToolNcController
        private ToolNcService _toolNcService;
        private NcMainProgramService _ncMainProgramService;

        public ToolNcController()
        {
            _toolNcService = new ToolNcService();
            _ncMainProgramService = new NcMainProgramService();
        }
        public ActionResult Index()
        {
            var model = _toolNcService.GetAllToolsFromCurrentXml();
            return View(model);
        }

        // GET: ToolNcController/Details/5
        public ActionResult Details(int id)
        {
            var model = _toolNcService.GetById(id);
            return View(model);
        }

        // GET: ToolNcController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToolNcController/Create
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

        // GET: ToolNcController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _toolNcService.GetById(id);
            return View(model);
        }

        // POST: ToolNcController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tool model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //var id = TempData["id"] as string;
                //var user = _userService.GetById(id);
                //_transactionService.AddTransactionByUser(model, user);

                //_toolNcService.Update(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ToolNcController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _toolNcService.GetById(id);
            return View(model);
        }

        // POST: ToolNcController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tool model)
        {
            try
            {
                _toolNcService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
