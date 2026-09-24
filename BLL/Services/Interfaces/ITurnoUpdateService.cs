using BLL.Result;
using Contracts.DTOs.TurnoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ITurnoUpdateService
    {
        Task<Result<TurnoReadDTO>> UpdateEstadoTurno(int turnoId, int estadoTurnoId);
        Task<Result<TurnoReadDTO>> UpdateFechaYHora(int turnoId, DateOnly fecha, TimeOnly hora);
        Task<Result<TurnoReadDTO>> UpdateServicios(int turnoId, List<int> serviciosIds);
    }
}
