using BladeMill.BLL.Services;
using System;

namespace BladeMill.ConsoleApp.BladeMillUsers
{
    public class Users
    {
        public static void Test()
        {
            var userService = new UserServiceWithoutDatabase();
            var users = userService.GetAll();
            users.ForEach(u => Console.WriteLine(u));
        }
    }
}
