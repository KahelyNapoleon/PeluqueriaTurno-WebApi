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

        /// <summary>
        /// Actualizar un registro de modelo de entidad
        /// </summary>
        /// <param name="id">Busca el registro de entidad de modelo</param>
        /// <param name="TEntity">De tipo DTO con las propiedades nuevas para actualizar
        /// el registro de entidad</param>
        /// <returns>No retorna nada, realiza tal cambio en la base de datos</returns>
        public async Task Update(int id, T TEntity)
        {
            var entity = await _dbSet.FindAsync(id);
            //Y que sucede si TEntity no es del mism otipo que entity?
            //EFCore resuelve esto, actualiza las propiedades, del modelo de entidad, de nombre
            //que coinciden con las del objeto de tipo DTO
            _dbContext.Entry(entity!).CurrentValues.SetValues(TEntity);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
