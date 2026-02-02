using DAL.Data;
using DAL.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositorios
{
    public class GenericRepository<T> : IGenericRepository<T> where T:class
    {
        private readonly DbSet<T> _dbSet;
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        public async Task Add(T TEntity)
        {
            _dbContext.Entry(TEntity).State = EntityState.Added;
           // await _dbSet.AddAsync(TEntity);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity!;
        }

        public async Task Remove(T TEntity)
        {
            _dbContext.Entry(TEntity).State = EntityState.Deleted;
            await SaveChangesAsync();
        }

        //correguir las advertencias.
        public async Task Update(T TEntity)
        {
            _dbContext.Entry(TEntity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
