//using DAL.Data;
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
        //private readonly DbSet<T> _dbSet;
        //private readonly TurnoServicioPeluqueriaContext _dbContext;
        //public GenericRepository(TurnoServicioPeluqueriaContext dbContext)
        //{
        //    _dbSet = dbContext.Set<T>();
        //    _dbContext = dbContext;
        //}

        //public async Task Add(T TEntity)
        //{
        //    await _dbSet.AddAsync(TEntity);
        //    _dbContext.SaveChanges();
        //}

        //public async Task<IEnumerable<T>> GetAll()
        //{
        //    var entities = await _dbSet.ToListAsync();
        //    return entities;
        //}

        //public async Task<T> GetById(int id)
        //{
        //    var entity = await _dbSet.FindAsync(id);
        //    return entity!;
        //}

        //public void Remove(T TEntity)
        //{
        //    _dbSet.Remove(TEntity);
        //}

        ////correguir las advertencias.
        //public void Update(T TEntity)
        //{
        //    _dbSet.Update(TEntity);
        //    _dbContext.SaveChanges();
        //}
    }
}
