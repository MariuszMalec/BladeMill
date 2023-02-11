using BladeMill.BLL.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BladeMill.BLL.Interfaces
{
    public interface IBMUserService
    {
        Task<IEnumerable<BMUserDto>> GetAll();
        Task<BMUserDto> GetById(int id);
    }
}