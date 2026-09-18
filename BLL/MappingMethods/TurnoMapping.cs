using BLL.Mapping;
using Contracts.DTOs.TurnoDTOs;
using Contracts.DTOs.TurnoServicioDTOs;
using DomainLayer.Models;

namespace BLL.MappingMethods
{
    public class TurnoMapping : IMappingService<Turno, TurnoReadDTO, TurnoCreateUpdateDTO>
    {
        public TurnoReadDTO ToReadDTO(Turno t)
        {
            return new TurnoReadDTO
            {
                TurnoId = t.TurnoId,
                Detalle = t.Detalle!,
                FechaTurno = t.FechaTurno,
                HoraTurno = t.HoraTurno,
                ClienteId = t.ClienteId,
                EstadoTurnoId = t.EstadoTurnoId,
                Servicios = t.TurnoServicios.Select(ts => new TurnoServicioReadDTO
                {
                    ServicioId = ts.ServicioId,
                    MontoAplicado = ts.MontoAplicado,
                    TiempoAplicado = ts.TiempoAplicado
                })


            };
        }
        public Turno ToUpdateEntity(int id,TurnoCreateUpdateDTO dto)
        {
            return new Turno
            {
                TurnoId = id,
                Detalle = dto.Detalle,
                ClienteId = dto.ClienteId,
                EstadoTurnoId = dto.EstadoTurnoId,
                HoraTurno = dto.HoraTurno,
                FechaTurno = dto.FechaTurno
            };
        }
        public Turno ToEntity(TurnoCreateUpdateDTO t)
        {
            return new Turno
            {
                Detalle = t.Detalle,
                ClienteId = t.ClienteId,
                EstadoTurnoId = t.EstadoTurnoId,
                HoraTurno = t.HoraTurno,
                FechaTurno = t.FechaTurno
            };
        }

    }
}
