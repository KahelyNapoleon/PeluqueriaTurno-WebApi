
using BLL.Mapping;
using Contracts.DTOs.TurnoDTOs;
using Contracts.DTOs.TurnoServicioDTOs;
using DomainLayer.Models;

namespace BLL.MappingMethods
{
    public  class TurnoServicioMapping : IMappingService<TurnoServicio, TurnoServicioReadDTO, TurnoServicioCreateUpdateDTO>
    {
        public  TurnoServicioReadDTO ToReadDTO( TurnoServicio t)
        { 
            return new TurnoServicioReadDTO
            {
                TurnoServicioId = t.TurnoServicioId,
                TurnoId     = t.TurnoId,
                ServicioId = t.ServicioId,
                MontoAplicado = t.MontoAplicado,
                TiempoAplicado = t.TiempoAplicado,
            };
        }

        public TurnoServicio ToUpdateEntity(int id, TurnoServicioCreateUpdateDTO t)
        {
            return new TurnoServicio
            {
                TurnoServicioId = id,
                TurnoId = t.TurnoId,
                ServicioId = t.ServicioId,
                MontoAplicado = t.MontoAplicado,
                TiempoAplicado = t.TiempoAplicado
            };
        }

        public  TurnoServicio ToEntity( TurnoServicioCreateUpdateDTO t)
        {
            return new TurnoServicio
            {
                TurnoId = t.TurnoId,
                ServicioId = t.ServicioId,
                MontoAplicado = t.MontoAplicado,
                TiempoAplicado = t.TiempoAplicado

            };
        }

     
    }
}
