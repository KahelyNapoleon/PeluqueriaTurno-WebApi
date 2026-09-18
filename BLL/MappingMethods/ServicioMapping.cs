using DomainLayer.Models;
using Contracts.DTOs.ServicioDTOs;
using BLL.Mapping;

namespace BLL.MappingMethods
{
    public  class ServicioMapping : IMappingService<Servicio, ServicioReadDTO, ServicioCreateUpdateDTO>
    {
        public  ServicioReadDTO ToReadDTO( Servicio s)
        {
            return new ServicioReadDTO
            {
                ServicioId = s.ServicioId,
                Descripcion = s.Descripcion,
                Precio = s.Precio,
                Duracion = s.Duracion,
                Observacion = s.Observacion,
                TipoServicioId = s.TipoServicioId
            };
        }

        public Servicio ToUpdateEntity(int id,ServicioCreateUpdateDTO dto)
        {
            return new Servicio
            {
                ServicioId = id,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Duracion = dto.Duracion,
                Observacion = dto.Observacion,
                TipoServicioId = dto.TipoServicioId
            };
        }

        public  Servicio ToEntity( ServicioCreateUpdateDTO s)
        {
            return new Servicio
            {
                Descripcion = s.Descripcion,
                Precio = s.Precio,
                Duracion = s.Duracion,
                Observacion = s.Observacion,
                TipoServicioId = s.TipoServicioId
            };
        }
    }
}
