using BladeMill.BLL.DAL;
using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> FindAll();
        Task Create(UserDto user);
        Task<UserDto> FindById(int id);
        Task<bool> Delete(UserDto user);
        Task<bool> Update(UserDto user);
    }
}
