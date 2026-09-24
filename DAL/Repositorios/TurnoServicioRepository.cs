using DAL.Data;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    public class TurnoServicioRepository : GenericRepository<TurnoServicio>, ITurnoServicioRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TurnoServicioRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TurnoServicio>> GetAllTurnosServicios()
        {
            var servicios = _dbContext.TurnoServicios
                .Include(ts => ts.Servicio);
            
            return await servicios.ToListAsync();
        
        }

        public async Task<IEnumerable<TurnoServicio>> GetAllTurnoServicioByTurnoId(int turnoId)
        {
            var turnoServicios = _dbContext.TurnoServicios
                .Where(ts => ts.TurnoId == turnoId)
                .ToListAsync();

            return await turnoServicios;
        }

        public async Task<List<int>> GetAllServiciosWithTurnoId(int id)
        {
            var servicios =  _dbContext.TurnoServicios
                .Where(ts => ts.TurnoId == id)
                .Select(ts => ts.ServicioId);
                

            return await servicios.ToListAsync();
                
                
        }

    }
}
