using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Dane pobierane z serwisu
    /// </summary>
    public class UserServiceWithoutDatabase
    {
        private static List<User> Users = new List<User>()
        {
            new User(1,"Mariusz", "Malec", 212517683),
            new User(2,"Mariusz", "Orzechowski", 212537861),
            new User(3,"Patryk", "Wisniewski", 212556351),
            new User(4,"Michal", "Popakul", 212538300),
            new User(5,"Jakub", "Obrebski", 212736611),
            new User(6,"Zbigniew", "Papierowski", 212540098),
            new User(7,"Piotr", "Grajczak", 212510479),
            new User(8,"Radek", "Kuszyk", 212564181),
            new User(9,"Michal", "Staszynski", 212791400),
            new User(10,"Marcin", "Mielewczyk", 212583581)
        };
        public List<User> GetAll()
        {
            return Users;
        }
        public User GetById(int id)
        {
            //Users = GetAll();
            return Users.SingleOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Get Sso for current user 
        /// </summary>
        /// <returns></returns>
        public int GetUserSso()
        {
            var getUserProfile = int.TryParse(Path.GetFileName(GetEnvironmentVariable("USERPROFILE")), out int sSO);
            if (getUserProfile == false)
            {
                return 0;
            }
            return sSO;
        }

        /// <summary>
        /// Get Sso according user last name 
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public int GetUserSso(string lastName)
        {
            var getSso = Users.Where(u=>u.LastName == lastName)
                              .Select(u=>u.Sso)
                              .FirstOrDefault();
            if (getSso == default)
            {
                return 0;
            }
            return getSso;
        }

        public string GetUserFirstLastName()//static(return) //void (no return)
        {
            Users = GetAll();
            var getUserProfile = int.TryParse(Path.GetFileName(GetEnvironmentVariable("USERPROFILE")), out int sSO);
            var user = Users.Where(u => u.Sso == sSO).FirstOrDefault();
            if (user == null)
                return new User(7, "Gal", "Anonim", 123456).LastName;
            return $"{user.FirstName} {user.LastName}";
        }
        private string GetEnvironmentVariable(string Variable)
        {
            Environment.CurrentDirectory = Environment.GetEnvironmentVariable(Variable);
            DirectoryInfo info = new DirectoryInfo(".");
            string envvariable = info.FullName;
            if (envvariable == "")
            {
                envvariable = "";
            }
            return envvariable;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            Users.Remove(user);
        }

        public void Create(User model)
        {
            model.Id = GetNextId();
            Users.Add(model);
        }
        public int GetNextId()
        {
            if (!Users.Any())
                return 0;
            return (Users?.Max(m => m.Id) ?? 0) + 1;
        }

    }
}
