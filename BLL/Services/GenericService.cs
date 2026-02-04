using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Result;
using BLL.Services.Interfaces;
using DAL.Repositorios;
using DAL.Repositorios.Interfaces;

namespace BLL.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private IGenericRepository<T> _repository;
        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<Result<T>> Add(T TEntity)
        {
            throw new ArgumentNullException();
            //Validacion de la entidad
            //Si es correcto agregar
            //Si no es correcto lanzar error

        }

        public async Task<Result<string>> Delete(int id)
        {
            var TEntity = await _repository.GetById(id);
            if (TEntity == null)
            {
                return Result<string>.Fail($"El id {id} no se encuentra en los registros");
            }
            await _repository.Remove(TEntity);

            return Result<string>.Succes("Registro eliminado");
        }

        public async Task<Result<IEnumerable<T>>> GetAll()
        {
            var TEntities = await _repository.GetAll();
            if (!TEntities.Any())
            {
                return Result<IEnumerable<T>>.Fail("Aun no hay registros.");
            }

            return Result<IEnumerable<T>>.Succes(TEntities);
        }

        public async Task<Result<T>> GetById(int id)
        {
            var TEntity = await _repository.GetById(id);
            if (TEntity == null)
            {
                return Result<T>.Fail($"Registro con id {id} no se encuentra.");
            }

            return Result<T>.Succes(TEntity);
        }

        public Task<Result<T>> Update(int id, T TEntity)
        {
            throw new NotImplementedException();
        }
    }
}
