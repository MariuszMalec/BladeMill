using BladeMill.BLL.DAL;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using BladeMill.BLL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Dane z bazy danych msql 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IBaseRepository<UserDto> Users;

        public UserService(IBaseRepository<UserDto> users)
        {
            Users = users;
        }
        public async Task<IEnumerable<UserDto>> FindAll()
        {
            return await Users.FindAll();
        }

        public async Task Create(UserDto user)
        {
            await Users.Create(user);
        }

        public async Task<UserDto> FindById(int id)
        {
            return await Users.FindById(id);
        }

        public async Task<bool> Delete(UserDto user)
        {
            return await Users.Delete(user);
        }
        public async Task<bool> Update(UserDto user)
        {
            return await Users.Update(user);
        }
    }
}
