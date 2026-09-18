using BLL.Mapping;
using Contracts.DTOs.TipoServicioDTOs;
using DomainLayer.Models;


namespace BLL.MappingMethods
{
    public class TipoServicioMapping : IMappingService<TipoServicio, TipoServicioReadDTO, TipoServicioCreateUpdateDTO>
    {
        public  TipoServicioReadDTO ToReadDTO( TipoServicio t)
        {
            return new TipoServicioReadDTO
            {
                TipoServicioId = t.TipoServicioId,
                Descripcion = t.Descripcion
            };
        }

        public TipoServicio ToUpdateEntity(int id, TipoServicioCreateUpdateDTO dto)
        {
            return new TipoServicio
            {
                TipoServicioId = id,
                Descripcion = dto.Descripcion
            };
        }

        public  TipoServicio ToEntity( TipoServicioCreateUpdateDTO t)
        {
            return new TipoServicio
            {
                Descripcion = t.Descripcion
            };

        }
    }
}
