using DAL.Data;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositorios
{
    public class HistorialTurnoRepository(ApplicationDbContext dbContext) : GenericRepository<HistorialTurno>(dbContext)
    {
    }
}
