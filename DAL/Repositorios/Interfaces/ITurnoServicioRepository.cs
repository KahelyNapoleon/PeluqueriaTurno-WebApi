using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios.Interfaces
{
    public interface ITurnoServicioRepository : IGenericRepository<TurnoServicio>
    {
        Task<IEnumerable<TurnoServicio>> GetAllTurnosServicios();
        Task<IEnumerable<TurnoServicio>> GetAllTurnoServicioByTurnoId(int turnoId);
        Task<List<int>> GetAllServiciosWithTurnoId(int id);
    }
}
