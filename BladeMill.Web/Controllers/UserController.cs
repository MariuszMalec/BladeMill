using AutoMapper;
using BladeMill.BLL.DAL;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeMill.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private string _sso = "";
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        private User MapUserDtoToUzytkownik(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Created = userDto.Created,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Sso = userDto.Sso             
            };
        }

        public async Task<ActionResult> Index()
        {
            var model = new List<User>() { };
            _logger.LogInformation("Sciagam dane z bazy danych...");
            var dataFromBase = await _userService.FindAll();
            model = _mapper.Map<List<User>>(dataFromBase);

            //model = dataFromBase.Select(MapUserDtoToUzytkownik);//reczne mapowanie na model

            if (!dataFromBase.Any())
                return BadRequest($"Brak uzytkowników!");

            return View(model);//tutaj tylko user model wchodzi!!
        }

        // GET: UserControlle/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var dataFromBase = await _userService.FindById(id);
            var model = _mapper.Map<User>(dataFromBase);

            if (model == null)
                return BadRequest($"Brak uzytkownika!");

            _logger.LogInformation($"Uzytkownik {model.LastName} uruchomil szczegoly o godz {DateTime.Now}");
            return View(model);
        }

        public ActionResult EmptyList(string user)//TODO to musi byc!!
        {
            ViewBag.User = user;
            return View();
        }

        // GET: UserControlle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserControlle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _userService.Create(model);

                //walidacja SSO
                //var maxSsoLenght = 9;
                //var validate = new UserValidator();
                //var message = validate.ValidateSSO(model.Sso, maxSsoLenght, _db);
                //if (!string.IsNullOrEmpty(message))
                //    return Content($"{message}");
                //var userDto = new UserDataBaseSet(_db);
                //userDto.AddUserDto(model);
                //patrz EntityFrameworkCore.03_ModelMapping / Configuration jak nadac properties/walidacje inna niz tutaj?
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserControlle/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var dataFromBase = await _userService.FindById(id);
            var model = _mapper.Map<User>(dataFromBase);
            if (model == null)
                return BadRequest($"Brak uzytkownika!");
            _logger.LogInformation($"Uzytkownik {model.LastName} uruchomil edycje o godz {DateTime.Now}");
            return View(model);
        }

        // POST: UserControlle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _userService.Update(model);

                //var maxSsoLenght = 9;
                //var validate = new UserValidator();
                //var message = validate.ValidateSSO(model.Sso, maxSsoLenght, _db);
                //if (!string.IsNullOrEmpty(message))
                //    return Content($"{message}");
                //var userDto = new UserDataBaseSet(_db);
                //userDto.UpdateUserDto(model, id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //GET: UserControlle/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var dataFromBase = await _userService.FindById(id);
            var model = _mapper.Map<User>(dataFromBase);
            if (model == null)
                return BadRequest($"Brak uzytkownika!");
            return View(model);
        }

        // POST: UserControlle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, UserDto model)
        {
            try
            {
                await _userService.Delete(model);
                _logger.LogInformation($"Uzytkownik {model.LastName} zostal usuniety o godz {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
