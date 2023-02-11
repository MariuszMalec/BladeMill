using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Models;
using BladeMill.BLL.Services;
using System;
using System.Linq;

namespace MainApp.BLL.Context
{
    public class SeedData
    {
        public static async void SeedUsers(ApplicationDbContext context)
        {
            if (context.Uzytkownicy.Any())
            {
                return;
            }
            var users = new UserServiceWithoutDatabase();
            foreach (var item in users.GetAll())
            {
                await context.AddAsync(new User { FirstName = item.FirstName, LastName = item.LastName, Sso = item.Sso, Created = DateTime.Now });
            }
            await context.SaveChangesAsync();
        }
    }
}
