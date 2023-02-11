using BladeMill.BLL.DAL;
using BladeMill.BLL.Services;
using Microsoft.EntityFrameworkCore;

namespace BladeMill.BLL.DatatAcess
{
    public static class ModelBulderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var userService = new UserServiceWithoutDatabase();
            modelBuilder.Entity<UserDto>().HasData(
                userService.GetAll()
                );
        }
    }
}
