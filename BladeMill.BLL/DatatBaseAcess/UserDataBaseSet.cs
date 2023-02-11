using BladeMill.BLL.DAL;
using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BladeMill.BLL.DatatAcess
{
    public class UserDataBaseSet
    {
        private readonly ApplicationDbContext _db;
        // GET: UserController
        public UserDataBaseSet(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddUsersDb()
        {
            //_db.AddAsync(new UserDto { FirstName = "Mikel", LastName = "Jordan" ,Sso=212212212});
            //_db.AddAsync(new UserDto { FirstName = "John", LastName = "Lenon" , Sso=313132123});
            var users = new UserServiceWithoutDatabase();
            foreach (var item in users.GetAll())
            {
                _db.AddAsync(new UserDto { FirstName = item.FirstName, LastName = item.LastName, Sso = item.Sso, Created = DateTime.Now });
            }
            _db.SaveChanges();
        }
        public List<User> GetAllDto()
        {
            var dtos = _db.Uzytkownicy.ToList();//wczytanie z bazy danych
            var model = dtos.Select(MapUserDtoToUzytkownik);//mapowanie na model
            return model.ToList();
        }

        public void SearchDto(string searchString)
        {
            var users = from u in _db.Uzytkownicy
                        select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName!.Contains(searchString));
            }

        }

        public ApplicationDbContext GetAllDb()
        {
            return _db;
        }
        public void AddUserDto(User model)
        {
            _db.AddAsync(new UserDto { Id = model.Id, FirstName = model.FirstName, LastName = model.LastName, Sso = model.Sso, Created = DateTime.Now });
            _db.SaveChanges();
            //wez idy z bazy
            var newId = _db.Uzytkownicy.OrderBy(id => id.Id).Last();
            model.Id = newId.Id;
        }
        private User MapUserDtoToUzytkownik(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Sso = userDto.Sso,
                Created = userDto.Created
            };
        }
        public void DeleteUserDto(User model, int id)
        {
            var person = _db.Uzytkownicy.Find(id);
            _db.Uzytkownicy.Remove(person);
            _db.SaveChanges();
        }

        public User GetByIdDto(int id)
        {
            return (User)_db.Uzytkownicy.Select(MapUserDtoToUzytkownik).Where(u => u.Id == id).FirstOrDefault();// z bazy pokazuje!
        }

        public void UpdateUserDto(User model, int id)
        {
            var person = _db.Uzytkownicy.Find(id);
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.Sso = model.Sso;
            _db.SaveChanges();
        }
    }
}
