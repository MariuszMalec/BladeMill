using BladeMill.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.BLL.Repositories
{
    public interface IBaseRepository<T> where T : IEntity
    {
        IQueryable<T> GetAllQueryable();
        Task<IEnumerable<T>> FindAll();
        Task<T> FindById(int id);
        Task<bool> isExists(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}
