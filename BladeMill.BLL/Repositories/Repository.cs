using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.BLL.Repositories
{
    public class Repository<T> : IBaseRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _entities.AsQueryable();
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> FindById(int id)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public Task<bool> isExists(int id)
        {
            return _entities.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Create(T entity)
        {
            await _entities.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Update(T entity)
        {
            _entities.Update(entity);
            return await Save();
        }

        public async Task<bool> Delete(T entity)
        {
            var entityToDelete = _entities.FirstOrDefault(x => x.Id == entity.Id);
            _entities.Remove(entityToDelete);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}
