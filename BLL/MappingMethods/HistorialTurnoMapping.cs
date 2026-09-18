using BLL.Mapping;
using Contracts.DTOs.HistorialTurnoDTOs;
using DomainLayer.Models;


namespace BLL.MappingMethods
{
    public class HistorialTurnoMapping : IMappingService<HistorialTurno, HistorialTurnoReadDTO, HistorialTurnoCreateUpdateDTO>
    {

        public HistorialTurnoReadDTO ToReadDTO(HistorialTurno h)
        {
            return new HistorialTurnoReadDTO
            {
                HistorialTurnoId = h.HistorialTurnoId,
                FechaHoraActual = h.FechaHoraActual,
                FechaHoraAnterior = h.FechaHoraAnterior,
                EstadoTurnoActual = h.EstadoTurnoActual,
                EstadoTurnoAnterior = h.EstadoTurnoAnterior
            };
        }

        public HistorialTurno ToUpdateEntity(int id,HistorialTurnoCreateUpdateDTO dto)
        {
            return new HistorialTurno
            {
                HistorialTurnoId = id,
                TurnoId = dto.TurnoId,
                FechaHoraActual =   dto.FechaHoraActual,
                FechaHoraAnterior = dto.FechaHoraAnterior,
                EstadoTurnoActual = dto.EstadoTurnoActual,
                EstadoTurnoAnterior = dto.EstadoTurnoAnterior
            };
        }

        public HistorialTurno ToEntity(HistorialTurnoCreateUpdateDTO h)
        {
            return new HistorialTurno
            {
                TurnoId = h.TurnoId,
                FechaHoraActual = h.FechaHoraActual,
                FechaHoraAnterior = h.FechaHoraAnterior,
                EstadoTurnoActual = h.EstadoTurnoActual,
                EstadoTurnoAnterior = h.EstadoTurnoAnterior
            };
        }

    }
}
