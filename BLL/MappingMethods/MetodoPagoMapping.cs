using BLL.Mapping;
using Contracts.DTOs.MetodoPagoDTOs;
using DomainLayer.Models;


namespace BLL.MappingMethods
{
    public class MetodoPagoMapping : IMappingService<MetodoPago, MetodoPagoReadDTO, MetodoPagoCreateUpdateDTO>
    {
        public  MetodoPagoReadDTO ToReadDTO( MetodoPago m)
        {
            return new MetodoPagoReadDTO
            {
                MetodoPagoId = m.MetodoPagoId,
                Descripcion = m.Descripcion,
            };
        }

        public MetodoPago ToUpdateEntity(int id, MetodoPagoCreateUpdateDTO dto)
        {
            return new MetodoPago
            {
                MetodoPagoId = id,
                Descripcion = dto.Descripcion
            };
        }

        public  MetodoPago ToEntity( MetodoPagoCreateUpdateDTO m)
        {
            return new MetodoPago
            {
                Descripcion = m.Descripcion
            };
        }
    }
}
