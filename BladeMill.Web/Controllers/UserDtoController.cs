using AutoMapper;
using BladeMill.BLL.DAL;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.Web.Controllers
{
    public class UserDtoController : Controller
    {
        // GET: UserDtoController

        private UserServiceWithoutDatabase _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public UserDtoController(UserServiceWithoutDatabase userService, IMapper mapper, ILogger<HomeController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var dto = _userService.GetAll();//wziete z modelu
            _logger.LogInformation("Sciagam dane z modelu z Service ...");
            //var model = dto.Select(e => _mapper.Map<UserDto>(e)); //to tez dziala
            var model = _mapper.Map<List<UserDto>>(dto);// mapowanie na dto co checemy pokazac tylko!
            _logger.LogInformation("Mapowanie automatyczne z User na UserDto");
            return View(model);
        }

        // GET: UserDtoController/Details/5
        public ActionResult Details(int id)
        {
            var dto = _userService.GetById(id);//wziete z modelu
            var model = _mapper.Map<UserDto>(dto);

            if (model == null)
                return BadRequest($"Brak uzytkownika!");

            return View(model);
        }

        // GET: UserDtoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDtoController/Create
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

        // GET: UserDtoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserDtoController/Edit/5
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

        // GET: UserDtoController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _userService.GetById(id);
            ViewBag.User = model.LastName;
            _logger.LogWarning("Get({Id}) you cannot delete user with Id", id);
            return RedirectToAction("EmptyList", new { User = model.LastName });
            return View();
        }

        // POST: UserDtoController/Delete/5
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
        public ActionResult EmptyList(string user)//TODO to musi byc!!
        {
            ViewBag.User = user;
            return View();
        }
    }
}
